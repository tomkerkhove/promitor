using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    ///     Scrapes an Azure API Management instance.
    /// </summary>
    public class ApiManagementScraper : AzureMonitorScraper<ApiManagementResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ApiManagement/service/{2}";

        public ApiManagementScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, ApiManagementResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.InstanceName);
        }

        protected override string DetermineMetricFilter(ApiManagementResourceDefinition resourceDefinition)
        {
            return "HostName eq '*'";
        }
    }
}