using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureQueue;

namespace Promitor.Core.Scraping.ResouceTypes
{
    public class AzureQueueScraper: Scraper<AzureQueueMetricDefinition>
    {
        private readonly AzureQueue _azureQueue;
        public AzureQueueScraper(AzureMetadata azureMetadata, MetricDefaults metricDefaults, AzureMonitorClient azureMonitorClient, ILogger logger, IExceptionTracker exceptionTracker)
            : base(azureMetadata, metricDefaults, azureMonitorClient, logger, exceptionTracker)
        {
            _azureQueue = new AzureQueue(logger);
        }

        protected override async Task<double> ScrapeResourceAsync(AzureQueueMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            return await _azureQueue.GetQueueSizeAsync(metricDefinition.AccountName, metricDefinition.QueueName, metricDefinition.SasToken);
        }
    }
}