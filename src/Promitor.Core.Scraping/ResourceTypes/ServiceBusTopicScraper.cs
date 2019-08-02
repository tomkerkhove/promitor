using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class ServiceBusTopicScraper : ServiceBusScraper<ServiceBusTopicMetricDefinition>
    {
        public ServiceBusTopicScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, ServiceBusTopicMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = GetResourceUri(subscriptionId, resourceGroupName, metricDefinition.Namespace);

            var filter = $"EntityName eq '{metricDefinition.TopicName}'";
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri, filter);

            var labels = new Dictionary<string, string>
            {
                {"entity_name", metricDefinition.TopicName},
                {"subscription_name", metricDefinition.SubscriptionName}
            };

            return new ScrapeResult(subscriptionId, resourceGroupName, metricDefinition.Namespace, resourceUri, foundMetricValue, labels);
        }
    }
}