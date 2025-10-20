using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    /// Scrapes an Azure DNS Zone
    /// </summary>
    public class DnsZoneScraper : AzureMonitorScraper<DnsZoneResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Network/dnszones/{2}";

        public DnsZoneScraper(ScraperConfiguration scraperConfiguration) : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, DnsZoneResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.ZoneName);
        }
    }
}