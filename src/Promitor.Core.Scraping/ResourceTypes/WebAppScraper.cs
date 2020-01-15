using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class WebAppScraper : AppServiceScraper<WebAppResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Web/sites/{2}";

        public WebAppScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUriWithoutDeploymentSlot(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, WebAppResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.WebAppName);
        }
    }
}