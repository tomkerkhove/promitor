using System;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.AspNetCore;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Factories;
using Promitor.Core.Telemetry.Interfaces;

namespace Promitor.Scraper.Host.Scheduling
{
    public class MetricScrapingJob : IScheduledJob
    {
        private readonly MetricDefinition _metric;
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IExceptionTracker _exceptionTracker;
        private readonly ILogger _logger;

        public MetricScrapingJob(MetricDefinition metric,
            IMetricsDeclarationProvider metricsDeclarationProvider,
            ILogger logger, IExceptionTracker exceptionTracker)
        {
            Guard.NotNull(metric, nameof(metric));
            Guard.NotNull(exceptionTracker, nameof(exceptionTracker));
            Guard.NotNull(logger, nameof(logger));

            _metric = metric;
            _metricsDeclarationProvider = metricsDeclarationProvider;
            _exceptionTracker = exceptionTracker;
            _logger = logger;

            ConfigureJob();
        }

        public string CronSchedule { get; set; }
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string CronTimeZone { get; }
        public bool RunImmediately { get; set; }

        private void ConfigureJob()
        {
            CronSchedule = _metric.Scraping.Schedule;
            RunImmediately = false;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scraping Azure Monitor - {timestamp}", DateTimeOffset.Now);

            var scrapeConfiguration = _metricsDeclarationProvider.Get();
            try
            {
                await ScrapeMetric(scrapeConfiguration.AzureMetadata, _metric);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to scrape: {exception.Message}");
                _exceptionTracker.Track(exception);
            }
        }

        private async Task ScrapeMetric(AzureMetadata azureMetadata, MetricDefinition metricDefinitionDefinition)
        {
            _logger.LogInformation("Scraping '{MetricName}' for resource type '{ResourceType}'", metricDefinitionDefinition.Name, metricDefinitionDefinition.ResourceType);

            var scraper = MetricScraperFactory.CreateScraper(metricDefinitionDefinition.ResourceType, azureMetadata, _logger, _exceptionTracker);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}
