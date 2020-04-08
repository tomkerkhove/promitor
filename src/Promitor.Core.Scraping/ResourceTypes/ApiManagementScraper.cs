using System.Collections.Generic;
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
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.InstanceName);
        }

        protected override string DetermineMetricFilter(ApiManagementResourceDefinition resourceDefinition)
        {
            if (string.IsNullOrWhiteSpace(resourceDefinition.LocationName))
            {
                return base.DetermineMetricFilter(resourceDefinition);
            }

            return $"Location eq '{resourceDefinition.LocationName}'";
        }

        protected override Dictionary<string, string> DetermineMetricLabels(ApiManagementResourceDefinition resourceDefinition)
        {
            var labels = base.DetermineMetricLabels(resourceDefinition);

            if (string.IsNullOrWhiteSpace(resourceDefinition.LocationName) == false)
            {
                labels.Add("location", resourceDefinition.LocationName);
            }

            return labels;
        }
    }
}