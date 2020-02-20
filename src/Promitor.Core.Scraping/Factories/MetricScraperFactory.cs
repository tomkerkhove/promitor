using System;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Model.AzureMonitor;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Core.Scraping.Prometheus.Interfaces;
using Promitor.Core.Scraping.ResourceTypes;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping.Factories
{
    public class MetricScraperFactory
    {
        private readonly ILogger _logger;

        public MetricScraperFactory(ILogger<MetricScraperFactory> logger)
        {
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
        }

        /// <summary>
        ///     Creates a scraper that is capable of scraping a specific resource type
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="metricDefinitionResourceType">Resource type to scrape</param>
        /// <param name="prometheusMetricWriter">Metrics collector for our Prometheus scraping endpoint</param>
        /// <param name="azureMonitorClient">Client to interact with Azure Monitor</param>
        public IScraper<IAzureResourceDefinition> CreateScraper(ResourceType metricDefinitionResourceType, AzureMetadata azureMetadata,
            IPrometheusMetricWriter prometheusMetricWriter, AzureMonitorClient azureMonitorClient)
        {
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
    }
}