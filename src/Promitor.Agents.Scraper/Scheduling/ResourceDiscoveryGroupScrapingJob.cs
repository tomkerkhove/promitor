using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Discovery;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Factories;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureMonitor.Configuration;

namespace Promitor.Agents.Scraper.Scheduling
{
    public class ResourceDiscoveryGroupScrapingJob : MetricScrapingJob, IScheduledJob
    {
        private readonly ResourceDiscoveryRepository _resourceDiscoveryRepository;
        private readonly MetricDefinition _metricDefinition;
        private readonly AzureMetadata _azureMetadata;
        private readonly MetricSinkWriter _metricSinkWriter;
        private readonly IAzureScrapingPrometheusMetricsCollector _azureScrapingPrometheusMetricsCollector;
        private readonly AzureMonitorClientFactory _azureMonitorClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AzureMonitorLoggingConfiguration> _azureMonitorLoggingConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        private readonly MetricScraperFactory _metricScraperFactory;

        public ResourceDiscoveryGroupScrapingJob(string jobName, string resourceDiscoveryGroupName, AzureMetadata azureMetadata, MetricDefinition metricDefinition, ResourceDiscoveryRepository resourceDiscoveryRepository,
            MetricSinkWriter metricSinkWriter,
            MetricScraperFactory metricScraperFactory,
            AzureMonitorClientFactory azureMonitorClientFactory,
            IAzureScrapingPrometheusMetricsCollector azureScrapingPrometheusMetricsCollector,
            IMemoryCache memoryCache,
            IConfiguration configuration,
            IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration,
            ILoggerFactory loggerFactory,
            ILogger<ResourceDiscoveryGroupScrapingJob> logger)
            : base(jobName, logger)
        {
            Guard.NotNullOrWhitespace(resourceDiscoveryGroupName, nameof(resourceDiscoveryGroupName));
            Guard.NotNull(resourceDiscoveryRepository, nameof(resourceDiscoveryRepository));
            Guard.NotNull(metricDefinition, nameof(metricDefinition));
            Guard.NotNull(azureMetadata, nameof(azureMetadata));
            Guard.NotNullOrWhitespace(jobName, nameof(jobName));
            Guard.NotNull(metricScraperFactory, nameof(metricScraperFactory));
            Guard.NotNull(azureMonitorClientFactory, nameof(azureMonitorClientFactory));
            Guard.NotNull(azureScrapingPrometheusMetricsCollector, nameof(azureScrapingPrometheusMetricsCollector));
            Guard.NotNull(memoryCache, nameof(memoryCache));
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(azureMonitorLoggingConfiguration, nameof(azureMonitorLoggingConfiguration));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));

            ResourceDiscoveryGroupName = resourceDiscoveryGroupName;

            _azureMetadata = azureMetadata;
            _metricDefinition = metricDefinition;
            _resourceDiscoveryRepository = resourceDiscoveryRepository;
            _metricSinkWriter = metricSinkWriter;

            _azureScrapingPrometheusMetricsCollector = azureScrapingPrometheusMetricsCollector;
            _azureMonitorClientFactory = azureMonitorClientFactory;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _azureMonitorLoggingConfiguration = azureMonitorLoggingConfiguration;
            _loggerFactory = loggerFactory;

            _metricScraperFactory = metricScraperFactory;
        }

        public string ResourceDiscoveryGroupName { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Scraping resource collection {ResourceDiscoveryGroup} - {Timestamp}", ResourceDiscoveryGroupName, DateTimeOffset.UtcNow);

            try
            {
                var discoveredResources = await _resourceDiscoveryRepository.GetResourceDiscoveryGroupAsync(ResourceDiscoveryGroupName);
                Logger.LogInformation("Discovered {ResourceCount} resources for resource collection {ResourceDiscoveryGroup}.", discoveredResources?.Count ?? 0, ResourceDiscoveryGroupName);

                if (discoveredResources == null)
                {
                    Logger.LogWarning("Discovered no resources for resource collection {ResourceDiscoveryGroup}.", ResourceDiscoveryGroupName);
                    return;
                }

                List<Task> scrapeTasks = new List<Task>();
                foreach (var discoveredResource in discoveredResources)
                {
                    Logger.LogDebug($"Scraping discovered resource {discoveredResource}");

                    var azureMonitorClient = _azureMonitorClientFactory.CreateIfNotExists(_azureMetadata.Cloud, _azureMetadata.TenantId, discoveredResource.SubscriptionId, _metricSinkWriter, _azureScrapingPrometheusMetricsCollector, _memoryCache, _configuration, _azureMonitorLoggingConfiguration, _loggerFactory);

                    // Scrape resource
                    var scrapeTask = ScrapeResourceAsync(discoveredResource, azureMonitorClient);
                    scrapeTasks.Add(scrapeTask);
                }

                await Task.WhenAll(scrapeTasks);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(exception, "Failed to scrape resource collection {ResourceDiscoveryGroup}: {Exception}", ResourceDiscoveryGroupName, exception.Message);
            }
        }

        private async Task ScrapeResourceAsync(IAzureResourceDefinition discoveredResource, AzureMonitorClient azureMonitorClient)
        {
            try
            {
                var scrapingDefinition = _metricDefinition.CreateScrapeDefinition(discoveredResource, _azureMetadata);

                var scraper = _metricScraperFactory.CreateScraper(scrapingDefinition.Resource.ResourceType, _metricSinkWriter, _azureScrapingPrometheusMetricsCollector, azureMonitorClient);
                await scraper.ScrapeAsync(scrapingDefinition);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(exception, "Failed to scrape resource collection {Resource}: {Exception}", discoveredResource.UniqueName, exception.Message);
            }
        }
    }
}