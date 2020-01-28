using System;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Core.Scraping.Prometheus.Interfaces;
using Promitor.Core.Scraping.ResourceTypes;
using Promitor.Core.Telemetry.Metrics.Interfaces;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureMonitor.Logging;

namespace Promitor.Core.Scraping.Factories
{
    public class MetricScraperFactory
    {
        private readonly AzureMonitorLoggingOptions _azureMonitorLoggingOptions;
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public MetricScraperFactory(AzureMonitorLoggingOptions azureMonitorLoggingOptions, IConfiguration configuration, ILogger<MetricScraperFactory> logger, ILoggerFactory loggerFactory)
        {
            Guard.NotNull(azureMonitorLoggingOptions, nameof(azureMonitorLoggingOptions));
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
            _loggerFactory = loggerFactory;
            _configuration = configuration;
            _azureMonitorLoggingOptions = azureMonitorLoggingOptions;
        }

        /// <summary>
        ///     Creates a scraper that is capable of scraping a specific resource type
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="metricDefinitionResourceType">Resource type to scrape</param>
        /// <param name="prometheusMetricWriter">Metrics collector for our Prometheus scraping endpoint</param>
        /// <param name="runtimeMetricsCollector">Metrics collector for our runtime</param>
        public IScraper<IAzureResourceDefinition> CreateScraper(ResourceType metricDefinitionResourceType, AzureMetadata azureMetadata,
            IPrometheusMetricWriter prometheusMetricWriter, IRuntimeMetricsCollector runtimeMetricsCollector)
        {
            var azureMonitorClient = CreateAzureMonitorClient(azureMetadata, runtimeMetricsCollector);
            var scraperConfiguration = new ScraperConfiguration(azureMetadata, azureMonitorClient, prometheusMetricWriter, _logger);

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
                case ResourceType.SqlDatabase:
                    return new SqlDatabaseScraper(scraperConfiguration);
                case ResourceType.SqlManagedInstance:
                    return new SqlManagedInstanceScraper(scraperConfiguration);
                case ResourceType.VirtualMachineScaleSet:
                    return new VirtualMachineScaleSetScraper(scraperConfiguration);
                case ResourceType.AppPlan:
                    return new AppPlanScraper(scraperConfiguration);
                case ResourceType.WebApp:
                    return new WebAppScraper(scraperConfiguration);
                case ResourceType.FunctionApp:
                    return new FunctionAppScraper(scraperConfiguration);
                case ResourceType.SqlServer:
                    return new SqlServerScraper(scraperConfiguration);
                case ResourceType.ApiManagement:
                    return new ApiManagementScraper(scraperConfiguration);
                case ResourceType.StorageAccount:
                    return new StorageAccountScraper(scraperConfiguration);
                case ResourceType.BlobStorage:
                    return new BlobStorageScraper(scraperConfiguration);
                case ResourceType.FileStorage:
                    return new FileStorageScraper(scraperConfiguration);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private AzureMonitorClient CreateAzureMonitorClient(AzureMetadata azureMetadata, IRuntimeMetricsCollector runtimeMetricsCollector)
        {
            var azureCredentials = DetermineAzureCredentials();

            var azureMonitorClient = new AzureMonitorClient(azureMetadata.Cloud, azureMetadata.TenantId, azureMetadata.SubscriptionId, azureCredentials.ApplicationId, azureCredentials.Secret, _azureMonitorLoggingOptions, runtimeMetricsCollector, _loggerFactory, _logger);
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