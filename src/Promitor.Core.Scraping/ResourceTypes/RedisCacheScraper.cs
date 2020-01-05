﻿using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using System;
using System.Threading.Tasks;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class RedisCacheScraper : Scraper<RedisCacheResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Cache/Redis/{2}";

        public RedisCacheScraper(ScraperConfiguration scraperConfiguration)
           : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, RedisCacheResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.CacheName);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var dimensionName = scrapeDefinition.AzureMetricConfiguration.Dimension?.Name;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName,dimensionName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resource.CacheName, resourceUri, foundMetricValue);
        }
    }
}
