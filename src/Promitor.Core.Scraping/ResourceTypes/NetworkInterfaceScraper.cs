using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class NetworkInterfaceScraper : Scraper<NetworkInterfaceResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Network/networkInterfaces/{2}";

        public NetworkInterfaceScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, NetworkInterfaceResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, resourceGroupName, resource.NetworkInterfaceName);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, resourceGroupName, resource.NetworkInterfaceName, resourceUri, foundMetricValue);
        }
    }
}