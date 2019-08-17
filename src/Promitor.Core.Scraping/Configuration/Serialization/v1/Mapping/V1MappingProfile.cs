using System.Collections.Generic;
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

            CreateMap<ContainerInstanceMetricDefinitionV1, ContainerInstanceResourceDefinition>();
            CreateMap<ContainerRegistryMetricDefinitionV1, ContainerRegistryResourceDefinition>();
            CreateMap<CosmosDbMetricDefinitionV1, CosmosDbResourceDefinition>();
            CreateMap<GenericAzureMetricDefinitionV1, GenericAzureResourceDefinition>();
            CreateMap<NetworkInterfaceMetricDefinitionV1, NetworkInterfaceResourceDefinition>();
            CreateMap<PostgreSqlMetricDefinitionV1, PostgreSqlResourceDefinition>();
            CreateMap<RedisCacheMetricDefinitionV1, RedisCacheResourceDefinition>();
            CreateMap<ServiceBusQueueMetricDefinitionV1, ServiceBusQueueResourceDefinition>()
                .ForCtorParam("ns", o => o.MapFrom(s => s.Namespace));
            CreateMap<StorageQueueMetricDefinitionV1, StorageQueueResourceDefinition>();
            CreateMap<VirtualMachineMetricDefinitionV1, VirtualMachineResourceDefinition>();

            CreateMap<MetricDefinitionV1, PrometheusMetricDefinition>();
            
            // MetricDefinitionV1 gets expanded into several properties on MetricDefinition
            CreateMap<MetricDefinitionV1, MetricDefinition>()
                .ForMember(m => m.PrometheusMetricDefinition, o => o.MapFrom(v1 => v1))
                .ForMember(m => m.ResourceType, o => o.MapFrom(v1 => v1.ResourceType))
                .ForMember(m => m.Resources, o => o.MapFrom(v1 => new List<MetricDefinitionV1> {v1}));
            
            CreateMap<MetricDefinitionV1, AzureResourceDefinition>()
                .Include<ContainerInstanceMetricDefinitionV1, ContainerInstanceResourceDefinition>()
                .Include<ContainerRegistryMetricDefinitionV1, ContainerRegistryResourceDefinition>()
                .Include<CosmosDbMetricDefinitionV1, CosmosDbResourceDefinition>()
                .Include<GenericAzureMetricDefinitionV1, GenericAzureResourceDefinition>()
                .Include<NetworkInterfaceMetricDefinitionV1, NetworkInterfaceResourceDefinition>()
                .Include<PostgreSqlMetricDefinitionV1, PostgreSqlResourceDefinition>()
                .Include<RedisCacheMetricDefinitionV1, RedisCacheResourceDefinition>()
                .Include<ServiceBusQueueMetricDefinitionV1, ServiceBusQueueResourceDefinition>()
                .Include<StorageQueueMetricDefinitionV1, StorageQueueResourceDefinition>()
                .Include<VirtualMachineMetricDefinitionV1, VirtualMachineResourceDefinition>();
        }
    }
}
