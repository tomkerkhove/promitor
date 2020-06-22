using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class FunctionAppScraper : AppServiceScraper<FunctionAppResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Web/sites/{2}";

        public FunctionAppScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUriWithoutDeploymentSlot(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, FunctionAppResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.FunctionAppName);
        }
    }
}