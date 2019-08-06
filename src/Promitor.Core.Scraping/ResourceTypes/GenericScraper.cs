using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Prometheus.Interfaces;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class GenericScraper : Scraper<GenericAzureMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/{2}";

        public GenericScraper(ScraperConfiguration scraperConfiguration, IPrometheusMetricWriter prometheusMetricWriter)
            : base(scraperConfiguration, prometheusMetricWriter)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, GenericAzureMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, metricDefinition.ResourceUri);
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri, metricDefinition.Filter);

            return new ScrapeResult(subscriptionId, resourceGroupName, resourceUri, foundMetricValue);
        }
    }
}