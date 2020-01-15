using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class NetworkInterfaceScraper : AzureMonitorScraper<NetworkInterfaceResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Network/networkInterfaces/{2}";

        public NetworkInterfaceScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, NetworkInterfaceResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.NetworkInterfaceName);
        }
    }
}