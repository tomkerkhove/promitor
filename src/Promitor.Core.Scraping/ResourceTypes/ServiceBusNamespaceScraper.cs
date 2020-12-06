using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class ServiceBusNamespaceScraper : AzureMessagingScraper<ServiceBusNamespaceResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusNamespaceScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, ServiceBusNamespaceResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.Namespace);
        }

        protected override bool IsEntityDeclared(ServiceBusNamespaceResourceDefinition resourceDefinition)
        {
            if (string.IsNullOrWhiteSpace(resourceDefinition.QueueName) == false)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(resourceDefinition.TopicName) == false)
            {
                return true;
            }

            return false;
        }

        protected override string GetEntityName(ServiceBusNamespaceResourceDefinition resourceDefinition)
        {
            if (string.IsNullOrWhiteSpace(resourceDefinition.QueueName) == false)
            {
                return resourceDefinition.QueueName;
            }

            return resourceDefinition.TopicName;
        }
    }
}