using AutoMapper;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping
{
    public class V1MappingProfile : Profile
    {
        public V1MappingProfile()
        {
            CreateMap<MetricsDeclarationV1, MetricsDeclaration>();
            CreateMap<AzureMetadataV1, AzureMetadata>();
            CreateMap<MetricDefaultsV1, MetricDefaults>();
            CreateMap<AggregationV1, Aggregation>();
            CreateMap<ScrapingV1, Configuration.Model.Scraping>();
            CreateMap<AzureMetricConfigurationV1, AzureMetricConfiguration>();
            CreateMap<MetricAggregationV1, MetricAggregation>();
            CreateMap<SecretV1, Secret>();

            CreateMap<ContainerInstanceMetricDefinitionV1, ContainerInstanceMetricDefinition>();
            CreateMap<ContainerRegistryMetricDefinitionV1, ContainerRegistryMetricDefinition>();
            CreateMap<CosmosDbMetricDefinitionV1, CosmosDbMetricDefinition>();
            CreateMap<GenericAzureMetricDefinitionV1, GenericAzureMetricDefinition>();
            CreateMap<NetworkInterfaceMetricDefinitionV1, NetworkInterfaceMetricDefinition>();
            CreateMap<PostgreSqlMetricDefinitionV1, PostgreSqlMetricDefinition>();
            CreateMap<RedisCacheMetricDefinitionV1, RedisCacheMetricDefinition>();
            CreateMap<ServiceBusQueueMetricDefinitionV1, ServiceBusQueueMetricDefinition>();
            CreateMap<StorageQueueMetricDefinitionV1, StorageQueueMetricDefinition>();
            CreateMap<VirtualMachineMetricDefinitionV1, VirtualMachineMetricDefinition>();

            CreateMap<MetricDefinitionV1, MetricDefinition>()
                .Include<ContainerInstanceMetricDefinitionV1, ContainerInstanceMetricDefinition>()
                .Include<ContainerRegistryMetricDefinitionV1, ContainerRegistryMetricDefinition>()
                .Include<CosmosDbMetricDefinitionV1, CosmosDbMetricDefinition>()
                .Include<GenericAzureMetricDefinitionV1, GenericAzureMetricDefinition>()
                .Include<NetworkInterfaceMetricDefinitionV1, NetworkInterfaceMetricDefinition>()
                .Include<PostgreSqlMetricDefinitionV1, PostgreSqlMetricDefinition>()
                .Include<RedisCacheMetricDefinitionV1, RedisCacheMetricDefinition>()
                .Include<ServiceBusQueueMetricDefinitionV1, ServiceBusQueueMetricDefinition>()
                .Include<StorageQueueMetricDefinitionV1, StorageQueueMetricDefinition>()
                .Include<VirtualMachineMetricDefinitionV1, VirtualMachineMetricDefinition>();
        }
    }
}
