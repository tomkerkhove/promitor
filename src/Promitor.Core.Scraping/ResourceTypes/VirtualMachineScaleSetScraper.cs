using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class VirtualMachineScaleSetScraper : Scraper<VirtualMachineScaleSetResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachineScaleSets/{2}";

        public VirtualMachineScaleSetScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, VirtualMachineScaleSetResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.ScaleSetName);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var dimensionName = scrapeDefinition.AzureMetricConfiguration.Dimension?.Name;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName,dimensionName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resource.ScaleSetName, resourceUri, foundMetricValue);
        }
    }
}