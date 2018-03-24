using System;
using System.Threading.Tasks;
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
            var foundMetric = await QueryAzureMonitorAsync(resourceUri);

            var gauge = Metrics.CreateGauge(metricDefinitionDefinition.Name, metricDefinitionDefinition.Description);
            gauge.Set(foundMetric);
        }

        //private static async Task<MonitorClient> Authenticate(string tenantId, string clientId, string secret, string subscriptionId)
        //{
        //    // Build the service credentials and Monitor client
        //    var serviceCredentials = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, secret);
        //    var monitorClient = new MonitorClient(serviceCredentials)
        //    {
        //        SubscriptionId = subscriptionId
        //    };

        //    return monitorClient;
        //}

        private async Task<double> QueryAzureMonitorAsync(string resourceUri)
        {
            //var metricsClient = await Authenticate(azureMetadata.TenantId, clientId, secret, azureMetadata.SubscriptionId);

            //var metricDefinitions = await metricsClient.MetricDefinitions.ListAsync(resourceUri);

            await Task.CompletedTask;

            return -1.0;
        }
    }
}