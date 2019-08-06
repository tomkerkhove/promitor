using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using System;
using System.Threading.Tasks;
using Promitor.Core.Scraping.Prometheus.Interfaces;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class RedisCacheScraper : Scraper<RedisCacheMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Cache/Redis/{2}";

        public RedisCacheScraper(ScraperConfiguration scraperConfiguration, IPrometheusMetricWriter prometheusMetricWriter)
           : base(scraperConfiguration, prometheusMetricWriter)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, RedisCacheMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, metricDefinition.CacheName);

            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, resourceGroupName, metricDefinition.CacheName, resourceUri, foundMetricValue);
        }
    }
}
