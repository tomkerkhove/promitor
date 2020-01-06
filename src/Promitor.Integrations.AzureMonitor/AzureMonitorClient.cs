using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Telemetry.Metrics.Interfaces;
using Promitor.Integrations.AzureMonitor.Exceptions;
using Promitor.Integrations.AzureMonitor.RequestHandlers;

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
        /// <param name="applicationId">Id of the Azure AD application used to authenticate with Azure Monitor</param>
        /// <param name="applicationSecret">Secret to authenticate with Azure Monitor for the specified Azure AD application</param>
        /// <param name="runtimeMetricsCollector">Metrics collector for our runtime</param>
        /// <param name="logger">Logger to use during interaction with Azure Monitor</param>
        public AzureMonitorClient(AzureEnvironment azureCloud, string tenantId, string subscriptionId, string applicationId, string applicationSecret, IRuntimeMetricsCollector runtimeMetricsCollector, ILogger logger)
        {
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNullOrWhitespace(applicationId, nameof(applicationId));
            Guard.NotNullOrWhitespace(applicationSecret, nameof(applicationSecret));

            var credentials = _azureCredentialsFactory.FromServicePrincipal(applicationId, applicationSecret, tenantId, azureCloud);

            var monitorHandler = new AzureResourceManagerThrottlingRequestHandler(tenantId, subscriptionId, applicationId, runtimeMetricsCollector, logger);
            _authenticatedAzureSubscription = Azure.Configure().WithDelegatingHandler(monitorHandler).Authenticate(credentials).WithSubscription(subscriptionId);
            _logger = logger;
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
            var metricsDefinitions = await _authenticatedAzureSubscription.MetricDefinitions.ListByResourceAsync(resourceId);
            var metricDefinition = metricsDefinitions.SingleOrDefault(definition => definition.Name.Value.ToUpper() == metricName.ToUpper());
            if (metricDefinition == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            var recordDateTime = DateTime.UtcNow;

            var closestAggregationInterval = DetermineAggregationInterval(metricName, aggregationInterval, metricDefinition.MetricAvailabilities);

            // Get the most recent metric
            var relevantMetric = await GetRelevantMetric(metricName, aggregationType, closestAggregationInterval, metricFilter, metricDimension, metricDefinition, recordDateTime);
            if (relevantMetric.Timeseries.Count < 1)
            {
                throw new MetricInformationNotFoundException(metricName, "No time series was found");
            }

            var measuredMetrics = new List<MeasuredMetric>();
            foreach (var timeseries in relevantMetric.Timeseries)
            {
                // Get the most recent value for that metric
                var mostRecentMetricValue = GetMostRecentMetricValue(metricName, timeseries, recordDateTime);

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

        private MetricValue GetMostRecentMetricValue(string metricName, TimeSeriesElement timeSeries,
            DateTime recordDateTime)
        {
            var relevantMetricValue = timeSeries.Data.Where(metricValue => metricValue.TimeStamp < recordDateTime)
                .OrderByDescending(metricValue => metricValue.TimeStamp)
                .FirstOrDefault();

            if (relevantMetricValue == null)
            {
                throw new MetricInformationNotFoundException(metricName, "No time series entry was found");
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
                metricQuery.WithOdataFilter(metricFilter);
            }

            if (string.IsNullOrWhiteSpace(metricDimension) == false)
            {
                metricQuery.WithOdataFilter($"{metricDimension} eq '*'");
            }

            return metricQuery;
        }
    }
}