using System;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.FeatureFlags;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Core.Scraping.ResourceTypes;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Core.Telemetry.Metrics.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping.Factories
{
    public class MetricScraperFactory
    {
        private readonly IExceptionTracker _exceptionTracker;
        private readonly IConfiguration _configuration;
        private readonly FeatureToggleClient _featureToggleClient;
        private readonly ILogger _logger;

        public MetricScraperFactory(IConfiguration configuration, FeatureToggleClient featureToggleClient, ILogger logger, IExceptionTracker exceptionTracker)
        {
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(featureToggleClient, nameof(featureToggleClient));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(exceptionTracker, nameof(exceptionTracker));

            _logger = logger;
            _featureToggleClient = featureToggleClient;
            _configuration = configuration;
            _exceptionTracker = exceptionTracker;
        }

        /// <summary>
        ///     Creates a scraper that is capable of scraping a specific resource type
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="metricDefinitionResourceType">Resource type to scrape</param>
        /// <param name="runtimeMetricsCollector">Metrics collector for our runtime</param>
        public IScraper<MetricDefinition> CreateScraper(ResourceType metricDefinitionResourceType, AzureMetadata azureMetadata,
            IRuntimeMetricsCollector runtimeMetricsCollector)
        {
            var azureMonitorClient = CreateAzureMonitorClient(azureMetadata, runtimeMetricsCollector);
            var scraperConfiguration = new ScraperConfiguration(azureMetadata, azureMonitorClient, _featureToggleClient, _logger, _exceptionTracker);

            switch (metricDefinitionResourceType)
            {
                case ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueScraper(scraperConfiguration);
                case ResourceType.Generic:
                    return new GenericScraper(scraperConfiguration);
                case ResourceType.StorageQueue:
                    return new StorageQueueScraper(scraperConfiguration);
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceScraper(scraperConfiguration);
                case ResourceType.VirtualMachine:
                    return new VirtualMachineScraper(scraperConfiguration);
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceScraper(scraperConfiguration);
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryScraper(scraperConfiguration);
                case ResourceType.CosmosDb:
                    return new CosmosDbScraper(scraperConfiguration);
                case ResourceType.RedisCache:
                    return new RedisCacheScraper(scraperConfiguration);
                case ResourceType.PostgreSql:
                    return new PostgreSqlScraper(scraperConfiguration);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private AzureMonitorClient CreateAzureMonitorClient(AzureMetadata azureMetadata, IRuntimeMetricsCollector runtimeMetricsCollector)
        {
            var azureCredentials = DetermineAzureCredentials();
            var azureMonitorClient = new AzureMonitorClient(azureMetadata.TenantId, azureMetadata.SubscriptionId, azureCredentials.ApplicationId, azureCredentials.Secret, runtimeMetricsCollector, _logger);
            return azureMonitorClient;
        }

        private AzureCredentials DetermineAzureCredentials()
        {
            var applicationId = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
            var applicationKey = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);

            return new AzureCredentials
            {
                ApplicationId = applicationId,
                Secret = applicationKey
            };
        }
    }
}