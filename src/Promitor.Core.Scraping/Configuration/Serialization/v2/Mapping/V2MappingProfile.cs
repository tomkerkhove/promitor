using AutoMapper;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Mapping
{
    public class V2MappingProfile : Profile
    {
        public V2MappingProfile()
        {
            CreateMap<MetricsDeclarationV2, MetricsDeclaration>();
            CreateMap<AzureMetadataV2, AzureMetadata>();
            CreateMap<MetricDefaultsV2, MetricDefaults>();
            CreateMap<AggregationV2, Aggregation>();
            CreateMap<ScrapingV2, Configuration.Model.Scraping>();
            CreateMap<AzureMetricConfigurationV2, AzureMetricConfiguration>();
            CreateMap<MetricAggregationV2, MetricAggregation>();
            CreateMap<SecretV2, Secret>();

            CreateMap<ContainerInstanceResourceV2, ContainerInstanceResourceDefinition>();
            CreateMap<ContainerRegistryResourceV2, ContainerRegistryResourceDefinition>();
            CreateMap<CosmosDbResourceV2, CosmosDbResourceDefinition>();
            CreateMap<GenericResourceV2, GenericAzureResourceDefinition>();
            CreateMap<NetworkInterfaceResourceV2, NetworkInterfaceResourceDefinition>();
            CreateMap<PostgreSqlResourceV2, PostgreSqlResourceDefinition>();
            CreateMap<RedisCacheResourceV2, RedisCacheResourceDefinition>();
            CreateMap<ServiceBusQueueResourceV2, ServiceBusQueueResourceDefinition>()
                .ForCtorParam("ns", o => o.MapFrom(s => s.Namespace));
            CreateMap<StorageQueueResourceV2, StorageQueueResourceDefinition>();
            CreateMap<VirtualMachineResourceV2, VirtualMachineResourceDefinition>();

            CreateMap<MetricDefinitionV2, PrometheusMetricDefinition>();

            CreateMap<MetricDefinitionV2, MetricDefinition>()
                .ForMember(m => m.PrometheusMetricDefinition, o => o.MapFrom(v2 => v2));
            
            CreateMap<AzureResourceDefinitionV2, AzureResourceDefinition>()
                .Include<ContainerInstanceResourceV2, ContainerInstanceResourceDefinition>()
                .Include<ContainerRegistryResourceV2, ContainerRegistryResourceDefinition>()
                .Include<CosmosDbResourceV2, CosmosDbResourceDefinition>()
                .Include<GenericResourceV2, GenericAzureResourceDefinition>()
                .Include<NetworkInterfaceResourceV2, NetworkInterfaceResourceDefinition>()
                .Include<PostgreSqlResourceV2, PostgreSqlResourceDefinition>()
                .Include<RedisCacheResourceV2, RedisCacheResourceDefinition>()
                .Include<ServiceBusQueueResourceV2, ServiceBusQueueResourceDefinition>()
                .Include<StorageQueueResourceV2, StorageQueueResourceDefinition>()
                .Include<VirtualMachineResourceV2, VirtualMachineResourceDefinition>();
        }
    }
}
