using System;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Factories;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Agents.Scraper.Scheduling
{
    public class ResourceScrapingJob : MetricScrapingJob,
        IScheduledJob
    {
        private readonly ScrapeDefinition<IAzureResourceDefinition> _metricScrapeDefinition;
        private readonly AzureMonitorClient _azureMonitorClient;
        private readonly MetricSinkWriter _metricSinkWriter;

        private readonly MetricScraperFactory _metricScraperFactory;

        public ResourceScrapingJob(string jobName,
            ScrapeDefinition<IAzureResourceDefinition> metricScrapeDefinition,
            MetricSinkWriter metricSinkWriter,
            MetricScraperFactory metricScraperFactory,
            AzureMonitorClient azureMonitorClient,
            ILogger<ResourceScrapingJob> logger)
            : base(jobName, logger)
        {
            Guard.NotNull(metricScrapeDefinition, nameof(metricScrapeDefinition));
            Guard.NotNull(metricScraperFactory, nameof(metricScraperFactory));
            Guard.NotNull(azureMonitorClient, nameof(azureMonitorClient));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));

            _metricScrapeDefinition = metricScrapeDefinition;
            _metricSinkWriter = metricSinkWriter;

            _metricScraperFactory = metricScraperFactory;
            _azureMonitorClient = azureMonitorClient;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Scraping Azure Monitor - {Timestamp}", DateTimeOffset.UtcNow);

            try
            {
                await ScrapeMetric(_metricScrapeDefinition);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(exception, "Failed to scrape: {Exception}", exception.Message);
            }
        }

        private async Task ScrapeMetric(ScrapeDefinition<IAzureResourceDefinition> metricDefinitionDefinition)
        {
            Logger.LogInformation("Scraping {MetricName} for resource type {ResourceType}", metricDefinitionDefinition.PrometheusMetricDefinition.Name, metricDefinitionDefinition.Resource.ResourceType);

            var scraper = _metricScraperFactory.CreateScraper(metricDefinitionDefinition.Resource.ResourceType, _metricSinkWriter, _azureMonitorClient);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}