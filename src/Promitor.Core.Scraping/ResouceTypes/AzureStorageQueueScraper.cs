using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureStorageQueue;

namespace Promitor.Core.Scraping.ResouceTypes
{
    public class AzureStorageQueueScraper: Scraper<AzureStorageQueueMetricDefinition>
    {
        private readonly AzureStorageQueue _azureStorageQueue;
        public AzureStorageQueueScraper(AzureMetadata azureMetadata, MetricDefaults metricDefaults, AzureMonitorClient azureMonitorClient, ILogger logger, IExceptionTracker exceptionTracker)
            : base(azureMetadata, metricDefaults, azureMonitorClient, logger, exceptionTracker)
        {
            _azureStorageQueue = new AzureStorageQueue(logger);
        }

        protected override async Task<double> ScrapeResourceAsync(AzureStorageQueueMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            if (metricDefinition.AzureMetricConfiguration.MetricName == "Duration")
            {
                return await _azureStorageQueue.GetLastMessageDurationAsync(metricDefinition.AccountName, metricDefinition.QueueName, metricDefinition.SasToken);
            }
            return await _azureStorageQueue.GetQueueSizeAsync(metricDefinition.AccountName, metricDefinition.QueueName, metricDefinition.SasToken);
        }
    }
}