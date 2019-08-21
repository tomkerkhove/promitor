using System.Collections.Generic;
using AutoMapper;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Promitor.Integrations.AzureStorage;

namespace Promitor.Scraper.Tests.Unit.Builders.Metrics.v1
{
    public class MetricsDeclarationBuilder
    {
        private readonly AzureMetadataV2 _azureMetadata;
        private readonly List<MetricDefinitionV2> _metrics = new List<MetricDefinitionV2>();
        private MetricDefaultsV2 _metricDefaults = new MetricDefaultsV2
        {
            Scraping = new ScrapingV2 { Schedule = @"0 * * ? * *" }
        };

        private V2Deserializer _v2Deserializer;

        public MetricsDeclarationBuilder(AzureMetadataV2 azureMetadata)
        {
            _azureMetadata = azureMetadata;
        }

        public static MetricsDeclarationBuilder WithMetadata(string tenantId = "tenantId", string subscriptionId = "subscriptionId", string resourceGroupName = "resourceGroupName")
        {
            var azureMetadata = new AzureMetadataV2
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

        public MetricsDeclarationBuilder WithDefaults(MetricDefaultsV2 defaults)
        {
            _metricDefaults = defaults;

            return this;
        }

        public string Build(IMapper mapper)
        {
            var metricsDeclaration = new MetricsDeclarationV2
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadata = _azureMetadata,
                MetricDefaults = _metricDefaults,
                Metrics = _metrics
            };

            var logger = new Mock<ILogger>();

            _v2Deserializer = new V2Deserializer(
                new AzureMetadataDeserializer(logger.Object),
                new MetricDefaultsDeserializer(
                    new AggregationDeserializer(logger.Object),
                    new ScrapingDeserializer(logger.Object),
                    logger.Object),
                new MetricDefinitionDeserializer(
                    new AzureMetricConfigurationDeserializer(
                        new MetricAggregationDeserializer(logger.Object),
                        logger.Object),
                    new ScrapingDeserializer(logger.Object),
                    new AzureResourceDeserializerFactory(new SecretDeserializer(logger.Object), logger.Object),
                    logger.Object),
                logger.Object);

            var configurationSerializer = new ConfigurationSerializer(NullLogger.Instance, mapper, _v2Deserializer);
            return configurationSerializer.Serialize(metricsDeclaration);
        }

        public MetricsDeclarationBuilder WithServiceBusMetric(string metricName = "promitor-service-bus", string metricDescription = "Description for a metric", string queueName = "promitor-queue", string serviceBusNamespace = "promitor-namespace", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new ServiceBusQueueResourceV2
            {
                QueueName = queueName,
                Namespace = serviceBusNamespace
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.ServiceBusQueue
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithContainerInstanceMetric(string metricName = "promitor-container-instance", string metricDescription = "Description for a metric", string containerGroup = "promitor-group", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new ContainerInstanceResourceV2
            {
                ContainerGroup = containerGroup
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.ContainerInstance
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithContainerRegistryMetric(string metricName = "promitor-container-registry", string metricDescription = "Description for a metric", string registryName = "promitor-container-registry", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new ContainerRegistryResourceV2
            {
                RegistryName = registryName
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.ContainerRegistry
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithCosmosDbMetric(string metricName = "promitor-cosmosdb", string metricDescription = "Description for a metric", string dbName = "promitor-cosmosdb", string azureMetricName = "TotalRequests")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new CosmosDbResourceV2
            {
                DbName = dbName
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.CosmosDb
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithAzureStorageQueueMetric(string metricName = "promitor", string metricDescription = "Description for a metric", string queueName = "promitor-queue", string accountName = "promitor-account", string sasToken = "?sig=promitor", string azureMetricName = AzureStorageConstants.Queues.Metrics.MessageCount)
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var secret = new SecretV2
            {
                RawValue = sasToken
            };

            var resource = new StorageQueueResourceV2
            {
                QueueName = queueName,
                AccountName = accountName,
                SasToken = secret
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> { resource },
                ResourceType = ResourceType.StorageQueue
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithVirtualMachineMetric(string metricName = "promitor-virtual-machine", string metricDescription = "Description for a metric", string virtualMachineName = "promitor-virtual-machine-name", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new VirtualMachineResourceV2
            {
                VirtualMachineName = virtualMachineName
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.VirtualMachine
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithNetworkInterfaceMetric(string metricName = "promitor-network-interface", string metricDescription = "Description for a metric", string networkInterfaceName = "promitor-network-interface-name", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new NetworkInterfaceResourceV2
            {
                NetworkInterfaceName = networkInterfaceName
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.NetworkInterface
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithGenericMetric(string metricName = "foo", string metricDescription = "Description for a metric", string resourceUri = "Microsoft.ServiceBus/namespaces/promitor-messaging", string filter = "EntityName eq \'orders\'", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new GenericResourceV2
            {
                ResourceUri = resourceUri,
                Filter = filter
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.Generic
            };

            _metrics.Add(metric);

            return this;
        }

        private AzureMetricConfigurationV2 CreateAzureMetricConfiguration(string azureMetricName)
        {
            return new AzureMetricConfigurationV2
            {
                MetricName = azureMetricName,
                Aggregation = new MetricAggregationV2
                {
                    Type = AggregationType.Average
                }
            };
        }

        public MetricsDeclarationBuilder WithRedisCacheMetric(string metricName = "promitor-redis", string metricDescription = "Description for a metric", string cacheName = "promitor-redis", string azureMetricName = "CacheHits")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new RedisCacheResourceV2
            {
                CacheName = cacheName
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.RedisCache
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithPostgreSqlMetric(string metricName = "promitor-postgresql", string metricDescription = "Description for a metric", string serverName = "promitor-postgresql", string azureMetricName = "cpu_percent")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var resource = new PostgreSqlResourceV2
            {
                ServerName = serverName
            };

            var metric = new MetricDefinitionV2
            {
                Name = metricName,
                Description = metricDescription,
                AzureMetricConfiguration = azureMetricConfiguration,
                Resources = new List<AzureResourceDefinitionV2> {resource},
                ResourceType = ResourceType.PostgreSql
            };

            _metrics.Add(metric);

            return this;
        }
    }
}