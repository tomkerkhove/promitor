using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Prometheus.Interfaces;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class ServiceBusQueueScraper : Scraper<ServiceBusQueueMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(ScraperConfiguration scraperConfiguration, IPrometheusMetricWriter prometheusMetricWriter)
            : base(scraperConfiguration, prometheusMetricWriter)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, ServiceBusQueueMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, metricDefinition.Namespace);

            var filter = $"EntityName eq '{metricDefinition.QueueName}'";
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri, filter);

            var labels = new Dictionary<string, string>
            {
                {"entity_name", metricDefinition.QueueName}
            };

            return new ScrapeResult(subscriptionId, resourceGroupName, metricDefinition.Namespace, resourceUri, foundMetricValue, labels);
        }
    }
}