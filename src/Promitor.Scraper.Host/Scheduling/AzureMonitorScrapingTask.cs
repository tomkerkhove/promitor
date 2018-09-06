using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Configuration.Providers.Interfaces;
using Promitor.Scraper.Host.Scheduling.Interfaces;
using Promitor.Scraper.Host.Scraping.Factories;

namespace Promitor.Scraper.Host.Scheduling
{
    public class AzureMonitorScrapingTask : IScheduledTask
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IExceptionTracker _exceptionTracker;

        public AzureMonitorScrapingTask(IMetricsDeclarationProvider metricsDeclarationProvider, IExceptionTracker exceptionTracker)
        {
            _metricsDeclarationProvider = metricsDeclarationProvider;
            _exceptionTracker = exceptionTracker;
        }

        public string Schedule => Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Scraping Azure Monitor - {DateTimeOffset.Now}");

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
            Console.WriteLine($"\t> Scraping {metricDefinitionDefinition.Name} of type {metricDefinitionDefinition.ResourceType}");

            var scraper = MetricScraperFactory.CreateScraper(azureMetadata, metricDefinitionDefinition.ResourceType, _exceptionTracker);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}