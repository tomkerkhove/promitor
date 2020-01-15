using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class SqlManagedInstanceScraper : AzureMonitorScraper<SqlManagedInstanceResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Sql/managedInstances/{2}";

        public SqlManagedInstanceScraper(ScraperConfiguration scraperConfiguration) : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, SqlManagedInstanceResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.InstanceName);
        }
    }
}