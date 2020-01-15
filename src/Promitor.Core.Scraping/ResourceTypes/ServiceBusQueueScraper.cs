using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class ServiceBusQueueScraper : AzureMonitorScraper<ServiceBusQueueResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, ServiceBusQueueResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.Namespace);
        }

        protected override Dictionary<string, string> DetermineMetricLabels(ServiceBusQueueResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            metricLabels.TryAdd("entity_name", resourceDefinition.QueueName);

            return metricLabels;
        }

        protected override string DetermineMetricFilter(ServiceBusQueueResourceDefinition resourceDefinition)
        {
            return $"EntityName eq '{resourceDefinition.QueueName}'";
        }
    }
}