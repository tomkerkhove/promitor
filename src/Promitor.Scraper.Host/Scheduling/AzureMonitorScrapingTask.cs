using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Factories;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Scraper.Host.Scheduling.Interfaces;

namespace Promitor.Scraper.Host.Scheduling
{
    public class AzureMonitorScrapingTask : IScheduledTask
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IExceptionTracker _exceptionTracker;
        private readonly ILogger _logger;

        public AzureMonitorScrapingTask(IMetricsDeclarationProvider metricsDeclarationProvider, ILogger logger, IExceptionTracker exceptionTracker)
        {
            _metricsDeclarationProvider = metricsDeclarationProvider;
            _logger = logger;
            _exceptionTracker = exceptionTracker;
        }

        public string Schedule => Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scraping Azure Monitor - {timestamp}", DateTimeOffset.Now);

            var scrapeConfiguration = _metricsDeclarationProvider.Get();

            var scrapingTasks = new List<Task>();

            foreach (var metricDefinition in scrapeConfiguration.Metrics)
            {
                var scrapingTask = ScrapeMetric(scrapeConfiguration.AzureMetadata, metricDefinition);
                scrapingTasks.Add(scrapingTask);
            }

            await Task.WhenAll(scrapingTasks);
        }

        private async Task ScrapeMetric(AzureMetadata azureMetadata, MetricDefinition metricDefinitionDefinition)
        {
            _logger.LogInformation("Scraping '{metricName}' for resource type '{resourceType}'", metricDefinitionDefinition.Name, metricDefinitionDefinition.ResourceType);

            var scraper = MetricScraperFactory.CreateScraper(azureMetadata, metricDefinitionDefinition.ResourceType, _logger,_exceptionTracker);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}