using System.Collections.Generic;
using AutoMapper;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Integrations.AzureStorage;
using Promitor.Scraper.Tests.Unit.Serialization.v1;

namespace Promitor.Scraper.Tests.Unit.Builders.Metrics.v1
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

        public MetricsDeclarationBuilder WithServiceBusMetric(string metricName = "promitor-service-bus", string metricDescription = "Description for a metric", string metricDimension = "", string queueName = "promitor-queue", string serviceBusNamespace = "promitor-namespace", string azureMetricName = "Total", bool omitResource = false)
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName, metricDimension);
            var resource = new ServiceBusQueueResourceV1
            {
                QueueName = queueName,
                Namespace = serviceBusNamespace
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                ResourceType = ResourceType.ServiceBusQueue
            };

            if (omitResource == false)
            {
                metric.Resources = new List<AzureResourceDefinitionV1> { resource };
            }

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithContainerInstanceMetric(string metricName = "promitor-container-instance", string metricDescription = "Description for a metric", string containerGroup = "promitor-group", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new ContainerInstanceResourceV1
            {
                ContainerGroup = containerGroup
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.ContainerInstance
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithContainerRegistryMetric(string metricName = "promitor-container-registry", string metricDescription = "Description for a metric", string registryName = "promitor-container-registry", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new ContainerRegistryResourceV1
            {
                RegistryName = registryName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.ContainerRegistry
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithCosmosDbMetric(string metricName = "promitor-cosmosdb", string metricDescription = "Description for a metric", string dbName = "promitor-cosmosdb", string azureMetricName = "TotalRequests")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new CosmosDbResourceV1
            {
                DbName = dbName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.CosmosDb
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithAppPlanMetric(string metricName = "promitor-app-plan", string metricDescription = "Description for a metric", string appPlanName = "promitor-app-plan", string azureMetricName = "TotalRequests")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new AppPlanResourceV1
            {
                AppPlanName = appPlanName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.AppPlan
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithFunctionAppMetric(string metricName = "promitor-fuction-app", string metricDescription = "Description for a metric", string functionAppName = "promitor-fuction-app", string azureMetricName = "TotalRequests")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new FunctionAppResourceV1
            {
                FunctionAppName = functionAppName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.FunctionApp
            };

            _metrics.Add(metric);

            return this;
        }
        
        public MetricsDeclarationBuilder WithAzureStorageQueueMetric(string metricName = "promitor", string metricDescription = "Description for a metric", string queueName = "promitor-queue", string accountName = "promitor-account", string sasToken = "?sig=promitor", string azureMetricName = AzureStorageConstants.Queues.Metrics.MessageCount)
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
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

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.StorageQueue
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithVirtualMachineMetric(string metricName = "promitor-virtual-machine", string metricDescription = "Description for a metric", string virtualMachineName = "promitor-virtual-machine-name", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new VirtualMachineResourceV1
            {
                VirtualMachineName = virtualMachineName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.VirtualMachine
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithWebAppMetric(string metricName = "promitor-web-app", string metricDescription = "Description for a metric", string webAppName = "promitor-web-app-name", string slotName = "production", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new WebAppResourceV1
            {
                WebAppName = webAppName,
                SlotName = slotName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.WebApp
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithVirtualMachineScaleSetMetric(string metricName = "promitor-virtual-machine-scale-set", string metricDescription = "Description for a metric", string scaleSetName = "promitor-scale-set-name", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new VirtualMachineScaleSetResourceV1()
            {
                ScaleSetName = scaleSetName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.VirtualMachineScaleSet
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithNetworkInterfaceMetric(string metricName = "promitor-network-interface", string metricDescription = "Description for a metric", string networkInterfaceName = "promitor-network-interface-name", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new NetworkInterfaceResourceV1
            {
                NetworkInterfaceName = networkInterfaceName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.NetworkInterface
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithGenericMetric(string metricName = "foo", string metricDescription = "Description for a metric", string resourceUri = "Microsoft.ServiceBus/namespaces/promitor-messaging", string filter = "EntityName eq \'orders\'", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new GenericResourceV1
            {
                ResourceUri = resourceUri,
                Filter = filter
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.Generic
            };

            _metrics.Add(metric);

            return this;
        }

        private AzureMetricConfigurationV1 CreateAzureMetricConfiguration(string azureMetricName, string metricDimension = "")
        {
            var metricConfig = new AzureMetricConfigurationV1
            {
                MetricName = azureMetricName,
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

        public MetricsDeclarationBuilder WithRedisCacheMetric(string metricName = "promitor-redis", string metricDescription = "Description for a metric", string cacheName = "promitor-redis", string azureMetricName = "CacheHits")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new RedisCacheResourceV1
            {
                CacheName = cacheName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.RedisCache
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithPostgreSqlMetric(string metricName = "promitor-postgresql", string metricDescription = "Description for a metric", string serverName = "promitor-postgresql", string azureMetricName = "cpu_percent")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new PostgreSqlResourceV1
            {
                ServerName = serverName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.PostgreSql
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithSqlDatabaseMetric(
            string metricName = "promitor-sql-db",
            string azureMetricName = "cpu_percent",
            string serverName = "promitor-sql-server",
            string databaseName = "promitor-db",
            string metricDescription = "Metric description")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new SqlDatabaseResourceV1
            {
                ServerName = serverName,
                DatabaseName = databaseName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.SqlDatabase
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithSqlManagedInstanceMetric(
            string metricName = "promitor-sql-managed-instance",
            string azureMetricName = "cpu_percent",
            string instanceName = "promitor-sql-instance",
            string metricDescription = "Metric description")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new SqlManagedInstanceResourceV1
            {
                InstanceName = instanceName
            };

            var metric = new MetricDefinitionV1
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV1> { resource },
                ResourceType = ResourceType.SqlManagedInstance
            };

            _metrics.Add(metric);

            return this;
        }
    }
}