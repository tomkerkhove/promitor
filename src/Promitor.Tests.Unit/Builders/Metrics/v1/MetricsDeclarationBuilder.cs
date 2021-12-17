using System.Collections.Generic;
using AutoMapper;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes.Enums;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Serialization.Enum;
using Promitor.Integrations.AzureStorage;
using Promitor.Tests.Unit.Serialization.v1;

namespace Promitor.Tests.Unit.Builders.Metrics.v1
{
    public class MetricsDeclarationBuilder
    {
        private readonly AzureMetadataV1 _azureMetadata;
        private readonly List<MetricDefinitionV1> _metrics = new List<MetricDefinitionV1>();
        private MetricDefaultsV1 _metricDefaults = new MetricDefaultsV1
        {
            Scraping = new ScrapingV1 { Schedule = @"0 * * ? * *" }
        };

        private V1Deserializer _v1Deserializer;

        public MetricsDeclarationBuilder(AzureMetadataV1 azureMetadata)
        {
            _azureMetadata = azureMetadata;
        }

        public static MetricsDeclarationBuilder WithMetadata(string tenantId = "tenantId", string subscriptionId = "subscriptionId", string resourceGroupName = "resourceGroupName")
        {
            var azureMetadata = new AzureMetadataV1
            {
                TenantId = tenantId,
                SubscriptionId = subscriptionId,
                ResourceGroupName = resourceGroupName
            };

            return new MetricsDeclarationBuilder(azureMetadata);
        }

        public static MetricsDeclarationBuilder WithoutMetadata()
        {
            return new MetricsDeclarationBuilder(azureMetadata: null);
        }

        public MetricsDeclarationBuilder WithDefaults(MetricDefaultsV1 defaults)
        {
            _metricDefaults = defaults;

            return this;
        }

        public string Build(IMapper mapper)
        {
            var metricsDeclaration = new MetricsDeclarationV1
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadata = _azureMetadata,
                MetricDefaults = _metricDefaults,
                Metrics = _metrics
            };

            _v1Deserializer = V1DeserializerFactory.CreateDeserializer();

            var configurationSerializer = new ConfigurationSerializer(NullLogger<ConfigurationSerializer>.Instance, mapper, _v1Deserializer);
            return configurationSerializer.Serialize(metricsDeclaration);
        }

        public MetricsDeclarationBuilder WithApiManagementMetric(string metricName = "promitor-api-management",
            string metricDescription = "Description for a metric",
            string instanceName = "promitor-app-plan",
            string locationName = "West Europe",
            string azureMetricName = "TotalRequests",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new ApiManagementResourceV1
            {
                InstanceName = instanceName,
                LocationName = locationName
            };

            CreateAndAddMetricDefinition(ResourceType.ApiManagement, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithApplicationGatewayMetric(string metricName = "promitor-application-gateway",
            string metricDescription = "Description for a metric",
            string applicationGatewayName = "promitor-application-gateway-name",
            string azureMetricName = "ApplicationGatewayTotalTime",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new ApplicationGatewayResourceV1
            {
                ApplicationGatewayName = applicationGatewayName
            };

            CreateAndAddMetricDefinition(ResourceType.ApplicationGateway, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithApplicationInsightsMetric(string metricName = "promitor-application-gateway",
            string metricDescription = "Description for a metric",
            string applicationInsightsName = "promitor-application-insights-name",
            string azureMetricName = "ApplicationGatewayTotalTime",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new ApplicationInsightsResourceV1
            {
                Name = applicationInsightsName
            };

            CreateAndAddMetricDefinition(ResourceType.ApplicationInsights, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithAppPlanMetric(string metricName = "promitor-app-plan",
            string metricDescription = "Description for a metric",
            string appPlanName = "promitor-app-plan",
            string azureMetricName = "TotalRequests",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new AppPlanResourceV1
            {
                AppPlanName = appPlanName
            };

            CreateAndAddMetricDefinition(ResourceType.AppPlan, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithAutomationAccountMetric(string metricName = "promitor-automation-metrics",
            string metricDescription = "Description for a metric",
            string accountName = "promitor-automation-account",
            string runbookName = "",
            string azureMetricName = "TotalJob",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new AutomationAccountResourceV1
            {
                AccountName = accountName,
                RunbookName = runbookName
            };

            CreateAndAddMetricDefinition(ResourceType.AutomationAccount, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithBlobStorageMetric(string metricName = "promitor",
            string metricDescription = "Description for a metric",
            string accountName = "promitor-account",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new BlobStorageResourceV1
            {
                AccountName = accountName
            };

            CreateAndAddMetricDefinition(ResourceType.BlobStorage, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithCdnMetric(string metricName = "promitor-cdn",
            string metricDescription = "Description for a metric",
            string cdnName = "promitor-cdn-name",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new CdnResourceV1
            {
                CdnName = cdnName
            };

            CreateAndAddMetricDefinition(ResourceType.Cdn, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithContainerInstanceMetric(string metricName = "promitor-container-instance",
            string metricDescription = "Description for a metric",
            string containerGroup = "promitor-group",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new ContainerInstanceResourceV1
            {
                ContainerGroup = containerGroup
            };

            CreateAndAddMetricDefinition(ResourceType.ContainerInstance, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithContainerRegistryMetric(string metricName = "promitor-container-registry",
            string metricDescription = "Description for a metric",
            string registryName = "promitor-container-registry",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new ContainerRegistryResourceV1
            {
                RegistryName = registryName
            };

            CreateAndAddMetricDefinition(ResourceType.ContainerRegistry, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithCosmosDbMetric(string metricName = "promitor-cosmosdb",
            string metricDescription = "Description for a metric",
            string dbName = "promitor-cosmosdb",
            string azureMetricName = "TotalRequests",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new CosmosDbResourceV1
            {
                DbName = dbName
            };

            CreateAndAddMetricDefinition(ResourceType.CosmosDb, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithDataShareMetric(string metricName = "promitor-data-share",
            string metricDescription = "Description for a metric",
            string accountName = "promitor-data-share-account",
            string shareName = "promitor-data-share",
            string azureMetricName = "TotalRequests",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new DataShareResourceV1
            {
                AccountName = accountName,
                ShareName = shareName
            };

            CreateAndAddMetricDefinition(ResourceType.DataShare, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithDataFactoryMetric(string metricName = "promitor-data-factory",
            string metricDescription = "Description for a metric",
            string factoryName = "promitor-data-factory",
            string pipelineName = "promitor-data-pipeline",
            string azureMetricName = "TotalRequests",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new DataFactoryResourceV1
            {
                FactoryName = factoryName,
                PipelineName = pipelineName
            };

            CreateAndAddMetricDefinition(ResourceType.DataFactory, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithDeviceProvisioningServiceMetric(string metricName = "promitor-dps",
            string metricDescription = "Description for a metric",
            string deviceProvisioningServiceName = "promitor-dps",
            string azureMetricName = "AttestationAttempts",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new DeviceProvisioningServiceResourceV1
            {
                DeviceProvisioningServiceName = deviceProvisioningServiceName
            };

            CreateAndAddMetricDefinition(ResourceType.DeviceProvisioningService, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithEventHubsMetric(string metricName = "promitor-event-hubs",
            string metricDescription = "Description for a metric",
            string metricDimension = "",
            string topicName = "promitor-queue",
            string eventHubsNamespace = "promitor-namespace",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new EventHubsResourceV1
            {
                TopicName = topicName,
                Namespace = eventHubsNamespace
            };

            CreateAndAddMetricDefinition(ResourceType.EventHubs, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource, metricDimension);

            return this;
        }

        public MetricsDeclarationBuilder WithExpressRouteCircuitMetric(string metricName = "promitor-express-route-circuit",
            string metricDescription = "Description for a metric",
            string expressRouteCircuitName = "promitor-express-route-circuit-name",
            string azureMetricName = "ArpAvailability",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new ExpressRouteCircuitResourceV1
            {
                ExpressRouteCircuitName = expressRouteCircuitName
            };

            CreateAndAddMetricDefinition(ResourceType.ExpressRouteCircuit, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithFunctionAppMetric(string metricName = "promitor-fuction-app",
            string metricDescription = "Description for a metric",
            string functionAppName = "promitor-fuction-app",
            string azureMetricName = "TotalRequests",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new FunctionAppResourceV1
            {
                FunctionAppName = functionAppName
            };

            CreateAndAddMetricDefinition(ResourceType.FunctionApp, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithFileStorageMetric(string metricName = "promitor",
            string metricDescription = "Description for a metric",
            string accountName = "promitor-account",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new FileStorageResourceV1
            {
                AccountName = accountName
            };

            CreateAndAddMetricDefinition(ResourceType.FileStorage, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithFrontDoorMetric(string metricName = "promitor",
            string metricDescription = "Description for a metric",
            string name = "BackendHealthPercentage",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new FrontDoorResourceV1
            {
                Name = name
            };

            CreateAndAddMetricDefinition(ResourceType.FrontDoor, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithGenericMetric(string metricName = "foo",
            string metricDescription = "Description for a metric",
            string resourceUri = "Microsoft.ServiceBus/namespaces/promitor-messaging",
            string filter = "EntityName eq \'orders\'",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new GenericResourceV1
            {
                ResourceUri = resourceUri,
                Filter = filter
            };

            CreateAndAddMetricDefinition(ResourceType.Generic, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithIoTHubMetric(string metricName = "promitor-iot-hub",
            string metricDescription = "Description for a metric",
            string iotHubName = "promitor-iot-hub",
            string azureMetricName = "devices.totalDevices",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new IoTHubResourceV1
            {
                IoTHubName = iotHubName
            };

            CreateAndAddMetricDefinition(ResourceType.IoTHub, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithKeyVaultMetric(string metricName = "promitor-kv",
            string metricDescription = "Description for a metric",
            string vaultName = "promitor-kv",
            string azureMetricName = "ServiceApiLatency",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new KeyVaultResourceV1
            {
                VaultName = vaultName
            };

            CreateAndAddMetricDefinition(ResourceType.KeyVault, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithKubernetesServiceMetric(string metricName = "promitor-aks",
            string metricDescription = "Description for a metric",
            string clusterName = "promitor-aks",
            string azureMetricName = "kube_node_status_condition",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new KubernetesServiceResourceV1
            {
                ClusterName = clusterName
            };

            CreateAndAddMetricDefinition(ResourceType.KubernetesService, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithLoadBalancerMetric(string metricName = "promitor-load-balancer",
            string metricDescription = "Description for a metric",
            string loadBalancerName = "promitor-load-balancer-name",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new LoadBalancerResourceV1()
            {
                LoadBalancerName = loadBalancerName
            };

            CreateAndAddMetricDefinition(ResourceType.LoadBalancer, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithLogicAppMetric(string metricName = "promitor-logic-apps-failed-runs",
            string metricDescription = "Description for a metric",
            string workflowName = "promitor-workflow",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new LogicAppResourceV1
            {
                WorkflowName = workflowName
            };

            CreateAndAddMetricDefinition(ResourceType.LogicApp, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithMariaDbMetric(string metricName = "promitor-maria-db",
            string metricDescription = "Description for a metric",
            string serverName = "promitor-maria-db-name",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new MariaDbResourceV1
            {
                ServerName = serverName
            };

            CreateAndAddMetricDefinition(ResourceType.MariaDb, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithMonitorAutoscaleMetric(string metricName = "promitor-autoscale",
            string metricDescription = "Description for a metric",
            string autoscaleSettingsName = "promitor-autoscale-rules",
            string azureMetricName = "ObservedCapacity",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new MonitorAutoscaleResourceV1
            {
                AutoscaleSettingsName = autoscaleSettingsName
            };

            CreateAndAddMetricDefinition(ResourceType.MonitorAutoscale, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithNetworkGatewayMetric(string metricName = "promitor-network-gateway",
            string metricDescription = "Description for a metric",
            string networkGatewayName = "promitor-network-gateway-name",
            string azureMetricName = "ExpressRouteGatewayPacketsPerSecond",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new NetworkGatewayResourceV1
            {
                NetworkGatewayName = networkGatewayName
            };

            CreateAndAddMetricDefinition(ResourceType.NetworkGateway, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithNetworkInterfaceMetric(string metricName = "promitor-network-interface",
            string metricDescription = "Description for a metric",
            string networkInterfaceName = "promitor-network-interface-name",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new NetworkInterfaceResourceV1
            {
                NetworkInterfaceName = networkInterfaceName
            };

            CreateAndAddMetricDefinition(ResourceType.NetworkInterface, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithPostgreSqlMetric(string metricName = "promitor-postgresql",
            string metricDescription = "Description for a metric",
            string serverName = "promitor-postgresql",
            string azureMetricName = "cpu_percent",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            PostgreSqlServerType serverType = PostgreSqlServerType.Single,
            bool omitResource = false)
        {
            var resource = new PostgreSqlResourceV1
            {
                ServerName = serverName,
                Type = serverType
            };

            CreateAndAddMetricDefinition(ResourceType.PostgreSql, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithRedisCacheMetric(string metricName = "promitor-redis",
            string metricDescription = "Description for a metric",
            string cacheName = "promitor-redis",
            string azureMetricName = "CacheHits",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new RedisCacheResourceV1
            {
                CacheName = cacheName
            };

            CreateAndAddMetricDefinition(ResourceType.RedisCache, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithRedisEnterpriseCacheMetric(string metricName = "promitor-redis-enterprise",
            string metricDescription = "Description for a metric",
            string cacheName = "promitor-redis-enterprise-cache-name",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new RedisEnterpriseCacheResourceV1
            {
                CacheName = cacheName
            };

            CreateAndAddMetricDefinition(ResourceType.RedisEnterpriseCache, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithServiceBusMetric(string metricName = "promitor-service-bus",
            string metricDescription = "Description for a metric",
            string metricDimension = "",
            string queueName = "promitor-queue",
            string topicName = "",
            string serviceBusNamespace = "promitor-namespace",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            bool omitResource = false,
            List<string> queueNames = null,
            int? azureMetricLimit = null,
            Dictionary<string, string> labels = null)
        {
            var serviceBusQueueResources = new List<AzureResourceDefinitionV1>();

            if (queueNames != null)
            {
                foreach (string queue in queueNames)
                {
                    var resource = new ServiceBusNamespaceResourceV1
                    {
                        QueueName = queue,
                        Namespace = serviceBusNamespace
                    };
                    serviceBusQueueResources.Add(resource);
                }
            }
            else
            {
                var resource = new ServiceBusNamespaceResourceV1
                {
                    QueueName = queueName,
                    TopicName = topicName,
                    Namespace = serviceBusNamespace
                };
                serviceBusQueueResources.Add(resource);
            }

            CreateAndAddMetricDefinition(ResourceType.ServiceBusNamespace, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, serviceBusQueueResources, metricDimension, labels);

            return this;
        }

        public MetricsDeclarationBuilder WithSqlDatabaseMetric(
            string metricName = "promitor-sql-db",
            string azureMetricName = "cpu_percent",
            string serverName = "promitor-sql-server",
            string databaseName = "promitor-db",
            string metricDescription = "Metric description",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new SqlDatabaseResourceV1
            {
                ServerName = serverName,
                DatabaseName = databaseName
            };

            CreateAndAddMetricDefinition(ResourceType.SqlDatabase, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithSqlElasticPoolMetric(
            string metricName = "promitor-sql-db",
            string azureMetricName = "cpu_percent",
            string serverName = "promitor-sql-server",
            string poolName = "promitor-elastic-pool",
            string metricDescription = "Metric description",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new SqlElasticPoolResourceV1
            {
                ServerName = serverName,
                PoolName = poolName
            };

            CreateAndAddMetricDefinition(ResourceType.SqlElasticPool, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithSqlServerMetric(
            string metricName = "promitor-sql-server",
            string azureMetricName = "cpu_percent",
            string serverName = "promitor-sql-server",
            string metricDescription = "Metric description",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new SqlServerResourceV1
            {
                ServerName = serverName
            };

            CreateAndAddMetricDefinition(ResourceType.SqlServer, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithSqlManagedInstanceMetric(
            string metricName = "promitor-sql-managed-instance",
            string azureMetricName = "cpu_percent",
            string instanceName = "promitor-sql-instance",
            string metricDescription = "Metric description",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new SqlManagedInstanceResourceV1
            {
                InstanceName = instanceName
            };

            CreateAndAddMetricDefinition(ResourceType.SqlManagedInstance, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithStorageQueueMetric(string metricName = "promitor",
            string metricDescription = "Description for a metric",
            string queueName = "promitor-queue",
            string accountName = "promitor-account",
            string sasToken = "?sig=promitor",
            string azureMetricName = AzureStorageConstants.Queues.Metrics.MessageCount,
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var secret = new SecretV1
            {
                RawValue = sasToken
            };

            var resource = new StorageQueueResourceV1
            {
                QueueName = queueName,
                AccountName = accountName,
                SasToken = secret
            };

            CreateAndAddMetricDefinition(ResourceType.StorageQueue, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithStorageAccountMetric(string metricName = "promitor",
            string metricDescription = "Description for a metric",
            string accountName = "promitor-account",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new StorageAccountResourceV1
            {
                AccountName = accountName
            };

            CreateAndAddMetricDefinition(ResourceType.StorageAccount, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithSynapseApacheSparkPoolMetric(
            string metricName = "promitor-sql-db",
            string azureMetricName = "cpu_percent",
            string workspaceName = "promitor-synapse-workspace",
            string poolName = "promitor-synapse-apache-spark-pool",
            string metricDescription = "Metric description",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new SynapseApacheSparkPoolResourceV1
            {
                WorkspaceName = workspaceName,
                PoolName = poolName
            };

            CreateAndAddMetricDefinition(ResourceType.SynapseApacheSparkPool, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithSynapseSqlPoolMetric(
            string metricName = "promitor-sql-db",
            string azureMetricName = "cpu_percent",
            string workspaceName = "promitor-synapse-workspace",
            string poolName = "promitor-synapse-sql-pool",
            string metricDescription = "Metric description",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new SynapseSqlPoolResourceV1
            {
                WorkspaceName = workspaceName,
                PoolName = poolName
            };

            CreateAndAddMetricDefinition(ResourceType.SynapseSqlPool, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithSynapseWorkspaceMetric(
            string metricName = "promitor-sql-db",
            string azureMetricName = "cpu_percent",
            string workspaceName = "promitor-synapse-workspace",
            string metricDescription = "Metric description",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new SynapseWorkspaceResourceV1
            {
                WorkspaceName = workspaceName
            };

            CreateAndAddMetricDefinition(ResourceType.SynapseWorkspace, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithVirtualMachineMetric(string metricName = "promitor-virtual-machine",
            string metricDescription = "Description for a metric",
            string virtualMachineName = "promitor-virtual-machine-name",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new VirtualMachineResourceV1
            {
                VirtualMachineName = virtualMachineName
            };

            CreateAndAddMetricDefinition(ResourceType.VirtualMachine, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithVirtualMachineScaleSetMetric(string metricName = "promitor-virtual-machine-scale-set",
            string metricDescription = "Description for a metric",
            string scaleSetName = "promitor-scale-set-name",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new VirtualMachineScaleSetResourceV1()
            {
                ScaleSetName = scaleSetName
            };

            CreateAndAddMetricDefinition(ResourceType.VirtualMachineScaleSet, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithVirtualNetworkMetric(string metricName = "promitor-virtual-network",
            string metricDescription = "Description for a metric",
            string virtualNetworkName = "promitor-virtual-network-name",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new VirtualNetworkResourceV1
            {
                VirtualNetworkName = virtualNetworkName
            };

            CreateAndAddMetricDefinition(ResourceType.VirtualNetwork, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        public MetricsDeclarationBuilder WithWebAppMetric(string metricName = "promitor-web-app",
            string metricDescription = "Description for a metric",
            string webAppName = "promitor-web-app-name",
            string slotName = "production",
            string azureMetricName = "Total",
            string resourceDiscoveryGroupName = "",
            int? azureMetricLimit = null,
            bool omitResource = false)
        {
            var resource = new WebAppResourceV1
            {
                WebAppName = webAppName,
                SlotName = slotName
            };

            CreateAndAddMetricDefinition(ResourceType.WebApp, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, resource);

            return this;
        }

        private void CreateAndAddMetricDefinition(ResourceType resourceType, string metricName, string metricDescription, string resourceDiscoveryGroupName, bool omitResource, string azureMetricName, int? azureMetricLimit, AzureResourceDefinitionV1 resource, string metricDimension = null)
        {
            CreateAndAddMetricDefinition(resourceType, metricName, metricDescription, resourceDiscoveryGroupName, omitResource, azureMetricName, azureMetricLimit, new List<AzureResourceDefinitionV1> {resource}, metricDimension);
        }

        private void CreateAndAddMetricDefinition(ResourceType resourceType, string metricName, string metricDescription, string resourceDiscoveryGroupName, bool omitResource, string azureMetricName, int? azureMetricLimit, List<AzureResourceDefinitionV1> resources, string metricDimension = null, Dictionary<string, string> labels = null)
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName, azureMetricLimit, metricDimension);
            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                ResourceType = resourceType,
                Labels = labels
            };

            if (omitResource == false)
            {
                metric.Resources = resources;
            }

            if (string.IsNullOrWhiteSpace(resourceDiscoveryGroupName) == false)
            {
                var resourceDiscoveryGroup = new AzureResourceDiscoveryGroupDefinitionV1 { Name = resourceDiscoveryGroupName };
                metric.ResourceDiscoveryGroups = new List<AzureResourceDiscoveryGroupDefinitionV1>
                {
                    resourceDiscoveryGroup
                };
            }

            _metrics.Add(metric);
        }

        private AzureMetricConfigurationV1 CreateAzureMetricConfiguration(string azureMetricName, int? azureMetricLimit, string metricDimension = "")
        {
            var metricConfig = new AzureMetricConfigurationV1
            {
                MetricName = azureMetricName,
                Limit = azureMetricLimit,
                Aggregation = new MetricAggregationV1
                {
                    Type = AggregationType.Average
                }
            };

            if (string.IsNullOrWhiteSpace(metricDimension) == false)
            {
                metricConfig.Dimension = new MetricDimensionV1
                {
                    Name = metricDimension
                };
            }

            return metricConfig;
        }
    }
}