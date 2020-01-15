using AutoMapper;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

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
            CreateMap<MetricDimensionV1, MetricDimension>();
            CreateMap<ScrapingV1, Configuration.Model.Scraping>();
            CreateMap<AzureMetricConfigurationV1, AzureMetricConfiguration>();
            CreateMap<MetricAggregationV1, MetricAggregation>();
            CreateMap<SecretV1, Secret>();

            CreateMap<ContainerInstanceResourceV1, ContainerInstanceResourceDefinition>();
            CreateMap<ContainerRegistryResourceV1, ContainerRegistryResourceDefinition>();
            CreateMap<CosmosDbResourceV1, CosmosDbResourceDefinition>();
            CreateMap<GenericResourceV1, GenericAzureResourceDefinition>();
            CreateMap<NetworkInterfaceResourceV1, NetworkInterfaceResourceDefinition>();
            CreateMap<PostgreSqlResourceV1, PostgreSqlResourceDefinition>();
            CreateMap<RedisCacheResourceV1, RedisCacheResourceDefinition>();
            CreateMap<ServiceBusQueueResourceV1, ServiceBusQueueResourceDefinition>()
                .ForCtorParam("ns", o => o.MapFrom(s => s.Namespace));
            CreateMap<StorageQueueResourceV1, StorageQueueResourceDefinition>();
            CreateMap<VirtualMachineResourceV1, VirtualMachineResourceDefinition>();
            CreateMap<SqlDatabaseResourceV1, SqlDatabaseResourceDefinition>();
            CreateMap<SqlManagedInstanceResourceV1, SqlManagedInstanceResourceDefinition>();
            CreateMap<VirtualMachineScaleSetResourceV1, VirtualMachineScaleSetResourceDefinition>();
            CreateMap<AppPlanResourceV1, AppPlanResourceDefinition>();
            CreateMap<WebAppResourceV1, WebAppResourceDefinition>();
            CreateMap<FunctionAppResourceV1, FunctionAppResourceDefinition>();

            CreateMap<MetricDefinitionV1, PrometheusMetricDefinition>();

            CreateMap<MetricDefinitionV1, MetricDefinition>()
                .ForMember(m => m.PrometheusMetricDefinition, o => o.MapFrom(v1 => v1));
            
            CreateMap<AzureResourceDefinitionV1, IAzureResourceDefinition>()
                .Include<ContainerInstanceResourceV1, ContainerInstanceResourceDefinition>()
                .Include<ContainerRegistryResourceV1, ContainerRegistryResourceDefinition>()
                .Include<CosmosDbResourceV1, CosmosDbResourceDefinition>()
                .Include<GenericResourceV1, GenericAzureResourceDefinition>()
                .Include<NetworkInterfaceResourceV1, NetworkInterfaceResourceDefinition>()
                .Include<PostgreSqlResourceV1, PostgreSqlResourceDefinition>()
                .Include<RedisCacheResourceV1, RedisCacheResourceDefinition>()
                .Include<ServiceBusQueueResourceV1, ServiceBusQueueResourceDefinition>()
                .Include<StorageQueueResourceV1, StorageQueueResourceDefinition>()
                .Include<VirtualMachineResourceV1, VirtualMachineResourceDefinition>()
                .Include<SqlDatabaseResourceV1, SqlDatabaseResourceDefinition>()
                .Include<SqlManagedInstanceResourceV1, SqlManagedInstanceResourceDefinition>()
                .Include<VirtualMachineScaleSetResourceV1, VirtualMachineScaleSetResourceDefinition>()
                .Include<AppPlanResourceV1, AppPlanResourceDefinition>()
                .Include<WebAppResourceV1, WebAppResourceDefinition>()
                .Include<FunctionAppResourceV1, FunctionAppResourceDefinition>();
        }
    }
}
