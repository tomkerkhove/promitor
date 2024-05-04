using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.AzureMonitor.Configuration;
using Promitor.Integrations.AzureMonitor.Exceptions;
using Promitor.Integrations.Azure.Authentication;
using Newtonsoft.Json;
using Promitor.Core.Metrics.Exceptions;
using Azure.Monitor.Query;
using Promitor.Core.Serialization.Enum;
using Azure.Monitor.Query.Models;
using Promitor.Integrations.AzureMonitor.HttpPipelinePolicies;
using Azure.Core;
using Promitor.Core.Extensions;

namespace Promitor.Integrations.AzureMonitor
{
    public class AzureMonitorQueryClient : IAzureMonitorClient
    {
        private readonly IOptions<AzureMonitorIntegrationConfiguration> _azureMonitorIntegrationConfiguration;
        private readonly TimeSpan _metricDefinitionCacheDuration = TimeSpan.FromHours(1);
        private readonly TimeSpan _metricNamespaceCacheDuration = TimeSpan.FromHours(1);
        private readonly MetricsQueryClient _metricsQueryClient;
        private readonly IMemoryCache _resourceMetricDefinitionMemoryCache;
        private readonly ILogger _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="azureCloud">Name of the Azure cloud to interact with</param>
        /// <param name="tenantId">Id of the tenant that owns the Azure subscription</param>
        /// <param name="subscriptionId">Id of the Azure subscription</param>
        /// <param name="azureAuthenticationInfo">Information regarding authentication with Microsoft Azure</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all configured sinks</param>
        /// <param name="azureScrapingSystemMetricsPublisher">Metrics collector to write metrics to Prometheus</param>
        /// <param name="resourceMetricDefinitionMemoryCache">Memory cache to store items in for performance optimizations</param>
        /// <param name="loggerFactory">Factory to create loggers with</param>
        /// <param name="azureMonitorIntegrationConfiguration">Options for Azure Monitor integration</param>
        /// <param name="azureMonitorLoggingConfiguration">Options for Azure Monitor logging</param>
        public AzureMonitorQueryClient(AzureCloud azureCloud, string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, IMemoryCache resourceMetricDefinitionMemoryCache, ILoggerFactory loggerFactory, IOptions<AzureMonitorIntegrationConfiguration> azureMonitorIntegrationConfiguration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration)
        {
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNull(azureAuthenticationInfo, nameof(azureAuthenticationInfo));
            Guard.NotNull(azureMonitorIntegrationConfiguration, nameof(azureMonitorIntegrationConfiguration));
            Guard.NotNull(azureMonitorLoggingConfiguration, nameof(azureMonitorLoggingConfiguration));
            Guard.NotNull(resourceMetricDefinitionMemoryCache, nameof(resourceMetricDefinitionMemoryCache));

            _resourceMetricDefinitionMemoryCache = resourceMetricDefinitionMemoryCache;
            _azureMonitorIntegrationConfiguration = azureMonitorIntegrationConfiguration;
            _logger = loggerFactory.CreateLogger<AzureMonitorQueryClient>();
            _metricsQueryClient = CreateAzureMonitorMetricsClient(azureCloud, tenantId, subscriptionId, azureAuthenticationInfo, loggerFactory, metricSinkWriter, azureScrapingSystemMetricsPublisher, azureMonitorLoggingConfiguration);
            _logger.LogWarning("Creating query client");
        }

        /// <summary>
        ///     Queries Azure Monitor to get the latest value for a specific metric
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="metricDimensions">List of names of dimensions to split metric on</param>
        /// <param name="aggregationType">Aggregation for the metric to use</param>
        /// <param name="aggregationInterval">Interval that is used to aggregate metrics</param>
        /// <param name="resourceId">Id of the resource to query</param>
        /// <param name="metricFilter">Optional filter to filter out metrics</param>
        /// <param name="metricLimit">Limit of resources to query metrics for when using filtering</param>
        /// <returns>Latest representation of the metric</returns>
        public async Task<List<MeasuredMetric>> QueryMetricAsync(string metricName, List<string> metricDimensions, PromitorMetricAggregationType aggregationType, TimeSpan aggregationInterval,
            string resourceId, string metricFilter = null, int? metricLimit = null)
        {
            Guard.NotNullOrWhitespace(metricName, nameof(metricName));
            Guard.NotNullOrWhitespace(resourceId, nameof(resourceId));
                
            // Get all metrics
            var startQueryingTime = DateTime.UtcNow;
            var metricNamespaces = await GetMetricNamespacesAsync(resourceId);
            var metricNamespace = metricNamespaces.SingleOrDefault();
            if (metricNamespace == null)
            {
                throw new MetricNotFoundException(metricName);
            }
            var metricsDefinitions = await GetMetricDefinitionsAsync(resourceId, metricNamespace); 
            var metricDefinition = metricsDefinitions.SingleOrDefault(definition => definition.Name.ToUpper() == metricName.ToUpper());
            if (metricDefinition == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            var closestAggregationInterval = DetermineAggregationInterval(metricName, aggregationInterval, metricDefinition.MetricAvailabilities);

            // Get the most recent metric
            var metricResult = await GetRelevantMetric(resourceId, metricName, MetricAggregationTypeConverter.AsMetricAggregationType(aggregationType), closestAggregationInterval, metricFilter, metricDimensions, metricLimit, startQueryingTime);
            
            var seriesForMetric = metricResult.TimeSeries;
            if (seriesForMetric.Count < 1)
            {
                throw new MetricInformationNotFoundException(metricName, "No time series was found", metricDimensions);
            } 

            var measuredMetrics = new List<MeasuredMetric>();
            foreach (var timeseries in seriesForMetric)
            {
                // Get the most recent value for that metric, that has a finished time series
                // We need to shift the time to ensure that the time series is finalized and not report invalid values
                var maxTimeSeriesTime = startQueryingTime.AddMinutes(closestAggregationInterval.TotalMinutes);

                var mostRecentMetricValue = GetMostRecentMetricValue(metricName, timeseries, maxTimeSeriesTime);
                string labels = string.Join(" ", timeseries.Metadata.Select(kv => $"{kv.Key}: {kv.Value}"));
                _logger.LogWarning("{labels} labels found with value {value}", labels, mostRecentMetricValue);

                // Get the metric value according to the requested aggregation type
                var requestedMetricAggregate = InterpretMetricValue(MetricAggregationTypeConverter.AsMetricAggregationType(aggregationType), mostRecentMetricValue);
                try 
                {
                    var measuredMetric = metricDimensions.Any() 
                                ? MeasuredMetric.CreateForDimensions(requestedMetricAggregate, metricDimensions, timeseries) 
                                : MeasuredMetric.CreateWithoutDimensions(requestedMetricAggregate);
                    measuredMetrics.Add(measuredMetric);
                } 
                catch (MissingDimensionException e) 
                {
                    _logger.LogWarning("{MetricName} has return a time series with empty value for {Dimension} and the measurements will be dropped", metricName, e.DimensionName); 
                    _logger.LogDebug("The violating time series has content {Details}", JsonConvert.SerializeObject(e.TimeSeries)); 
                }
            }

            return measuredMetrics;
        }

        private async Task<IReadOnlyList<MetricDefinition>> GetMetricDefinitionsAsync(string resourceId, string metricNamespace)
        {
            // Get cached metric definitions
            if (_resourceMetricDefinitionMemoryCache.TryGetValue(resourceId, out IReadOnlyList<MetricDefinition> metricDefinitions))
            {
                return metricDefinitions;
            }
            var metricsDefinitions = new List<MetricDefinition>(); 
            await foreach (var definition in _metricsQueryClient.GetMetricDefinitionsAsync(resourceId, metricNamespace))
            {
                metricsDefinitions.Add(definition);
            }
            
            // Get from API and cache it
            _resourceMetricDefinitionMemoryCache.Set(resourceId, metricsDefinitions, _metricDefinitionCacheDuration);

            return metricsDefinitions;
        }

        private async Task<List<string>> GetMetricNamespacesAsync(string resourceId)
        {
            // Get cached metric namespaces 
            var namespaceKey = $"{resourceId}_namespace";
            if (_resourceMetricDefinitionMemoryCache.TryGetValue(namespaceKey, out List<string> metricNamespaces))
            {
                return metricNamespaces;
            }
            var foundMetricNamespaces = new List<string>(); 
            await foreach (var metricNamespace in _metricsQueryClient.GetMetricNamespacesAsync(resourceId))
            {
                foundMetricNamespaces.Add(metricNamespace.FullyQualifiedName);
            }
            
            // Get from API and cache it
            _resourceMetricDefinitionMemoryCache.Set(namespaceKey, foundMetricNamespaces, _metricDefinitionCacheDuration);

            return foundMetricNamespaces;
        }

        private TimeSpan DetermineAggregationInterval(string metricName, TimeSpan requestedAggregationInterval, IReadOnlyList<MetricAvailability> availableMetricPeriods)
        {
            // Get perfect match
            if (availableMetricPeriods.Any(availableMetricPeriod => availableMetricPeriod.Granularity == requestedAggregationInterval))
            {
                return requestedAggregationInterval;
            }

            var closestAggregationInterval = GetClosestAggregationInterval(requestedAggregationInterval, availableMetricPeriods);

            _logger.LogWarning("{MetricName} will be using {ClosestAggregationInterval} aggregation interval rather than {RequestedAggregationInterval} given it was not available", metricName, closestAggregationInterval.ToString("g"), requestedAggregationInterval.ToString("g"));

            return closestAggregationInterval;
        }

        private static TimeSpan GetClosestAggregationInterval(TimeSpan requestedAggregationInterval, IReadOnlyList<MetricAvailability> availableMetricPeriods)
        {
            var closestAggregationIntervalDifference = TimeSpan.MaxValue;
            var closestAggregationInterval = TimeSpan.MaxValue;

            foreach (var availableMetricPeriod in availableMetricPeriods)
            {
                if (availableMetricPeriod.Granularity.HasValue == false)
                {
                    continue;
                }

                var periodDifference = availableMetricPeriod.Granularity.Value - requestedAggregationInterval;

                if (Math.Abs(periodDifference.TotalSeconds) < Math.Abs(closestAggregationIntervalDifference.TotalSeconds))
                {
                    closestAggregationIntervalDifference = periodDifference;
                    closestAggregationInterval = availableMetricPeriod.Granularity.Value;
                }
            }

            return closestAggregationInterval;
        }

        private async Task<MetricResult> GetRelevantMetric(string resourceId, string metricName, MetricAggregationType metricAggregation, TimeSpan metricInterval,
            string metricFilter, List<string> metricDimensions, int? metricLimit, DateTime recordDateTime)
        {   
            MetricsQueryOptions queryOptions;
            var querySizeLimit = metricLimit ?? Defaults.MetricDefaults.Limit;
            var historyStartingFromInHours = _azureMonitorIntegrationConfiguration.Value.History.StartingFromInHours;
            _logger.LogWarning("metric dimensions: {metricDimensions}", metricDimensions);
            if (metricDimensions.Any())
            {
                var metricDimensionsFilter = string.Join(" and ", metricDimensions.Select(metricDimension => $"{metricDimension} eq '*'"));
                _logger.LogWarning("metricDimensionsFilter {metricDimensionsFilter}", metricDimensionsFilter);
                queryOptions = new MetricsQueryOptions {
                    Aggregations= {
                        metricAggregation
                    }, 
                    Filter = metricDimensionsFilter,
                    Size = querySizeLimit, 
                    TimeRange= new QueryTimeRange(new DateTimeOffset(recordDateTime), new DateTimeOffset(recordDateTime.AddHours(historyStartingFromInHours)))
                };
            } 
            else 
            {
                queryOptions = new MetricsQueryOptions {
                    Aggregations= {
                        metricAggregation
                    }, 
                    Size = querySizeLimit, 
                    TimeRange= new QueryTimeRange(new DateTimeOffset(recordDateTime), new DateTimeOffset(recordDateTime.AddHours(historyStartingFromInHours)))
                };
            }
            
            var metricsQueryResponse = await _metricsQueryClient.QueryResourceAsync(resourceId, [metricName], queryOptions);
            foreach (MetricResult metric in metricsQueryResponse.Value.Metrics)
            {
                foreach (MetricTimeSeriesElement element in metric.TimeSeries)
                {
                    string labels = string.Join(" ", element.Metadata.Select(kv => $"{kv.Key}: {kv.Value}"));
                    _logger.LogWarning("Returned series with name {name} and dimension values {value}", metric.Name, labels);
                }
            }
            var relevantMetric = metricsQueryResponse.Value.Metrics.SingleOrDefault(var => var.Name.ToUpper() == metricName.ToUpper());
            if (relevantMetric == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            return relevantMetric;
        }

        private MetricValue GetMostRecentMetricValue(string metricName, MetricTimeSeriesElement timeSeries, DateTime recordDateTime)
        {
            var relevantMetricValue = timeSeries.Values.Where(metricValue => metricValue.TimeStamp < recordDateTime)
                                                     .MaxBy(metricValue => metricValue.TimeStamp);

            if (relevantMetricValue == null)
            {
                throw new MetricInformationNotFoundException(metricName, $"No time series entry was found recorded before {recordDateTime}");
            }

            return relevantMetricValue;
        }

        private static double? InterpretMetricValue(MetricAggregationType metricAggregation, MetricValue relevantMetricValue)
        {
            switch (metricAggregation)
            {
                case MetricAggregationType.Average:
                    return relevantMetricValue.Average;
                case MetricAggregationType.Count:
                    return relevantMetricValue.Count;
                case MetricAggregationType.Maximum:
                    return relevantMetricValue.Maximum;
                case MetricAggregationType.Minimum:
                    return relevantMetricValue.Minimum;
                case MetricAggregationType.Total:
                    return relevantMetricValue.Total;
                case MetricAggregationType.None:
                    return 0;
                default:
                    throw new Exception($"Unable to determine the metrics value for aggregator '{metricAggregation}'");
            }
        }

        /// <summary>
        ///     Creates authenticated client to query for metrics
        /// </summary>
        private MetricsQueryClient CreateAzureMonitorMetricsClient(AzureCloud azureCloud, string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, ILoggerFactory loggerFactory, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration) {
            var metricsQueryClientOptions = new MetricsQueryClientOptions{
                Audience = azureCloud.DetermineMetricsClientAudience(),
            }; 
            var azureRateLimitPolicy = new RecordArmRateLimitMetricsPolicy(tenantId, subscriptionId, azureAuthenticationInfo, metricSinkWriter, azureScrapingSystemMetricsPublisher);
            var addPromitorUserAgentPolicy = new RegisterPromitorAgentPolicy(tenantId, subscriptionId, azureAuthenticationInfo, metricSinkWriter);
            metricsQueryClientOptions.AddPolicy(azureRateLimitPolicy, HttpPipelinePosition.PerCall);
            metricsQueryClientOptions.AddPolicy(addPromitorUserAgentPolicy, HttpPipelinePosition.BeforeTransport);
            var tokenCredential = AzureAuthenticationFactory.GetTokenCredential(nameof(azureCloud), tenantId, azureAuthenticationInfo, azureCloud.GetAzureAuthorityHost());
            
            
            if (azureAuthenticationInfo.Mode == AuthenticationMode.ServicePrincipal) {
                return new MetricsQueryClient(tokenCredential, metricsQueryClientOptions);
            } else {
                return new MetricsQueryClient(tokenCredential, metricsQueryClientOptions);
            }
        }
    }
}