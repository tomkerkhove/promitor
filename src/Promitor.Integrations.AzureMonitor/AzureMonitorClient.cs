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
using Promitor.Integrations.AzureMonitor.Exceptions;

namespace Promitor.Integrations.AzureMonitor
{
    public class AzureMonitorClient
    {
        public const double NO_DATA = 0.0D;

        private readonly IAzure _authenticatedAzureSubscription;
        private readonly AzureCredentialsFactory _azureCredentialsFactory = new AzureCredentialsFactory();

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tenantId">Id of the tenant that owns the Azure subscription</param>
        /// <param name="subscriptionId">Id of the Azure subscription</param>
        /// <param name="applicationId">Id of the Azure AD application used to authenticate with Azure Monitor</param>
        /// <param name="applicationSecret">Secret to authenticate with Azure Monitor for the specified Azure AD application</param>
        public AzureMonitorClient(string tenantId, string subscriptionId, string applicationId,
            string applicationSecret)
        {
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNullOrWhitespace(applicationId, nameof(applicationId));
            Guard.NotNullOrWhitespace(applicationSecret, nameof(applicationSecret));

            var credentials = _azureCredentialsFactory.FromServicePrincipal(applicationId, applicationSecret, tenantId, AzureEnvironment.AzureGlobalCloud);

            _authenticatedAzureSubscription = Azure.Authenticate(credentials).WithSubscription(subscriptionId);
        }

        /// <summary>
        ///     Queries Azure Monitor to get the latest value for a specific metric
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="metricAggregation">Aggregation for the metric to use</param>
        /// <param name="resourceId">Id of the resource to query</param>
        /// <param name="metricFilter">Optional filter to filter out metrics</param>
        /// <returns>Latest representation of the metric</returns>
        public async Task<double> QueryMetricAsync(string metricName, AggregationType metricAggregation,
            string resourceId, DataGranularityDescriptor dataGranularity, string metricFilter = null)
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

            var dataGranularityTimeSpan = GetDataGranularity(dataGranularity, metricDefinition);
            double result = await ReadDataPoint(metricName, metricAggregation, metricFilter, metricDefinition, dataGranularityTimeSpan);          
            return result;
        }

        public async Task<double> ReadDataPoint(string metricName, AggregationType metricAggregation, string metricFilter, IMetricDefinition metricDefinition, TimeSpan dataGranularity)
        {
            var recordDateTime = DateTime.UtcNow;

            // Get the most recent metric
            var relevantMetric = await GetRelevantMetric(metricName, metricAggregation, metricFilter, metricDefinition, recordDateTime, dataGranularity);

            // Get the most recent value for that metric
            var mostRecentMetricValue = GetMostRecentMetricValue(metricName, relevantMetric.Timeseries, recordDateTime);

            // Get the metric value according to the requested aggregation type
            var requestMetricAggregate = InterpretMetricValue(metricAggregation, mostRecentMetricValue);

            return requestMetricAggregate;
        }

        private async Task<IMetric> GetRelevantMetric(string metricName, AggregationType metricAggregation,
            string metricFilter, IMetricDefinition metricDefinition, DateTime recordDateTime, TimeSpan dataGranularity)
        {
            var metricQuery = CreateMetricsQuery(metricAggregation, metricFilter, metricDefinition, recordDateTime, dataGranularity);
            var metrics = await metricQuery.ExecuteAsync();

            // We already filtered this out so only expect to have one
            var relevantMetric = metrics.Metrics.SingleOrDefault(var => var.Name.Value.ToUpper() == metricName.ToUpper());
            if (relevantMetric == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            return relevantMetric;
        }

        private MetricValue GetMostRecentMetricValue(string metricName, IReadOnlyList<TimeSeriesElement> timeSeries,
            DateTime recordDateTime)
        {
            var timeSerie = timeSeries.FirstOrDefault(); // TODO: Can we really do this?
            if (timeSerie == null)
            {
                throw new MetricInformationNotFoundException(metricName, "No time series was found");
            }

            var relevantMetricValue = timeSerie.Data.Where(metricValue => metricValue.TimeStamp < recordDateTime)
                .OrderByDescending(metricValue => metricValue.TimeStamp)
                .FirstOrDefault();

            if (relevantMetricValue == null)
            {
                throw new MetricInformationNotFoundException(metricName, "No time series entry was found");
            }

            return relevantMetricValue;
        }

        private double InterpretMetricValue(AggregationType metricAggregation, MetricValue relevantMetricValue)
        {
            switch (metricAggregation)
            {
                case AggregationType.Average:
                    return relevantMetricValue.Average ?? NO_DATA;
                case AggregationType.Count:
                    return relevantMetricValue.Count ?? NO_DATA;
                case AggregationType.Maximum:
                    return relevantMetricValue.Maximum ?? NO_DATA;
                case AggregationType.Minimum:
                    return relevantMetricValue.Minimum ?? NO_DATA;
                case AggregationType.Total:
                    return relevantMetricValue.Total ?? NO_DATA;
                case AggregationType.None:
                    return 0.0D; // TODO: What is the behavior for None aggregation?
                default:
                    throw new Exception($"Unable to determine the metrics value for aggregator '{metricAggregation}'");
            }
        }

        private IWithMetricsQueryExecute CreateMetricsQuery(AggregationType metricAggregation, string metricFilter,
            IMetricDefinition metricDefinition, DateTime recordDateTime, TimeSpan lowestGranularity)
        {
            DateTime startTime = recordDateTime.AddSeconds(-4.0D * lowestGranularity.TotalSeconds); // TODO: Four is a arbitrary buffer factor

            var metricQuery = metricDefinition.DefineQuery()
                .StartingFrom(startTime)
                .EndsBefore(recordDateTime)
                .WithAggregation(metricAggregation.ToString())
                .WithInterval(lowestGranularity);

            if (string.IsNullOrWhiteSpace(metricFilter) == false)
            {
                metricQuery.WithOdataFilter(metricFilter);
            }

            return metricQuery;
        }

        private static TimeSpan GetDataGranularity(DataGranularityDescriptor dataGranularity, IMetricDefinition metricDefinition)
        {
            if (dataGranularity.DataGranularity == DataGranularity.Lowest)
            {
                MetricAvailability avail = metricDefinition.MetricAvailabilities
                           .Where(x => x.TimeGrain.HasValue)
                           .OrderBy(x => x.TimeGrain.Value)
                           .FirstOrDefault();

                if (avail == null)
                {
                    throw new Exception($"Metric {metricDefinition.Name} has no metric availability (data granularity) defined");
                }

                return avail.TimeGrain.Value;
            }
            else if (dataGranularity.DataGranularity == DataGranularity.Specified)
            {
                return dataGranularity.SpecifiedTimeSpan;
            }
            else
            {
                throw new Exception("Do not know how to handle this data granularity type");
            }
        }
    }
}