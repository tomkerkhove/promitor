using System;
using System.Threading.Tasks;
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
        public StorageQueueScraper(AzureMetadata azureMetadata, MetricDefaults metricDefaults, AzureMonitorClient azureMonitorClient, ILogger logger, IExceptionTracker exceptionTracker)
            : base(azureMetadata, metricDefaults, azureMonitorClient, logger, exceptionTracker)
        {
            _azureStorageQueueClient = new AzureStorageQueueClient(logger);
        }

        protected override async Task<double> ScrapeResourceAsync(StorageQueueMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            GuardNet.Guard.NotNull(metricDefinition, nameof(metricDefinition));
            GuardNet.Guard.NotNull(metricDefinition.AzureMetricConfiguration, nameof(metricDefinition.AzureMetricConfiguration));
            GuardNet.Guard.NotNullOrEmpty(metricDefinition.AzureMetricConfiguration.MetricName, nameof(metricDefinition.AzureMetricConfiguration.MetricName));

            switch (metricDefinition.AzureMetricConfiguration.MetricName.ToLowerInvariant())
            {
                case AzureStorageConstants.Queues.Metrics.TimeSpentInQueue:
                    return await _azureStorageQueueClient.GetQueueMessageTimeSpentInQueueAsync(metricDefinition.AccountName, metricDefinition.QueueName, metricDefinition.SasToken);
                case AzureStorageConstants.Queues.Metrics.MessageCount:
                    return await _azureStorageQueueClient.GetQueueMessageCountAsync(metricDefinition.AccountName, metricDefinition.QueueName, metricDefinition.SasToken);
                default:
                    throw new InvalidMetricNameException(metricDefinition.AzureMetricConfiguration.MetricName, metricDefinition.ResourceType.ToString());
            }
        }
    }
}