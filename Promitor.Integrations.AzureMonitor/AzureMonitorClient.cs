using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace Promitor.Integrations.AzureMonitor
{
    public class AzureMonitorClient
    {
        private readonly IAzure authenticatedAzureSubscription;

        public AzureMonitorClient(string azureTenantId, string subscriptionId, string applicationId,
            string applicationSecret)
        {
            var credentials = new AzureCredentialsFactory().FromServicePrincipal(applicationId, applicationSecret,
                azureTenantId, AzureEnvironment.AzureGlobalCloud);

            authenticatedAzureSubscription = Azure.Authenticate(credentials)
                .WithSubscription(subscriptionId);
        }

        public async Task<double> QueryMetricAsync(string metricName, AggregationType metricAggregation, string metricFilter, string resourceId)
        {
            try
            {
                // Get all metrics
                var metricsDefinitions =
                    await authenticatedAzureSubscription.MetricDefinitions.ListByResourceAsync(resourceId);

                var metricDefinition =
                    metricsDefinitions.SingleOrDefault(definition => definition.Name.Value == metricName);
                if (metricDefinition == null)
                {
                    throw new Exception("Specified metric is not available");
                }

                var recordDateTime = DateTime.UtcNow;

                var metrics = await metricDefinition.DefineQuery()
                    .StartingFrom(recordDateTime.AddDays(-5))
                    .EndsBefore(recordDateTime)
                    .WithAggregation(metricAggregation.ToString())
                    //.WithOdataFilter(metricFilter)
                    .WithInterval(TimeSpan.FromMinutes(5)) // TODO: Align with cron
                    .ExecuteAsync();

                var relevantMetric = metrics.Metrics.FirstOrDefault();
                if (relevantMetric == null)
                {
                    throw new Exception("No metric was found");
                }

                var timeSeries = relevantMetric.Timeseries.FirstOrDefault();
                if (timeSeries == null)
                {
                    throw new Exception("No time series was found");
                }

                var relevantMetricValue = timeSeries.Data.Where(metricValue => metricValue.TimeStamp < recordDateTime)
                    .OrderByDescending(metricValue => metricValue.TimeStamp)
                    .FirstOrDefault();

                if (relevantMetricValue == null)
                {
                    throw new Exception("No time series was found");
                }

                switch (metricAggregation)
                {
                    case AggregationType.Average:
                        return relevantMetricValue.Average ?? -1;
                    case AggregationType.Count:
                        return relevantMetricValue.Count ?? -1;
                    case AggregationType.Maximum:
                        return relevantMetricValue.Maximum ?? -1;
                    case AggregationType.Minimum:
                        return relevantMetricValue.Minimum ?? -1;
                    case AggregationType.Total:
                        return relevantMetricValue.Total ?? -1;
                    case AggregationType.None:
                        return 0;
                    default:
                        throw new Exception($"Unable to determine the metrics value for aggregator '{metricAggregation}'");
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}