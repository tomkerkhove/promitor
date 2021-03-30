﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using GuardNet;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.AzureMonitor.Configuration;
using Promitor.Integrations.AzureMonitor.Exceptions;
using Promitor.Integrations.AzureMonitor.Logging;
using Promitor.Integrations.AzureMonitor.RequestHandlers;
using Promitor.Integrations.Azure.Authentication;

namespace Promitor.Integrations.AzureMonitor
{
    public class AzureMonitorClient
    {
        private readonly ILogger _logger;
        private readonly IAzure _authenticatedAzureSubscription;
        private readonly AzureCredentialsFactory _azureCredentialsFactory = new AzureCredentialsFactory();

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="azureCloud">Name of the Azure cloud to interact with</param>
        /// <param name="tenantId">Id of the tenant that owns the Azure subscription</param>
        /// <param name="subscriptionId">Id of the Azure subscription</param>
        /// <param name="azureAuthenticationInfo">Information regarding authentication with Microsoft Azure</param>
        /// <param name="azureMonitorLoggingConfiguration">Options for Azure Monitor logging</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all configured sinks</param>
        /// <param name="metricsCollector">Metrics collector to write metrics to Prometheus</param>
        /// <param name="loggerFactory">Factory to create loggers with</param>
        public AzureMonitorClient(AzureEnvironment azureCloud, string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, MetricSinkWriter metricSinkWriter, IRuntimeMetricsCollector metricsCollector, ILoggerFactory loggerFactory)
        {
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNull(azureAuthenticationInfo, nameof(azureAuthenticationInfo));
            Guard.NotNull(azureMonitorLoggingConfiguration, nameof(azureMonitorLoggingConfiguration));

            _logger = loggerFactory.CreateLogger<AzureMonitorClient>();
            _authenticatedAzureSubscription = CreateAzureClient(azureCloud, tenantId, subscriptionId, azureAuthenticationInfo, azureMonitorLoggingConfiguration, loggerFactory, metricSinkWriter, metricsCollector);
        }

        /// <summary>
        ///     Queries Azure Monitor to get the latest value for a specific metric
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="metricDimension">Name of dimension to split metric on</param>
        /// <param name="aggregationType">Aggregation for the metric to use</param>
        /// <param name="aggregationInterval">Interval that is used to aggregate metrics</param>
        /// <param name="resourceId">Id of the resource to query</param>
        /// <param name="metricFilter">Optional filter to filter out metrics</param>
        /// <returns>Latest representation of the metric</returns>
        public async Task<List<MeasuredMetric>> QueryMetricAsync(string metricName, string metricDimension, AggregationType aggregationType, TimeSpan aggregationInterval,
            string resourceId, string metricFilter = null)
        {
            Guard.NotNullOrWhitespace(metricName, nameof(metricName));
            Guard.NotNullOrWhitespace(resourceId, nameof(resourceId));

            // Get all metrics
            var startQueryingTime = DateTime.UtcNow;
            var metricsDefinitions = await _authenticatedAzureSubscription.MetricDefinitions.ListByResourceAsync(resourceId);
            var metricDefinition = metricsDefinitions.SingleOrDefault(definition => definition.Name.Value.ToUpper() == metricName.ToUpper());
            if (metricDefinition == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            var closestAggregationInterval = DetermineAggregationInterval(metricName, aggregationInterval, metricDefinition.MetricAvailabilities);

            // Get the most recent metric
            var relevantMetric = await GetRelevantMetric(metricName, aggregationType, closestAggregationInterval, metricFilter, metricDimension, metricDefinition, startQueryingTime);
            if (relevantMetric.Timeseries.Count < 1)
            {
                throw new MetricInformationNotFoundException(metricName, "No time series was found", metricDimension);
            }

            var measuredMetrics = new List<MeasuredMetric>();
            foreach (var timeseries in relevantMetric.Timeseries)
            {
                // Get the most recent value for that metric, that has a finished time series
                // We need to shift the time to ensure that the time series is finalized and not report invalid values
                var maxTimeSeriesTime = startQueryingTime.AddMinutes(closestAggregationInterval.TotalMinutes);

                var mostRecentMetricValue = GetMostRecentMetricValue(metricName, timeseries, maxTimeSeriesTime);

                // Get the metric value according to the requested aggregation type
                var requestedMetricAggregate = InterpretMetricValue(aggregationType, mostRecentMetricValue);

                var measuredMetric = string.IsNullOrWhiteSpace(metricDimension) ? MeasuredMetric.CreateWithoutDimension(requestedMetricAggregate) : MeasuredMetric.CreateForDimension(requestedMetricAggregate, metricDimension, timeseries);
                measuredMetrics.Add(measuredMetric);
            }

            return measuredMetrics;
        }

        private TimeSpan DetermineAggregationInterval(string metricName, TimeSpan requestedAggregationInterval, IReadOnlyList<MetricAvailability> availableMetricPeriods)
        {
            // Get perfect match
            if (availableMetricPeriods.Any(availableMetricPeriod => availableMetricPeriod.TimeGrain == requestedAggregationInterval))
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
                if (availableMetricPeriod.TimeGrain.HasValue == false)
                {
                    continue;
                }

                var periodDifference = availableMetricPeriod.TimeGrain.Value - requestedAggregationInterval;

                if (Math.Abs(periodDifference.TotalSeconds) < Math.Abs(closestAggregationIntervalDifference.TotalSeconds))
                {
                    closestAggregationIntervalDifference = periodDifference;
                    closestAggregationInterval = availableMetricPeriod.TimeGrain.Value;
                }
            }

            return closestAggregationInterval;
        }

        private async Task<IMetric> GetRelevantMetric(string metricName, AggregationType metricAggregation, TimeSpan metricInterval,
            string metricFilter, string metricDimension, IMetricDefinition metricDefinition, DateTime recordDateTime)
        {
            var metricQuery = CreateMetricsQuery(metricAggregation, metricInterval, metricFilter, metricDimension, metricDefinition, recordDateTime);
            var metrics = await metricQuery.ExecuteAsync();

            // We already filtered this out so only expect to have one
            var relevantMetric = metrics.Metrics.SingleOrDefault(var => var.Name.Value.ToUpper() == metricName.ToUpper());
            if (relevantMetric == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            return relevantMetric;
        }

        private MetricValue GetMostRecentMetricValue(string metricName, TimeSeriesElement timeSeries, DateTime recordDateTime)
        {
            var relevantMetricValue = timeSeries.Data.Where(metricValue => metricValue.TimeStamp < recordDateTime)
                .OrderByDescending(metricValue => metricValue.TimeStamp)
                .FirstOrDefault();

            if (relevantMetricValue == null)
            {
                throw new MetricInformationNotFoundException(metricName, $"No time series entry was found recorded before {recordDateTime}");
            }

            return relevantMetricValue;
        }

        private static double? InterpretMetricValue(AggregationType metricAggregation, MetricValue relevantMetricValue)
        {
            switch (metricAggregation)
            {
                case AggregationType.Average:
                    return relevantMetricValue.Average;
                case AggregationType.Count:
                    return relevantMetricValue.Count;
                case AggregationType.Maximum:
                    return relevantMetricValue.Maximum;
                case AggregationType.Minimum:
                    return relevantMetricValue.Minimum;
                case AggregationType.Total:
                    return relevantMetricValue.Total;
                case AggregationType.None:
                    return 0;
                default:
                    throw new Exception($"Unable to determine the metrics value for aggregator '{metricAggregation}'");
            }
        }

        private IWithMetricsQueryExecute CreateMetricsQuery(AggregationType metricAggregation, TimeSpan metricsInterval, string metricFilter, string metricDimension,
            IMetricDefinition metricDefinition, DateTime recordDateTime)
        {
            var metricQuery = metricDefinition.DefineQuery()
                .StartingFrom(recordDateTime.AddDays(-5))
                .EndsBefore(recordDateTime)
                .WithAggregation(metricAggregation.ToString())
                .WithInterval(metricsInterval);

            if (string.IsNullOrWhiteSpace(metricFilter) == false)
            {
                var filter = metricFilter.Replace("/", "%2F");
                metricQuery.WithOdataFilter(filter);
            }

            if (string.IsNullOrWhiteSpace(metricDimension) == false)
            {
                metricQuery.WithOdataFilter($"{metricDimension} eq '*'");
            }

            return metricQuery;
        }

        private IAzure CreateAzureClient(AzureEnvironment azureCloud, string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory, MetricSinkWriter metricSinkWriter, IRuntimeMetricsCollector metricsCollector)
        {
            var credentials = AzureAuthenticationFactory.CreateAzureAuthentication(azureCloud, tenantId, azureAuthenticationInfo, _azureCredentialsFactory);
            var throttlingLogger = loggerFactory.CreateLogger<AzureResourceManagerThrottlingRequestHandler>();
            var monitorHandler = new AzureResourceManagerThrottlingRequestHandler(tenantId, subscriptionId, azureAuthenticationInfo, metricSinkWriter, metricsCollector, throttlingLogger);

            var azureClientConfiguration = Microsoft.Azure.Management.Fluent.Azure.Configure()
                .WithDelegatingHandler(monitorHandler);
            
            var azureMonitorLogging = azureMonitorLoggingConfiguration.Value;
            if (azureMonitorLogging.IsEnabled)
            {
                var integrationLogger = loggerFactory.CreateLogger<AzureMonitorIntegrationLogger>();
                ServiceClientTracing.AddTracingInterceptor(new AzureMonitorIntegrationLogger(integrationLogger));
                ServiceClientTracing.IsEnabled = true;

                azureClientConfiguration = azureClientConfiguration.WithDelegatingHandler(new HttpLoggingDelegatingHandler())
                    .WithLogLevel(azureMonitorLogging.InformationLevel);
            }

            return azureClientConfiguration
                .Authenticate(credentials)
                .WithSubscription(subscriptionId);
        }
    }
}