using System.Collections.Generic;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class DataShareScraper : AzureMonitorScraper<DataShareResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.DataShare/accounts/{2}";

        public DataShareScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, DataShareResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.AccountName);
        }

        protected override string DetermineMetricFilter(DataShareResourceDefinition resourceDefinition)
        {
            var entityName = "*";

            if (IsShareNameConfigured(resourceDefinition))
            {
                entityName = resourceDefinition.ShareName;
            }

            return $"ShareName eq '{entityName}'";
        }

        protected override Dictionary<string, string> DetermineMetricLabels(DataShareResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            if (IsShareNameConfigured(resourceDefinition))
            {
                metricLabels.Add("share_name", resourceDefinition.ShareName);
            }

            return metricLabels;
        }

        private static bool IsShareNameConfigured(DataShareResourceDefinition resourceDefinition)
        {
            return string.IsNullOrWhiteSpace(resourceDefinition.ShareName) == false;
        }
    }
}