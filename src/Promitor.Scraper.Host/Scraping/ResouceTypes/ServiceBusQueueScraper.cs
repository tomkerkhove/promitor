﻿using System.Threading.Tasks;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Integrations.AzureMonitor;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;

namespace Promitor.Scraper.Host.Scraping.ResouceTypes
{
    public class ServiceBusQueueScraper : Scraper<ServiceBusQueueMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(AzureMetadata azureMetadata, AzureCredentials azureCredentials, IExceptionTracker exceptionTracker)
            : base(azureMetadata, azureCredentials, exceptionTracker)
        {
        }

        protected override async Task<double> ScrapeResourceAsync(AzureMonitorClient azureMonitorClient, ServiceBusQueueMetricDefinition metricDefinition)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinition.Namespace);

            var filter = $"EntityName eq '{metricDefinition.QueueName}'";
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await azureMonitorClient.QueryMetricAsync(metricName, metricDefinition.AzureMetricConfiguration.Aggregation, resourceUri, filter);

            return foundMetricValue;
        }
    }
}