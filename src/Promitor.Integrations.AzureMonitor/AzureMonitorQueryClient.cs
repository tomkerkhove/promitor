using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.AzureMonitor.Configuration;
using Promitor.Integrations.AzureMonitor.Exceptions;
using Promitor.Integrations.Azure.Authentication;
using Newtonsoft.Json;
using Promitor.Core.Metrics.Exceptions;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using Promitor.Integrations.AzureMonitor.HttpPipelinePolicies;
using Azure.Core;
using Promitor.Core.Extensions;
using Azure.Core.Diagnostics;
using System.Diagnostics.Tracing;
using Promitor.Integrations.AzureMonitor.Extensions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Promitor.Integrations.AzureMonitor
{
    public class AzureMonitorQueryClient : IAzureMonitorClient
    {
        private readonly IOptions<AzureMonitorIntegrationConfiguration> _azureMonitorIntegrationConfiguration;
        private readonly TimeSpan _metricDefinitionCacheDuration = TimeSpan.FromHours(1);
        private readonly MetricsQueryClient _metricsQueryClient; // for single resource queries 
        private readonly MetricsClient _metricsBatchQueryClient; // for batch queries
        private readonly IMemoryCache _resourceMetricDefinitionMemoryCache;
        private readonly ILogger _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="cloudEndpoints">Azure cloud and endpoints to interact with</param>
        /// <param name="tenantId">Id of the tenant that owns the Azure subscription</param>
        /// <param name="subscriptionId">Id of the Azure subscription</param>
        /// <param name="azureAuthenticationInfo">Information regarding authentication with Microsoft Azure</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all configured sinks</param>
        /// <param name="azureScrapingSystemMetricsPublisher">Metrics collector to write metrics to Prometheus</param>
        /// <param name="resourceMetricDefinitionMemoryCache">Memory cache to store items in for performance optimizations</param>
        /// <param name="loggerFactory">Factory to create loggers with</param>
        /// <param name="azureMonitorIntegrationConfiguration">Options for Azure Monitor integration</param>
        /// <param name="azureMonitorLoggingConfiguration">Options for Azure Monitor logging</param>
        public AzureMonitorQueryClient(IAzureCloudEndpoints cloudEndpoints, string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, IMemoryCache resourceMetricDefinitionMemoryCache, ILoggerFactory loggerFactory, IOptions<AzureMonitorIntegrationConfiguration> azureMonitorIntegrationConfiguration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration)
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
            _metricsQueryClient = CreateAzureMonitorMetricsClient(cloudEndpoints, tenantId, subscriptionId, azureAuthenticationInfo, metricSinkWriter, azureScrapingSystemMetricsPublisher, azureMonitorLoggingConfiguration);
            if (_azureMonitorIntegrationConfiguration.Value.MetricsBatching.Enabled)
            {
                _metricsBatchQueryClient = CreateAzureMonitorMetricsBatchClient(cloudEndpoints, tenantId, azureAuthenticationInfo, azureMonitorIntegrationConfiguration, azureMonitorLoggingConfiguration);
            }
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
            var metricNamespaces = await _metricsQueryClient.GetAndCacheMetricNamespacesAsync(resourceId, _resourceMetricDefinitionMemoryCache, _metricDefinitionCacheDuration);
            var metricNamespace = metricNamespaces.SingleOrDefault();
            if (metricNamespace == null)
            {
                throw new MetricNotFoundException(metricName);
            }
            var metricsDefinitions = await _metricsQueryClient.GetAndCacheMetricDefinitionsAsync(resourceId, metricNamespace, _resourceMetricDefinitionMemoryCache, _metricDefinitionCacheDuration); 
            var metricDefinition = metricsDefinitions.SingleOrDefault(definition => definition.Name.ToUpper() == metricName.ToUpper());
            if (metricDefinition == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            var closestAggregationInterval = DetermineAggregationInterval(metricName, aggregationInterval, metricDefinition.MetricAvailabilities);

            // Get the most recent metric
            var metricResult = await _metricsQueryClient.GetRelevantMetricSingleResource(resourceId, metricName, MetricAggregationTypeConverter.AsMetricAggregationType(aggregationType), closestAggregationInterval, metricFilter, metricDimensions, metricLimit, startQueryingTime, _azureMonitorIntegrationConfiguration);
            
            return ProcessMetricResult(metricResult, metricName, startQueryingTime, closestAggregationInterval, aggregationType, metricDimensions);
        }

        public async Task<List<ResourceAssociatedMeasuredMetric>> BatchQueryMetricAsync(string metricName, List<string> metricDimensions, PromitorMetricAggregationType aggregationType, TimeSpan aggregationInterval,
            List<string >resourceIds, string metricFilter = null, int? metricLimit = null) 
        {
            Guard.NotNullOrWhitespace(metricName, nameof(metricName));
            Guard.NotLessThan(resourceIds.Count(), 1, nameof(resourceIds));
            Guard.NotNull(_metricsBatchQueryClient, nameof(_metricsBatchQueryClient));
            
           // Get all metrics
            var startQueryingTime = DateTime.UtcNow;
            var resourceIdsWithMetricDefined = new List<string>();
            

            var metricNamespaces = await _metricsQueryClient.GetAndCacheMetricNamespacesAsync(resourceIds.First(), _resourceMetricDefinitionMemoryCache, _metricDefinitionCacheDuration);
            var metricNamespace = metricNamespaces.SingleOrDefault();
            if (metricNamespace == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            MetricDefinition metricDefinition = null;
            await Task.WhenAll(resourceIds.Select(async resourceId => 
            {
                var metricsDefinitions = await _metricsQueryClient.GetAndCacheMetricDefinitionsAsync(resourceIds.First(), metricNamespace, _resourceMetricDefinitionMemoryCache, _metricDefinitionCacheDuration); 
                var metricDefinition = metricsDefinitions.SingleOrDefault(definition => definition.Name.ToUpper() == metricName.ToUpper());
                if (metricDefinition == null)
                {
                    _logger.LogWarning("Metric {MetricName} is not defined for Resource {ResourceID}. Check Azure Monitor documentation for possible misconfiguration", metricName, resourceId);
                }
                else 
                {
                    resourceIdsWithMetricDefined.Add(resourceId);
                }
            }));

            if (resourceIdsWithMetricDefined.Count < 1) 
            {
                _logger.LogError("Metric {MetricName} is undefined for all resources within batch. Aborting batch job", metricName);
                throw new MetricNotFoundException(metricName);
            }
            
            var closestAggregationInterval = DetermineAggregationInterval(metricName, aggregationInterval, metricDefinition.MetricAvailabilities);

            // Get the most recent metric
            var metricResultsList = await _metricsBatchQueryClient.GetRelevantMetricForResources(resourceIdsWithMetricDefined, metricName, metricNamespace, MetricAggregationTypeConverter.AsMetricAggregationType(aggregationType), closestAggregationInterval, metricFilter, metricDimensions, metricLimit, startQueryingTime, _azureMonitorIntegrationConfiguration, _logger);

            //TODO: This is potentially a lot of results to process in a single thread. Think of ways to utilize additional parallelism
            return metricResultsList
                .SelectMany(metricResult => 
                {
                    try 
                    {
                        return ProcessMetricResult(metricResult, metricName, startQueryingTime, closestAggregationInterval, aggregationType, metricDimensions)
                                                .Select(measuredMetric => measuredMetric.WithResourceIdAssociation(metricResult.ParseResourceIdFromResultId()));
                    }
                    catch (MetricInformationNotFoundException e) 
                    {
                        _logger.LogError("Azure Monitor returned no data for metric {MetricName} for resource {ResourceId} ", metricName, metricResult.ParseResourceIdFromResultId());
                        return [];
                    }
                    catch (Exception e) 
                    {
                        _logger.LogError("Encountered unknown exception when processing metric {MetricName} for resource {ResourceId} ", metricName, metricResult.ParseResourceIdFromResultId());
                        return [];
                    } 
                }) 
                .ToList();
        }

        /// <summary>
        ///     Process metrics query response as time series values using the Promitor data model(MeasuredMetric)
        /// </summary>
        private List<MeasuredMetric> ProcessMetricResult(MetricResult metricResult, string metricName, DateTime startQueryingTime, TimeSpan closestAggregationInterval, PromitorMetricAggregationType aggregationType, List<string> metricDimensions)
        {
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

                // Get the metric value according to the requested aggregation type
                var requestedMetricAggregate = InterpretMetricValue(MetricAggregationTypeConverter.AsMetricAggregationType(aggregationType), mostRecentMetricValue);
                try 
                {
                    var measuredMetric = metricDimensions.Count > 0 
                                ? MeasuredMetric.CreateForDimensions(requestedMetricAggregate, metricDimensions, timeseries) 
                                : MeasuredMetric.CreateWithoutDimensions(requestedMetricAggregate);
                    measuredMetrics.Add(measuredMetric);
                } 
                catch (MissingDimensionException e) 
                {
                    _logger.LogWarning("{MetricName} has returned a time series with empty value for {Dimension} and the measurements will be dropped", metricName, e.DimensionName); 
                    _logger.LogDebug("The violating time series has content {Details}", JsonConvert.SerializeObject(e.TimeSeries)); 
                }
            }

            return measuredMetrics;
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

        private MetricValue GetMostRecentMetricValue(string metricName, MetricTimeSeriesElement timeSeries, DateTimeOffset recordDateTime)
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
        private MetricsQueryClient CreateAzureMonitorMetricsClient(IAzureCloudEndpoints cloudEndpoints, string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration) {
            var metricsQueryClientOptions = new MetricsQueryClientOptions{
                Audience = cloudEndpoints.DetermineMetricsClientAudience(),
            }; 
            var azureRateLimitPolicy = new RecordArmRateLimitMetricsPolicy(tenantId, subscriptionId, azureAuthenticationInfo, metricSinkWriter, azureScrapingSystemMetricsPublisher);
            var addPromitorUserAgentPolicy = new RegisterPromitorAgentPolicy(tenantId, subscriptionId, azureAuthenticationInfo, metricSinkWriter);
            metricsQueryClientOptions.AddPolicy(azureRateLimitPolicy, HttpPipelinePosition.PerCall);
            metricsQueryClientOptions.AddPolicy(addPromitorUserAgentPolicy, HttpPipelinePosition.BeforeTransport);
            var tokenCredential = AzureAuthenticationFactory.GetTokenCredential(nameof(cloudEndpoints.Cloud), tenantId, azureAuthenticationInfo, cloudEndpoints.GetAzureAuthorityHost());
            
            var azureMonitorLogging = azureMonitorLoggingConfiguration.Value;
            if (azureMonitorLogging.IsEnabled)
            {
                using AzureEventSourceListener traceListener = AzureEventSourceListener.CreateTraceLogger(EventLevel.Informational);
                metricsQueryClientOptions.Diagnostics.IsLoggingEnabled = true;
            }
            return new MetricsQueryClient(tokenCredential, metricsQueryClientOptions);
        }

        /// <summary>
        ///     Creates authenticated client for metrics batch queries
        /// </summary>
        private MetricsClient CreateAzureMonitorMetricsBatchClient(IAzureCloudEndpoints cloudEndpoints, string tenantId, AzureAuthenticationInfo azureAuthenticationInfo, IOptions<AzureMonitorIntegrationConfiguration> azureMonitorIntegrationConfiguration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration) {
            var azureRegion = azureMonitorIntegrationConfiguration.Value.MetricsBatching.AzureRegion;
            var metricsClientOptions = new MetricsClientOptions{
                Audience = cloudEndpoints.DetermineMetricsClientBatchQueryAudience(),
                Retry =
                {
                    Mode = RetryMode.Exponential,
                    MaxRetries = 3,
                    Delay = TimeSpan.FromSeconds(1), 
                    MaxDelay = TimeSpan.FromSeconds(30), 
                }
            }; // retry policy as suggested in the documentation: https://learn.microsoft.com/en-us/azure/azure-monitor/essentials/migrate-to-batch-api?tabs=individual-response#529-throttling-errors
            var tokenCredential = AzureAuthenticationFactory.GetTokenCredential(nameof(cloudEndpoints.Cloud), tenantId, azureAuthenticationInfo, cloudEndpoints.GetAzureAuthorityHost());
            metricsClientOptions.AddPolicy(new ModifyOutgoingAzureMonitorRequestsPolicy(_logger), HttpPipelinePosition.BeforeTransport); 
            var azureMonitorLogging = azureMonitorLoggingConfiguration.Value;
            if (azureMonitorLogging.IsEnabled)
            {
                using AzureEventSourceListener traceListener = AzureEventSourceListener.CreateTraceLogger(EventLevel.Informational);
                metricsClientOptions.Diagnostics.IsLoggingEnabled = true;
            }
            _logger.LogInformation("Using batch scraping API URL: {URL}", InsertRegionIntoUrl(azureRegion, cloudEndpoints.DetermineMetricsClientBatchQueryAudience().ToString()));
            return new MetricsClient(new Uri(InsertRegionIntoUrl(azureRegion, cloudEndpoints.DetermineMetricsClientBatchQueryAudience().ToString())), tokenCredential, metricsClientOptions);
        }

        public static string InsertRegionIntoUrl(string region, string baseUrl)
        {
            // Find the position where ".metrics" starts in the URL
            int metricsIndex = baseUrl.IndexOf("metrics", StringComparison.Ordinal);

            // Split the base URL into two parts: before and after the ".metrics"
            string beforeMetrics = baseUrl.Substring(0, metricsIndex);
            string afterMetrics = baseUrl.Substring(metricsIndex);

            // Concatenate the region between the two parts
            return $"{beforeMetrics}{region}.{afterMetrics}";
        }
    }
}