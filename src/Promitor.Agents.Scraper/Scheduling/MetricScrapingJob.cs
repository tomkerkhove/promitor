using System;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Factories;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Agents.Scraper.Scheduling
{
    public class MetricScrapingJob : IScheduledJob
    {
        private readonly ScrapeDefinition<IAzureResourceDefinition> _metricScrapeDefinition;
        private readonly IPrometheusMetricWriter _prometheusMetricWriter;
        private readonly AzureMonitorClient _azureMonitorClient;
        private readonly MetricSinkWriter _metricSinkWriter;
        private readonly ILogger _logger;

        private readonly MetricScraperFactory _metricScraperFactory;

        public MetricScrapingJob(string jobName,
            ScrapeDefinition<IAzureResourceDefinition> metricScrapeDefinition,
            MetricSinkWriter metricSinkWriter,
            IPrometheusMetricWriter prometheusMetricWriter,
            MetricScraperFactory metricScraperFactory,
            AzureMonitorClient azureMonitorClient,
            ILogger<MetricScrapingJob> logger)
        {
            Guard.NotNull(metricScrapeDefinition, nameof(metricScrapeDefinition));
            Guard.NotNull(prometheusMetricWriter, nameof(prometheusMetricWriter));
            Guard.NotNull(metricScraperFactory, nameof(metricScraperFactory));
            Guard.NotNull(azureMonitorClient, nameof(azureMonitorClient));
            Guard.NotNullOrWhitespace(jobName, nameof(jobName));
            Guard.NotNull(logger, nameof(logger));

            Name = jobName;

            _metricScrapeDefinition = metricScrapeDefinition;
            _prometheusMetricWriter = prometheusMetricWriter;
            _metricSinkWriter = metricSinkWriter;
            _logger = logger;

            _metricScraperFactory = metricScraperFactory;
            _azureMonitorClient = azureMonitorClient;
        }

        public string Name { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scraping Azure Monitor - {Timestamp}", DateTimeOffset.UtcNow);

            try
            {
                await ScrapeMetric(_metricScrapeDefinition);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Failed to scrape: {Exception}", exception.Message);
            }
        }

        private async Task ScrapeMetric(ScrapeDefinition<IAzureResourceDefinition> metricDefinitionDefinition)
        {
            _logger.LogInformation("Scraping {MetricName} for resource type {ResourceType}", metricDefinitionDefinition.PrometheusMetricDefinition.Name, metricDefinitionDefinition.Resource.ResourceType);

            var scraper = _metricScraperFactory.CreateScraper(metricDefinitionDefinition.Resource.ResourceType, _metricSinkWriter, _prometheusMetricWriter, _azureMonitorClient);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}