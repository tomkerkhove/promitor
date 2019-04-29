using System;
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
        private readonly AzureStorageQueueClient _azureStorageQueueClient;
        public StorageQueueScraper(AzureMetadata azureMetadata, AzureMonitorClient azureMonitorClient, ILogger logger, IExceptionTracker exceptionTracker)
            : base(azureMetadata, azureMonitorClient, logger, exceptionTracker)
        {
            _azureStorageQueueClient = new AzureStorageQueueClient(logger);
        }

        protected override async Task<double> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, StorageQueueMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));
            Guard.NotNull(metricDefinition.AzureMetricConfiguration, nameof(metricDefinition.AzureMetricConfiguration));
            Guard.NotNull(metricDefinition.SasToken, nameof(metricDefinition.SasToken));
            Guard.NotNullOrEmpty(metricDefinition.AzureMetricConfiguration.MetricName, nameof(metricDefinition.AzureMetricConfiguration.MetricName));

            var sasToken = metricDefinition.SasToken.GetSecretValue();

            switch (metricDefinition.AzureMetricConfiguration.MetricName.ToLowerInvariant())
            {
                case AzureStorageConstants.Queues.Metrics.TimeSpentInQueue:
                    return await _azureStorageQueueClient.GetQueueMessageTimeSpentInQueueAsync(metricDefinition.AccountName, metricDefinition.QueueName, sasToken);
                case AzureStorageConstants.Queues.Metrics.MessageCount:
                    return await _azureStorageQueueClient.GetQueueMessageCountAsync(metricDefinition.AccountName, metricDefinition.QueueName, sasToken);
                default:
                    throw new InvalidMetricNameException(metricDefinition.AzureMetricConfiguration.MetricName, metricDefinition.ResourceType.ToString());
            }
        }
    }
}