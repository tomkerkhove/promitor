using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Prometheus.Interfaces;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class NetworkInterfaceScraper : Scraper<NetworkInterfaceMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Network/networkInterfaces/{2}";

        public NetworkInterfaceScraper(ScraperConfiguration scraperConfiguration, IPrometheusMetricWriter prometheusMetricWriter)
            : base(scraperConfiguration, prometheusMetricWriter)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, NetworkInterfaceMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinition.NetworkInterfaceName);

            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, resourceGroupName, metricDefinition.NetworkInterfaceName, resourceUri, foundMetricValue);
        }
    }
}