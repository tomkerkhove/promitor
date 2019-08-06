using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class ContainerRegistryScraper : Scraper<ContainerRegistryResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ContainerRegistry/registries/{2}";

        public ContainerRegistryScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, ContainerRegistryResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, resource.RegistryName);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, resourceGroupName, resource.RegistryName, resourceUri, foundMetricValue);
        }
    }
}