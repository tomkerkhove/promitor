using AutoMapper;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
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
            CreateMap<LogAnalyticsConfigurationV1, LogAnalyticsConfiguration>();

            CreateMap<MetricAggregationV1, MetricAggregation>();
            CreateMap<AzureResourceDiscoveryGroupDefinitionV1, AzureResourceDiscoveryGroup>();
            CreateMap<SecretV1, Secret>();

            CreateMap<ApiManagementResourceV1, ApiManagementResourceDefinition>();
            CreateMap<ApplicationGatewayResourceV1, ApplicationGatewayResourceDefinition>();
            CreateMap<ApplicationInsightsResourceV1, ApplicationInsightsResourceDefinition>();
            CreateMap<AppPlanResourceV1, AppPlanResourceDefinition>();
            CreateMap<AutomationAccountResourceV1, AutomationAccountResourceDefinition>();
            CreateMap<BlobStorageResourceV1, BlobStorageResourceDefinition>();
            CreateMap<CdnResourceV1, CdnResourceDefinition>();
            CreateMap<ContainerInstanceResourceV1, ContainerInstanceResourceDefinition>();
            CreateMap<ContainerRegistryResourceV1, ContainerRegistryResourceDefinition>();
            CreateMap<DataExplorerClusterResourceV1, DataExplorerClusterResourceDefinition>();
            CreateMap<DataFactoryResourceV1, DataFactoryResourceDefinition>();
            CreateMap<DataShareResourceV1, DataShareResourceDefinition>();
            CreateMap<DeviceProvisioningServiceResourceV1, DeviceProvisioningServiceResourceDefinition>();
            CreateMap<CosmosDbResourceV1, CosmosDbResourceDefinition>();
            CreateMap<EventHubsResourceV1, EventHubResourceDefinition>();
            CreateMap<ExpressRouteCircuitResourceV1, ExpressRouteCircuitResourceDefinition>();
            CreateMap<FileStorageResourceV1, FileStorageResourceDefinition>();
            CreateMap<FrontDoorResourceV1, FrontDoorResourceDefinition>();
            CreateMap<FunctionAppResourceV1, FunctionAppResourceDefinition>();
            CreateMap<GenericResourceV1, GenericAzureResourceDefinition>();
            CreateMap<IoTHubResourceV1, IoTHubResourceDefinition>();
            CreateMap<KeyVaultResourceV1, KeyVaultResourceDefinition>();
            CreateMap<KubernetesServiceResourceV1, KubernetesServiceResourceDefinition>();
            CreateMap<LoadBalancerResourceV1, LoadBalancerResourceDefinition>();
            CreateMap<LogAnalyticsResourceV1, LogAnalyticsResourceDefinition>();
            CreateMap<LogicAppResourceV1, LogicAppResourceDefinition>();
            CreateMap<MariaDbResourceV1, MariaDbResourceDefinition>();
            CreateMap<MonitorAutoscaleResourceV1, MonitorAutoscaleResourceDefinition>();
            CreateMap<MySqlResourceV1, MySqlResourceDefinition>();
            CreateMap<NatGatewayResourceV1, NatGatewayResourceDefinition>();
            CreateMap<NetworkGatewayResourceV1, NetworkGatewayResourceDefinition>();
            CreateMap<NetworkInterfaceResourceV1, NetworkInterfaceResourceDefinition>();
            CreateMap<PostgreSqlResourceV1, PostgreSqlResourceDefinition>();
            CreateMap<PowerBiDedicatedResourceV1, PowerBiDedicatedResourceDefinition>();
            CreateMap<PublicIpAddressResourceV1, PublicIpAddressResourceDefinition>();
            CreateMap<RedisCacheResourceV1, RedisCacheResourceDefinition>();
            CreateMap<RedisEnterpriseCacheResourceV1, RedisEnterpriseCacheResourceDefinition>();
            CreateMap<ServiceBusNamespaceResourceV1, ServiceBusNamespaceResourceDefinition>();
            CreateMap<SqlDatabaseResourceV1, SqlDatabaseResourceDefinition>();
            CreateMap<SqlElasticPoolResourceV1, SqlElasticPoolResourceDefinition>();
            CreateMap<SqlManagedInstanceResourceV1, SqlManagedInstanceResourceDefinition>();
            CreateMap<SqlServerResourceV1, SqlServerResourceDefinition>();
            CreateMap<StorageAccountResourceV1, StorageAccountResourceDefinition>();
            CreateMap<StorageQueueResourceV1, StorageQueueResourceDefinition>();
            CreateMap<SynapseApacheSparkPoolResourceV1, SynapseApacheSparkPoolResourceDefinition>();
            CreateMap<SynapseSqlPoolResourceV1, SynapseSqlPoolResourceDefinition>();
            CreateMap<SynapseWorkspaceResourceV1, SynapseWorkspaceResourceDefinition>();
            CreateMap<TrafficManagerResourceV1, TrafficManagerResourceDefinition>();
            CreateMap<VirtualNetworkResourceV1, VirtualNetworkResourceDefinition>();
            CreateMap<VirtualMachineResourceV1, VirtualMachineResourceDefinition>();
            CreateMap<VirtualMachineScaleSetResourceV1, VirtualMachineScaleSetResourceDefinition>();
            CreateMap<WebAppResourceV1, WebAppResourceDefinition>();

            CreateMap<MetricDefinitionV1, PrometheusMetricDefinition>();

            CreateMap<MetricDefinitionV1, MetricDefinition>()
                .ForMember(m => m.PrometheusMetricDefinition, o => o.MapFrom(v1 => v1));

            CreateMap<AzureResourceDefinitionV1, AzureResourceDefinition>()
                .Include<ApiManagementResourceV1, ApiManagementResourceDefinition>()
                .Include<ApplicationGatewayResourceV1, ApplicationGatewayResourceDefinition>()
                .Include<ApplicationInsightsResourceV1, ApplicationInsightsResourceDefinition>()
                .Include<AppPlanResourceV1, AppPlanResourceDefinition>()
                .Include<AutomationAccountResourceV1, AutomationAccountResourceDefinition>()
                .Include<BlobStorageResourceV1, BlobStorageResourceDefinition>()
                .Include<ContainerInstanceResourceV1, ContainerInstanceResourceDefinition>()
                .Include<ContainerRegistryResourceV1, ContainerRegistryResourceDefinition>()
                .Include<CdnResourceV1, CdnResourceDefinition>()
                .Include<CosmosDbResourceV1, CosmosDbResourceDefinition>()
                .Include<DataExplorerClusterResourceV1, DataExplorerClusterResourceDefinition>()
                .Include<DataFactoryResourceV1, DataFactoryResourceDefinition>()
                .Include<DataShareResourceV1, DataShareResourceDefinition>()
                .Include<DeviceProvisioningServiceResourceV1, DeviceProvisioningServiceResourceDefinition>()
                .Include<EventHubsResourceV1, EventHubResourceDefinition>()
                .Include<ExpressRouteCircuitResourceV1, ExpressRouteCircuitResourceDefinition>()
                .Include<FileStorageResourceV1, FileStorageResourceDefinition>()
                .Include<FrontDoorResourceV1, FrontDoorResourceDefinition>()
                .Include<FunctionAppResourceV1, FunctionAppResourceDefinition>()
                .Include<GenericResourceV1, GenericAzureResourceDefinition>()
                .Include<IoTHubResourceV1, IoTHubResourceDefinition>()
                .Include<KeyVaultResourceV1, KeyVaultResourceDefinition>()
                .Include<KubernetesServiceResourceV1, KubernetesServiceResourceDefinition>()
                .Include<LoadBalancerResourceV1, LoadBalancerResourceDefinition>()
                .Include<LogAnalyticsResourceV1, LogAnalyticsResourceDefinition>()
                .Include<LogicAppResourceV1, LogicAppResourceDefinition>()
                .Include<MariaDbResourceV1, MariaDbResourceDefinition>()
                .Include<MonitorAutoscaleResourceV1, MonitorAutoscaleResourceDefinition>()
                .Include<MySqlResourceV1, MySqlResourceDefinition>()
                .Include<NatGatewayResourceV1, NatGatewayResourceDefinition>()
                .Include<NetworkGatewayResourceV1, NetworkGatewayResourceDefinition>()
                .Include<NetworkInterfaceResourceV1, NetworkInterfaceResourceDefinition>()
                .Include<PostgreSqlResourceV1, PostgreSqlResourceDefinition>()
                .Include<PowerBiDedicatedResourceV1, PowerBiDedicatedResourceDefinition>()
                .Include<PublicIpAddressResourceV1, PublicIpAddressResourceDefinition>()
                .Include<RedisCacheResourceV1, RedisCacheResourceDefinition>()
                .Include<RedisEnterpriseCacheResourceV1, RedisEnterpriseCacheResourceDefinition>()
                .Include<ServiceBusNamespaceResourceV1, ServiceBusNamespaceResourceDefinition>()
                .Include<SqlDatabaseResourceV1, SqlDatabaseResourceDefinition>()
                .Include<SqlElasticPoolResourceV1, SqlElasticPoolResourceDefinition>()
                .Include<SqlManagedInstanceResourceV1, SqlManagedInstanceResourceDefinition>()
                .Include<SqlServerResourceV1, SqlServerResourceDefinition>()
                .Include<StorageAccountResourceV1, StorageAccountResourceDefinition>()
                .Include<StorageQueueResourceV1, StorageQueueResourceDefinition>()
                .Include<SynapseApacheSparkPoolResourceV1, SynapseApacheSparkPoolResourceDefinition>()
                .Include<SynapseSqlPoolResourceV1, SynapseSqlPoolResourceDefinition>()
                .Include<SynapseWorkspaceResourceV1, SynapseWorkspaceResourceDefinition>()
                .Include<TrafficManagerResourceV1, TrafficManagerResourceDefinition>()
                .Include<VirtualNetworkResourceV1, VirtualNetworkResourceDefinition>()
                .Include<VirtualMachineResourceV1, VirtualMachineResourceDefinition>()
                .Include<VirtualMachineScaleSetResourceV1, VirtualMachineScaleSetResourceDefinition>()
                .Include<WebAppResourceV1, WebAppResourceDefinition>();
        }
    }
}
