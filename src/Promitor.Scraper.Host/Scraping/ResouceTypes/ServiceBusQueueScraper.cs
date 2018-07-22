using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor;
using Microsoft.Azure.Management.Monitor.Models;
using Microsoft.Rest.Azure.OData;
using Prometheus.Client;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Model.Configuration;
using Promitor.Scraper.Host.Scraping.Exceptions;
using Promitor.Integrations.AzureMonitor;
using Promitor.Scraper.Model;
using Promitor.Scraper.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics.ResouceTypes;

namespace Promitor.Scraper.Host.Scraping.ResouceTypes
{
    public class ServiceBusQueueScraper : Scraper<ServiceBusQueueMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(AzureMetadata azureMetadata, AzureCredentials azureCredentials) : base(azureMetadata, azureCredentials)
        {
        }

        protected override async Task<double> ScrapeResourceAsync(AzureMonitorClient azureMonitorClient, ServiceBusQueueMetricDefinition metricDefinition)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinition.Namespace);

            var filter = $"EntityName eq '{metricDefinition.QueueName}'";
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await azureMonitorClient.QueryMetricAsync(metricName, metricDefinition.AzureMetricConfiguration.Aggregation, resourceUri, filter);

            return foundMetricValue;
        }
    }
}