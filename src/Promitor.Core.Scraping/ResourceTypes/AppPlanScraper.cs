using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class AppPlanScraper : AzureMonitorScraper<AppPlanResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Web/serverfarms/{2}";

        public AppPlanScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, AppPlanResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.AppPlanName);
        }
    }
}