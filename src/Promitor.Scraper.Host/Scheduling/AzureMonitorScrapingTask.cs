using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Scraper.Host.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Configuration.Providers.Interfaces;
using Promitor.Scraper.Host.Model.Configuration;
using Promitor.Scraper.Host.Scraping.Factories;
using Promitor.Scraper.Scheduling.Interfaces;

namespace Promitor.Scraper.Host.Scheduling
{
    public class AzureMonitorScrapingTask : IScheduledTask
    {
        private readonly IMetricsDeclarationProvider metricsDeclarationProvider;
        private readonly IExceptionTracker exceptionTracker;

        public AzureMonitorScrapingTask(IMetricsDeclarationProvider metricsDeclarationProvider, IExceptionTracker exceptionTracker)
        {
            this.metricsDeclarationProvider = metricsDeclarationProvider;
            this.exceptionTracker = exceptionTracker;
        }

        public string Schedule => Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Scraping Azure Monitor - {DateTimeOffset.Now}");

            var scrapeConfiguration = metricsDeclarationProvider.Get();

            var scrapingTasks = new List<Task>();

            try
            {
                var activity = new Activity("Scrape Monitor");
                var t = Activity.Current;
                
                Console.WriteLine();
                foreach (var metricDefinition in scrapeConfiguration.Metrics)
                {
                    var scrapingTask = ScrapeMetric(scrapeConfiguration.AzureMetadata, metricDefinition);
                    scrapingTasks.Add(scrapingTask);
                }

                await Task.WhenAll(scrapingTasks);
            }
            catch (Exception exception)
            {
                exceptionTracker.Track(exception);
            }
        }

        private async Task ScrapeMetric(AzureMetadata azureMetadata, MetricDefinition metricDefinitionDefinition)
        {
            Console.WriteLine($"\t> Scraping {metricDefinitionDefinition.Name} of type {metricDefinitionDefinition.ResourceType}");

            var scraper = MetricScraperFactory.CreateScraper(azureMetadata, metricDefinitionDefinition.ResourceType);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}