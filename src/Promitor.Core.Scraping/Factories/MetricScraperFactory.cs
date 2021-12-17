using System;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Interfaces;
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
        /// <param name="metricDefinitionResourceType">Resource type to scrape</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all sinks</param>
        /// <param name="azureScrapingPrometheusMetricsCollector">Collector to send metrics related to the runtime</param>
        /// <param name="azureMonitorClient">Client to interact with Azure Monitor</param>
        public IScraper<IAzureResourceDefinition> CreateScraper(ResourceType metricDefinitionResourceType, MetricSinkWriter metricSinkWriter, IAzureScrapingPrometheusMetricsCollector azureScrapingPrometheusMetricsCollector, AzureMonitorClient azureMonitorClient)
        {
            var scraperConfiguration = new ScraperConfiguration(azureMonitorClient, metricSinkWriter, azureScrapingPrometheusMetricsCollector, _logger);

            switch (metricDefinitionResourceType)
            {
                case ResourceType.ApiManagement:
                    return new ApiManagementScraper(scraperConfiguration);
                case ResourceType.ApplicationGateway:
                    return new ApplicationGatewayScraper(scraperConfiguration);
                case ResourceType.ApplicationInsights:
                    return new ApplicationInsightsScraper(scraperConfiguration);
                case ResourceType.AppPlan:
                    return new AppPlanScraper(scraperConfiguration);
                case ResourceType.AutomationAccount:
                    return new AutomationAccountScraper(scraperConfiguration);
                case ResourceType.BlobStorage:
                    return new BlobStorageScraper(scraperConfiguration);
                case ResourceType.Cdn:
                    return new CdnScraper(scraperConfiguration);
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceScraper(scraperConfiguration);
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryScraper(scraperConfiguration);
                case ResourceType.CosmosDb:
                    return new CosmosDbScraper(scraperConfiguration);
                case ResourceType.DataFactory:
                    return new DataFactoryScraper(scraperConfiguration);
                case ResourceType.DataShare:
                    return new DataShareScraper(scraperConfiguration);
                case ResourceType.DeviceProvisioningService:
                    return new DeviceProvisioningServiceScraper(scraperConfiguration);
                case ResourceType.EventHubs:
                    return new EventHubsScraper(scraperConfiguration);
                case ResourceType.ExpressRouteCircuit:
                    return new ExpressRouteCircuitScraper(scraperConfiguration);
                case ResourceType.FileStorage:
                    return new FileStorageScraper(scraperConfiguration);
                case ResourceType.FrontDoor:
                    return new FrontDoorScraper(scraperConfiguration);
                case ResourceType.FunctionApp:
                    return new FunctionAppScraper(scraperConfiguration);
                case ResourceType.Generic:
                    return new GenericScraper(scraperConfiguration);
                case ResourceType.IoTHub:
                    return new IoTHubScraper(scraperConfiguration);
                case ResourceType.KeyVault:
                    return new KeyVaultScraper(scraperConfiguration);
                case ResourceType.KubernetesService:
                    return new KubernetesServiceScraper(scraperConfiguration);
                case ResourceType.LoadBalancer:
                    return new LoadBalancerScraper(scraperConfiguration);
                case ResourceType.LogicApp:
                    return new LogicAppScraper(scraperConfiguration);
                case ResourceType.MariaDb:
                    return new MariaDbScraper(scraperConfiguration);
                case ResourceType.MonitorAutoscale:
                    return new MonitorAutoscaleScraper(scraperConfiguration);
                case ResourceType.NetworkGateway:
                    return new NetworkGatewayScraper(scraperConfiguration);
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceScraper(scraperConfiguration);
                case ResourceType.PostgreSql:
                    return new PostgreSqlScraper(scraperConfiguration);
                case ResourceType.RedisCache:
                    return new RedisCacheScraper(scraperConfiguration);
                case ResourceType.RedisEnterpriseCache:
                    return new RedisEnterpriseCacheScraper(scraperConfiguration);
                case ResourceType.ServiceBusNamespace:
                    return new ServiceBusNamespaceScraper(scraperConfiguration);
                case ResourceType.SqlDatabase:
                    return new SqlDatabaseScraper(scraperConfiguration);
                case ResourceType.SqlElasticPool:
                    return new SqlElasticPoolScraper(scraperConfiguration);
                case ResourceType.SqlManagedInstance:
                    return new SqlManagedInstanceScraper(scraperConfiguration);
                case ResourceType.SqlServer:
                    return new SqlServerScraper(scraperConfiguration);
                case ResourceType.StorageAccount:
                    return new StorageAccountScraper(scraperConfiguration);
                case ResourceType.StorageQueue:
                    return new StorageQueueScraper(scraperConfiguration);
                case ResourceType.SynapseApacheSparkPool:
                    return new SynapseApacheSparkPoolScraper(scraperConfiguration);
                case ResourceType.SynapseSqlPool:
                    return new SynapseSqlPoolScraper(scraperConfiguration);
                case ResourceType.SynapseWorkspace:
                    return new SynapseWorkspaceScraper(scraperConfiguration);
                case ResourceType.VirtualMachine:
                    return new VirtualMachineScraper(scraperConfiguration);
                case ResourceType.VirtualMachineScaleSet:
                    return new VirtualMachineScaleSetScraper(scraperConfiguration);
                case ResourceType.VirtualNetwork:
                    return new VirtualNetworkScraper(scraperConfiguration);
                case ResourceType.WebApp:
                    return new WebAppScraper(scraperConfiguration);
                default:
                    throw new ArgumentOutOfRangeException(nameof(metricDefinitionResourceType), metricDefinitionResourceType, "Matching scraper not found");
            }
        }
    }
}
