using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class FunctionAppScraper : Scraper<FunctionAppResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Web/sites/{2}/slots/{3}";

        public FunctionAppScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, FunctionAppResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var slotName = string.IsNullOrWhiteSpace(resource.SlotName) ? "production" : resource.SlotName;
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, scrapeDefinition.ResourceGroupName, resource.FunctionAppName, slotName);

            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var dimensionName = scrapeDefinition.AzureMetricConfiguration.Dimension?.Name;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, dimensionName, aggregationType, aggregationInterval, resourceUri);

            var customLabels = new Dictionary<string, string>
            {
                {"slot_name",slotName }
            };
            
            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resource.FunctionAppName, resourceUri, foundMetricValue, customLabels);
        }
    }
}