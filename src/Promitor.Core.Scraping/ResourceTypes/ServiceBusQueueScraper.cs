using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class ServiceBusQueueScraper : Scraper<ServiceBusQueueResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, ServiceBusQueueResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.Namespace);

            var filter = $"EntityName eq '{resource.QueueName}'";
            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var dimensionName = scrapeDefinition.AzureMetricConfiguration.DimensionName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName,dimensionName, aggregationType, aggregationInterval, resourceUri, filter);

            var labels = new Dictionary<string, string>
            {
                {"entity_name", resource.QueueName}
            };

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resource.Namespace, resourceUri, foundMetricValue, labels);
        }
    }
}