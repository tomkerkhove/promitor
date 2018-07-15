using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent;
using Prometheus.Client;
using Promitor.Integrations.AzureMonitor;
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

        protected override async Task ScrapeResourceAsync(MonitorManagementClient monitoringClient, ServiceBusQueueMetricDefinition metricDefinition)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinition.Namespace);

            // TODO: Inject
            var monitorClient = new AzureMonitorClient(AzureMetadata.TenantId, AzureMetadata.SubscriptionId, this.AzureCredentials.ApplicationId, this.AzureCredentials.Secret);

            var filter = $"EntityName eq '{metricDefinition.QueueName}'";
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetric = await monitorClient.QueryMetricAsync(metricName, metricDefinition.AzureMetricConfiguration.Aggregation, filter, resourceUri);
            
            var gauge = Metrics.CreateGauge(metricDefinition.Name, metricDefinition.Description);
            gauge.Set(foundMetric);
        }
    }
}