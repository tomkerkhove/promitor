using System;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Factories;
using Promitor.Core.Scraping.Prometheus.Interfaces;
using Promitor.Core.Telemetry.Metrics.Interfaces;

namespace Promitor.Scraper.Host.Scheduling
{
    public class MetricScrapingJob : IScheduledJob
    {
        private readonly ScrapeDefinition<IAzureResourceDefinition> _metric;
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IPrometheusMetricWriter _prometheusMetricWriter;
        private readonly IRuntimeMetricsCollector _runtimeMetricsCollector;
        private readonly ILogger _logger;

        private readonly MetricScraperFactory _metricScraperFactory;

        public MetricScrapingJob(ScrapeDefinition<IAzureResourceDefinition> metric,
            IMetricsDeclarationProvider metricsDeclarationProvider,
            IPrometheusMetricWriter prometheusMetricWriter,
            IRuntimeMetricsCollector runtimeMetricsCollector,
            MetricScraperFactory metricScraperFactory,
            ILogger<MetricScrapingJob> logger)
        {
            Guard.NotNull(metric, nameof(metric));
            Guard.NotNull(metricsDeclarationProvider, nameof(metricsDeclarationProvider));
            Guard.NotNull(prometheusMetricWriter, nameof(prometheusMetricWriter));
            Guard.NotNull(runtimeMetricsCollector, nameof(runtimeMetricsCollector));
            Guard.NotNull(metricScraperFactory, nameof(metricScraperFactory));
            Guard.NotNull(logger, nameof(logger));

            _metric = metric;
            _metricsDeclarationProvider = metricsDeclarationProvider;
            _prometheusMetricWriter = prometheusMetricWriter;
            _runtimeMetricsCollector = runtimeMetricsCollector;
            _logger = logger;

            _metricScraperFactory = metricScraperFactory;

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
            _logger.LogInformation("Scraping Azure Monitor - {Timestamp}", DateTimeOffset.UtcNow);

            var scrapeConfiguration = _metricsDeclarationProvider.Get();
            try
            {
                await ScrapeMetric(scrapeConfiguration.AzureMetadata, _metric);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Failed to scrape: {Exception}", exception.Message);
            }
        }

        private async Task ScrapeMetric(AzureMetadata azureMetadata, ScrapeDefinition<IAzureResourceDefinition> metricDefinitionDefinition)
        {
            _logger.LogInformation("Scraping {MetricName} for resource type {ResourceType}", metricDefinitionDefinition.PrometheusMetricDefinition.Name, metricDefinitionDefinition.Resource.ResourceType);

            var scraper = _metricScraperFactory.CreateScraper(metricDefinitionDefinition.Resource.ResourceType, azureMetadata, _prometheusMetricWriter, _runtimeMetricsCollector);
            await scraper.ScrapeAsync(metricDefinitionDefinition);
        }
    }
}
