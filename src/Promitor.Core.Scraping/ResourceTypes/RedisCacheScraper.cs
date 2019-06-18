using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;
using System;
using System.Threading.Tasks;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class RedisCacheScraper : Scraper<RedisCacheMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Cache/Redis/{2}";

        public RedisCacheScraper(AzureMetadata azureMetadata, AzureMonitorClient azureMonitorClient, ILogger logger, IExceptionTracker exceptionTracker)
           : base(azureMetadata, azureMonitorClient, logger, exceptionTracker)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, RedisCacheMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, metricDefinition.CacheName);

            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(resourceUri, foundMetricValue);
        }
    }
}
