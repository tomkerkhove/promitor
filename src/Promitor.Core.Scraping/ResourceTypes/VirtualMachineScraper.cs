using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class VirtualMachineScraper : Scraper<VirtualMachineResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachines/{2}";

        public VirtualMachineScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, VirtualMachineResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, resourceGroupName, resource.VirtualMachineName);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, resourceGroupName, resource.VirtualMachineName, resourceUri, foundMetricValue);
        }
    }
}