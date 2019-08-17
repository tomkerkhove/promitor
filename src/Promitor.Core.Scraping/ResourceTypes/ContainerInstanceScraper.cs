using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class ContainerInstanceScraper : Scraper<ContainerInstanceResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ContainerInstance/containerGroups/{2}";

        public ContainerInstanceScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, ContainerInstanceResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.ContainerGroup);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resource.ContainerGroup, resourceUri, foundMetricValue);
        }
    }
}