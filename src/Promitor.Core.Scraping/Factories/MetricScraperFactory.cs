using System;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IExceptionTracker _exceptionTracker;

        public MetricScraperFactory(IConfiguration configuration, ILogger logger, IExceptionTracker exceptionTracker)
        {
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(exceptionTracker, nameof(exceptionTracker));

            _configuration = configuration;
            _logger = logger;
            _exceptionTracker = exceptionTracker;
        }

        /// <summary>
        ///     Creates a scraper that is capable of scraping a specific resource type
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="metricDefinitionResourceType">Resource type to scrape</param>
        /// <param name="runtimeMetricsCollector">Metrics collector for our runtime</param>
        /// <param name="logger">General logger</param>
        /// <param name="exceptionTracker">Tracker used to log exceptions</param>
        public IScraper<MetricDefinition> CreateScraper(ResourceType metricDefinitionResourceType, AzureMetadata azureMetadata,
            IRuntimeMetricsCollector runtimeMetricsCollector)
        {
            var azureMonitorClient = CreateAzureMonitorClient(azureMetadata, runtimeMetricsCollector);

            switch (metricDefinitionResourceType)
            {
                case ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.Generic:
                    return new GenericScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.StorageQueue:
                    return new StorageQueueScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.VirtualMachine:
                    return new VirtualMachineScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.CosmosDb:
                    return new CosmosDbScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.RedisCache:
                    return new RedisCacheScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
                case ResourceType.PostgreSql:
                    return new PostgreSqlScraper(azureMetadata, azureMonitorClient, _logger, _exceptionTracker);
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