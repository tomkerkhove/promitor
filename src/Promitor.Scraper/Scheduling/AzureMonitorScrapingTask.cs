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
        private readonly IScrapeConfigurationProvider scrapeConfigurationProvider;

        public AzureMonitorScrapingTask(IScrapeConfigurationProvider scrapeConfigurationProvider)
        {
            this.scrapeConfigurationProvider = scrapeConfigurationProvider;
        }

        public string Schedule => Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Scraping Azure Monitor - {DateTimeOffset.Now}");

            var scrapeConfiguration = scrapeConfigurationProvider.GetConfiguration();

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
            Console.WriteLine($"Scraping {metricDefinitionDefinition.Name} of type {metricDefinitionDefinition.ResourceType}");

            var scraper = MetricScraperFactory.CreateScraper(azureMetadata, metricDefinitionDefinition.ResourceType);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}