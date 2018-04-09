using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor;
using Microsoft.Azure.Management.Monitor.Models;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Rest.Azure.OData;
using Newtonsoft.Json;
using Prometheus.Client;
using Promitor.Scraper.Model;
using Promitor.Scraper.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics.ResouceTypes;

namespace Promitor.Scraper.Scraping.ResouceTypes
{
    public class ServiceBusQueueScraper : Scraper<ServiceBusQueueMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(AzureMetadata azureMetadata, AzureCredentials azureCredentials) : base(azureMetadata, azureCredentials)
        {
        }

        protected override async Task ScrapeResourceAsync(ServiceBusQueueMetricDefinition metricDefinitionDefinition)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinitionDefinition.Namespace);
            var foundMetric = await QueryAzureMonitorAsync(resourceUri, metricDefinitionDefinition.QueueName);

            var gauge = Metrics.CreateGauge(metricDefinitionDefinition.Name, metricDefinitionDefinition.Description);
            gauge.Set(foundMetric);
        }

        private static void EnumerateMetrics(Response metrics, string armId, string entityName, int maxRecords = 60)
        {
            Console.WriteLine(
                "Cost: {0}\r\nTimespan: {1}\r\nInterval: {2}\r\n",
                metrics.Cost,
                metrics.Timespan,
                metrics.Interval);

            var numRecords = 0;
            Console.WriteLine("Printing metrics for Resource " + armId);
            foreach (var metric in metrics.Value)
            {
                foreach (var timeSeries in metric.Timeseries)
                {
                    // Use Average and multiplier for bigger time ranges than one minute and when observing bigger time ranges than 5 minutes.
                    // Use Total for short time ranges and 1 minute interval for observing e.g. one hour worth of data and decide to automatically scale receivers or senders.
                    foreach (var data in timeSeries.Data)
                    {
                        Console.WriteLine($"{entityName}\t{metric.Name.Value}\t{data.TimeStamp}\t{metric.Name.LocalizedValue}\t{data.Total}");
                    }
                }

                numRecords++;
                if (numRecords >= maxRecords)
                {
                    break;
                }
            }
        }

        private async Task<ServiceClientCredentials> AuthenticateWithAzureAsync()
        {
            return await ApplicationTokenProvider.LoginSilentAsync(AzureMetadata.TenantId, AzureCredentials.ApplicationId, AzureCredentials.Secret);
        }

        private async Task<double> QueryAzureMonitorAsync(string resourceId, string queueName)
        {
            var metricName = "ActiveMessages"; // Valid metrics "IncomingMessages,IncomingRequests,ActiveMessages,Messages,Size"            
            var aggregation = "Total"; // Valid aggregations: Total and Average

            // Create new Metrics token and Management client.
            var azureServiceCredentials = await AuthenticateWithAzureAsync();
            var monitoringClient = new MonitorManagementClient(azureServiceCredentials);

            var metricDefinitions = await monitoringClient.MetricDefinitions.ListAsync(resourceId);
            if (metricDefinitions.FirstOrDefault(
                    metric => string.Equals(metric.Name.Value, metricName, StringComparison.InvariantCultureIgnoreCase)) == null)
            {
                Console.WriteLine("Invalid metric");
            }
            var odataFilterMetrics = new ODataQuery<MetadataValue>($"EntityName eq '{queueName}'");

            // Use this as quick and easy way to understand what metrics are emitted and what to query for. 
            // When looking for the count and size of an entity the only supported way is using total and 1 minute time slices.
            // Accessing those metrics via code is mostly for auto scaling purposes on sender and receiver side.
            //var metrics1 = await monitoringClient.Metrics.ListAsync(resourceId, metricnames: metricName, odataQuery: odataFilterMetrics, timespan: timeSpan, aggregation: aggregation, interval: TimeSpan.FromMinutes(value: 1));
            var metrics = await monitoringClient.Metrics.ListAsync(resourceId, metricnames: metricName, odataQuery: odataFilterMetrics, aggregation: aggregation, interval: TimeSpan.FromMinutes(value: 1));
            var rawMetrics = JsonConvert.SerializeObject(metrics, Formatting.Indented);
            Console.WriteLine(rawMetrics);
            EnumerateMetrics(metrics, resourceId, queueName);

            var metricData = GetLatestMetricData(metrics);

            return metricData;
        }

        private double GetLatestMetricData(Response metrics)
        {
            var currentStamp = DateTime.UtcNow;
            var metric = metrics.Value.First();
            var timeSeriesElement = metric.Timeseries.First();
            var lastestMetricData = timeSeriesElement.Data.OrderByDescending(measurePoint => measurePoint.TimeStamp).FirstOrDefault(measurePoint => measurePoint.TimeStamp < currentStamp);

            Console.WriteLine($"{metric.Name.Value}\t{lastestMetricData.TimeStamp}\t{metric.Name.LocalizedValue}\t{lastestMetricData.Total}");

            return lastestMetricData.Total ?? -1;
        }
    }
}