using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Promitor.Scraper.Configuration;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using Promitor.Scraper.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics;
using Promitor.Scraper.Scheduling.Interfaces;
using Promitor.Scraper.Scraping.Factories;

namespace Promitor.Scraper.Scheduling
{
    public class AzureMonitorScrapingTask : IScheduledTask
    {
        private readonly IMetricsDeclarationProvider metricsDeclarationProvider;

        public AzureMonitorScrapingTask(IMetricsDeclarationProvider metricsDeclarationProvider)
        {
            this.metricsDeclarationProvider = metricsDeclarationProvider;
        }

        public string Schedule => Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Scraping Azure Monitor - {DateTimeOffset.Now}");

            var scrapeConfiguration = metricsDeclarationProvider.Get();

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

            var scraper = MetricScraperFactory.CreateScraper(azureMetadata, metricDefinitionDefinition.ResourceType);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}