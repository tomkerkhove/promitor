using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping.ResouceTypes
{
    public class ServiceBusQueueScraper : Scraper<ServiceBusQueueMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        public ServiceBusQueueScraper(AzureMetadata azureMetadata, MetricDefaults metricDefaults, AzureMonitorClient azureMonitorClient, ILogger logger, IExceptionTracker exceptionTracker)
            : base(azureMetadata, metricDefaults, azureMonitorClient, logger, exceptionTracker)
        {
        }

        protected override async Task<double> ScrapeResourceAsync(ServiceBusQueueMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinition.Namespace);

            var filter = $"EntityName eq '{metricDefinition.QueueName}'";
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri, filter);

            return foundMetricValue;
        }
    }
}