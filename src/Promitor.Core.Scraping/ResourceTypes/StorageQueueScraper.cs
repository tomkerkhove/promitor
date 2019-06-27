using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureStorage;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class StorageQueueScraper : Scraper<StorageQueueMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Storage/storageAccounts/{2}/queueServices";
        private readonly AzureStorageQueueClient _azureStorageQueueClient;
        public StorageQueueScraper(AzureMetadata azureMetadata, AzureMonitorClient azureMonitorClient, ILogger logger, IExceptionTracker exceptionTracker)
            : base(azureMetadata, azureMonitorClient, logger, exceptionTracker)
        {
            _azureStorageQueueClient = new AzureStorageQueueClient(logger);
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, StorageQueueMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));
            Guard.NotNull(metricDefinition.AzureMetricConfiguration, nameof(metricDefinition.AzureMetricConfiguration));
            Guard.NotNull(metricDefinition.SasToken, nameof(metricDefinition.SasToken));
            Guard.NotNullOrEmpty(metricDefinition.AzureMetricConfiguration.MetricName, nameof(metricDefinition.AzureMetricConfiguration.MetricName));

            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, metricDefinition.AccountName);
            var sasToken = metricDefinition.SasToken.GetSecretValue();
            double foundMetricValue;

            switch (metricDefinition.AzureMetricConfiguration.MetricName.ToLowerInvariant())
            {
                case AzureStorageConstants.Queues.Metrics.TimeSpentInQueue:
                    foundMetricValue = await _azureStorageQueueClient.GetQueueMessageTimeSpentInQueueAsync(metricDefinition.AccountName, metricDefinition.QueueName, sasToken);
                    break;
                case AzureStorageConstants.Queues.Metrics.MessageCount:
                    foundMetricValue = await _azureStorageQueueClient.GetQueueMessageCountAsync(metricDefinition.AccountName, metricDefinition.QueueName, sasToken);
                    break;
                default:
                    throw new InvalidMetricNameException(metricDefinition.AzureMetricConfiguration.MetricName, metricDefinition.ResourceType.ToString());
            }

            var labels = new Dictionary<string, string>
            {
                {"queue_name", metricDefinition.QueueName}
            };

            return new ScrapeResult(subscriptionId, resourceGroupName, metricDefinition.AccountName, resourceUri, foundMetricValue, labels);
        }
    }
}