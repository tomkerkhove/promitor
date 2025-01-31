using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class CognitiveServicesAccountsScraper : AzureMonitorScraper<CognitiveServicesAccountsResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.CognitiveServices/accounts/{2}";

        public CognitiveServicesAccountsScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, CognitiveServicesAccountsResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.CognitiveServicesAccountsName);
        }
    }
}
