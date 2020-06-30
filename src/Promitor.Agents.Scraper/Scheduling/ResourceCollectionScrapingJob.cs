﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Discovery;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Factories;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureMonitor.Configuration;

namespace Promitor.Agents.Scraper.Scheduling
{
    public class ResourceCollectionScrapingJob : MetricScrapingJob, IScheduledJob
    {
        private readonly ResourceDiscoveryRepository _resourceDiscoveryRepository;
        private readonly MetricDefinition _metricDefinition;
        private readonly AzureMetadata _azureMetadata;
        private readonly IPrometheusMetricWriter _prometheusMetricWriter;
        private readonly MetricSinkWriter _metricSinkWriter;
        private readonly IRuntimeMetricsCollector _runtimeMetricCollector;
        private readonly AzureMonitorClientFactory _azureMonitorClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AzureMonitorLoggingConfiguration> _azureMonitorLoggingConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        private readonly MetricScraperFactory _metricScraperFactory;

        public ResourceCollectionScrapingJob(string jobName, string resourceCollectionName, AzureMetadata azureMetadata, MetricDefinition metricDefinition, ResourceDiscoveryRepository resourceDiscoveryRepository,
            MetricSinkWriter metricSinkWriter,
            IPrometheusMetricWriter prometheusMetricWriter,
            MetricScraperFactory metricScraperFactory,
            AzureMonitorClientFactory azureMonitorClientFactory, IRuntimeMetricsCollector runtimeMetricCollector, IConfiguration configuration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory,
            ILogger<ResourceCollectionScrapingJob> logger)
            : base(jobName, logger)
        {
            Guard.NotNullOrWhitespace(resourceCollectionName, nameof(resourceCollectionName));
            Guard.NotNull(resourceDiscoveryRepository, nameof(resourceDiscoveryRepository));
            Guard.NotNull(metricDefinition, nameof(metricDefinition));
            Guard.NotNull(azureMetadata, nameof(azureMetadata));
            Guard.NotNullOrWhitespace(jobName, nameof(jobName));
            Guard.NotNull(prometheusMetricWriter, nameof(prometheusMetricWriter));
            Guard.NotNull(metricScraperFactory, nameof(metricScraperFactory));
            Guard.NotNull(azureMonitorClientFactory, nameof(azureMonitorClientFactory));
            Guard.NotNull(runtimeMetricCollector, nameof(runtimeMetricCollector));
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(azureMonitorLoggingConfiguration, nameof(azureMonitorLoggingConfiguration));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));

            ResourceCollectionName = resourceCollectionName;

            _azureMetadata = azureMetadata;
            _metricDefinition = metricDefinition;
            _resourceDiscoveryRepository = resourceDiscoveryRepository;
            _prometheusMetricWriter = prometheusMetricWriter;
            _metricSinkWriter = metricSinkWriter;

            _runtimeMetricCollector = runtimeMetricCollector;
            _azureMonitorClientFactory = azureMonitorClientFactory;
            _configuration = configuration;
            _azureMonitorLoggingConfiguration = azureMonitorLoggingConfiguration;
            _loggerFactory = loggerFactory;

            _metricScraperFactory = metricScraperFactory;
        }

        public string ResourceCollectionName { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Scraping resource collection {ResourceCollection} - {Timestamp}", ResourceCollectionName, DateTimeOffset.UtcNow);

            try
            {
                var discoveredResources = await _resourceDiscoveryRepository.GetResourceCollectionAsync(ResourceCollectionName);
                Logger.LogInformation("Discovered {ResourceCount} resources for resource collection {ResourceCollection}.", discoveredResources?.Count ?? 0, ResourceCollectionName);

                if (discoveredResources == null)
                {
                    Logger.LogWarning("Discovered no resources for resource collection {ResourceCollection}.", ResourceCollectionName);
                    return;
                }

                List<Task> scrapeTasks = new List<Task>();
                foreach (var discoveredResource in discoveredResources)
                {
                    Logger.LogDebug($"Scraping discovered resource {discoveredResource}");

                    var azureMonitorClient = _azureMonitorClientFactory.CreateIfNotExists(_azureMetadata.Cloud, _azureMetadata.TenantId, discoveredResource.SubscriptionId, _metricSinkWriter, _runtimeMetricCollector, _configuration, _azureMonitorLoggingConfiguration, _loggerFactory);

                    // Scrape resource
                    var scrapeTask = ScrapeResourceAsync(discoveredResource, azureMonitorClient);
                    scrapeTasks.Add(scrapeTask);
                }

                await Task.WhenAll(scrapeTasks);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(exception, "Failed to scrape resource collection {ResourceCollection}: {Exception}", ResourceCollectionName, exception.Message);
            }
        }

        private async Task ScrapeResourceAsync(IAzureResourceDefinition discoveredResource, AzureMonitorClient azureMonitorClient)
        {
            try
            {
                var scrapingDefinition = _metricDefinition.CreateScrapeDefinition(discoveredResource, _azureMetadata);

                var scraper = _metricScraperFactory.CreateScraper(scrapingDefinition.Resource.ResourceType, _metricSinkWriter, _prometheusMetricWriter, azureMonitorClient);
                await scraper.ScrapeAsync(scrapingDefinition);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(exception, "Failed to scrape resource collection {Resource}: {Exception}", discoveredResource.UniqueName, exception.Message);
            }
        }
    }
}