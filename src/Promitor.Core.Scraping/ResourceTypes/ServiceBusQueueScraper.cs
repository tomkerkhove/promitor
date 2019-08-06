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

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, ScrapeDefinition<AzureResourceDefinition> scrapeDefinition, ServiceBusQueueResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, resource.Namespace);

            var filter = $"EntityName eq '{resource.QueueName}'";
            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri, filter);

            var labels = new Dictionary<string, string>
            {
                {"entity_name", resource.QueueName}
            };

            return new ScrapeResult(subscriptionId, resourceGroupName, resource.Namespace, resourceUri, foundMetricValue, labels);
        }
    }
}