using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor;
using Microsoft.Azure.Management.Monitor.Models;
using Microsoft.Rest.Azure.OData;
using Prometheus.Client;
using Promitor.Scraper.Model;
using Promitor.Scraper.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics.ResouceTypes;
using Promitor.Scraper.Scraping.Exceptions;

namespace Promitor.Scraper.Scraping.ResouceTypes
{
    public class ServiceBusQueueScraper : Scraper<ServiceBusQueueMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(AzureMetadata azureMetadata, AzureCredentials azureCredentials) : base(azureMetadata, azureCredentials)
        {
        }

        protected override async Task ScrapeResourceAsync(MonitorManagementClient monitoringClient, ServiceBusQueueMetricDefinition metricDefinition)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinition.Namespace);
            var foundMetric = await QueryAzureMonitorAsync(monitoringClient, resourceUri, metricDefinition.QueueName, metricDefinition.AzureMetricConfiguration.MetricName, metricDefinition.AzureMetricConfiguration.Aggregation);

            var gauge = Metrics.CreateGauge(metricDefinition.Name, metricDefinition.Description);
            gauge.Set(foundMetric);
        }

        private double ExtractMostRecentMetricData(Metric metric, AggregationType metricAggregation)
        {
            var currentStamp = DateTime.UtcNow;

            var timeSeriesElement = metric?.Timeseries.FirstOrDefault();
            if (timeSeriesElement == null)
            {
                var metricName = metric?.Name?.Value;
                throw new ScrapingException(metricName, $"No time series found for metric '{metricName}'");
            }

            var lastestMetricData = timeSeriesElement.Data.OrderByDescending(measurePoint => measurePoint.TimeStamp)
                .First(measurePoint => measurePoint.TimeStamp < currentStamp);

            switch (metricAggregation)
            {
                case AggregationType.Average:
                    return lastestMetricData.Average ?? -1;
                case AggregationType.Count:
                    return lastestMetricData.Count ?? -1;
                case AggregationType.Maximum:
                    return lastestMetricData.Maximum ?? -1;
                case AggregationType.Minimum:
                    return lastestMetricData.Minimum ?? -1;
                case AggregationType.Total:
                    return lastestMetricData.Total ?? -1;
                case AggregationType.None:
                    return 0;
                default:
                    throw new Exception($"Unable to determine the metrics value for aggregator '{metricAggregation}'");
            }
        }

        private async Task<double> QueryAzureMonitorAsync(MonitorManagementClient monitoringClient, string resourceId, string queueName, string metricName, AggregationType metricAggregation)
        {
            var metricDefinitions = await monitoringClient.MetricDefinitions.ListAsync(resourceId);
            if (metricDefinitions.FirstOrDefault(
                    metric => string.Equals(metric.Name.Value, metricName, StringComparison.InvariantCultureIgnoreCase)) == null)
            {
                Console.WriteLine(value: "Invalid metric");
            }

            var odataFilterMetrics = new ODataQuery<MetadataValue>($"EntityName eq '{queueName}'");

            var metrics = await monitoringClient.Metrics.ListAsync(resourceId, metricnames: metricName, odataQuery: odataFilterMetrics, aggregation: metricAggregation.ToString(), interval: TimeSpan.FromMinutes(value: 1));
            var foundMetric = metrics.Value.FirstOrDefault();
            if (foundMetric == null)
            {
                throw new ScrapingException(metricName, $"No metric was found for '{metricName}'");
            }

            var metricData = ExtractMostRecentMetricData(foundMetric, metricAggregation);

            return metricData;
        }
    }
}