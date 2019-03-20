using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.AspNetCore;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Factories;
using Promitor.Core.Telemetry.Interfaces;

namespace Promitor.Scraper.Host.Scheduling
{
    public class MetricScrapingJob : IScheduledJob
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IExceptionTracker _exceptionTracker;
        private readonly ILogger _logger;

        public MetricScrapingJob(IMetricsDeclarationProvider metricsDeclarationProvider, ILogger logger, IExceptionTracker exceptionTracker)
        {
            Guard.NotNull(metricsDeclarationProvider, nameof(metricsDeclarationProvider));
            Guard.NotNull(exceptionTracker, nameof(exceptionTracker));
            Guard.NotNull(logger, nameof(logger));

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
            CronSchedule = Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);
            RunImmediately = false;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scraping Azure Monitor - {timestamp}", DateTimeOffset.Now);

            try
            {
                var scrapeConfiguration = _metricsDeclarationProvider.Get();

                var scrapingTasks = new List<Task>();

                foreach (var metricDefinition in scrapeConfiguration.Metrics)
                {
                    var scrapingTask = ScrapeMetric(scrapeConfiguration.AzureMetadata, scrapeConfiguration.MetricDefaults, metricDefinition);
                    scrapingTasks.Add(scrapingTask);
                }

                await Task.WhenAll(scrapingTasks);
            }
            catch (Exception exception)
            {
                _exceptionTracker.Track(exception);
            }
        }

        private async Task ScrapeMetric(AzureMetadata azureMetadata, MetricDefaults metricDefaults, MetricDefinition metricDefinitionDefinition)
        {
            _logger.LogInformation("Scraping '{MetricName}' for resource type '{ResourceType}'", metricDefinitionDefinition.Name, metricDefinitionDefinition.ResourceType);

            var scraper = MetricScraperFactory.CreateScraper(metricDefinitionDefinition.ResourceType, azureMetadata, metricDefaults, _logger, _exceptionTracker);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}
