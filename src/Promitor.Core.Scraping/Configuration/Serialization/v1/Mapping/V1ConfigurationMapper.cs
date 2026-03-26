using System;
using System.Collections.Generic;
using System.Linq;
using Promitor.Core.Configuration;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using ScrapingModel = Promitor.Core.Scraping.Configuration.Model.Scraping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping
{
    /// <summary>
    /// Maps V1 configuration model types to domain model types.
    /// Replaces AutoMapper with explicit, hand-written mappings for security (GHSA-rvv3-g6hj-g44x).
    /// </summary>
    public class V1ConfigurationMapper
    {
        private static readonly Dictionary<Type, Func<AzureResourceDefinitionV1, V1ConfigurationMapper, AzureResourceDefinition>> resourceMappers
            = new Dictionary<Type, Func<AzureResourceDefinitionV1, V1ConfigurationMapper, AzureResourceDefinition>>
        {
            [typeof(ApiManagementResourceV1)] = (s, _) => { var r = (ApiManagementResourceV1)s; return new ApiManagementResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.InstanceName, r.LocationName); },
            [typeof(ApplicationGatewayResourceV1)] = (s, _) => { var r = (ApplicationGatewayResourceV1)s; return new ApplicationGatewayResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ApplicationGatewayName); },
            [typeof(ApplicationInsightsResourceV1)] = (s, _) => { var r = (ApplicationInsightsResourceV1)s; return new ApplicationInsightsResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.Name); },
            [typeof(AppPlanResourceV1)] = (s, _) => { var r = (AppPlanResourceV1)s; return new AppPlanResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AppPlanName); },
            [typeof(AutomationAccountResourceV1)] = (s, _) => { var r = (AutomationAccountResourceV1)s; return new AutomationAccountResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AccountName, r.RunbookName); },
            [typeof(AzureFirewallResourceV1)] = (s, _) => { var r = (AzureFirewallResourceV1)s; return new AzureFirewallResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AzureFirewallName); },
            [typeof(BlobStorageResourceV1)] = (s, _) => { var r = (BlobStorageResourceV1)s; return new BlobStorageResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AccountName); },
            [typeof(CdnResourceV1)] = (s, _) => { var r = (CdnResourceV1)s; return new CdnResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.CdnName); },
            [typeof(CognitiveServicesAccountResourceV1)] = (s, _) => { var r = (CognitiveServicesAccountResourceV1)s; return new CognitiveServicesAccountResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.CognitiveServicesAccountName); },
            [typeof(ContainerInstanceResourceV1)] = (s, _) => { var r = (ContainerInstanceResourceV1)s; return new ContainerInstanceResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ContainerGroup); },
            [typeof(ContainerRegistryResourceV1)] = (s, _) => { var r = (ContainerRegistryResourceV1)s; return new ContainerRegistryResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.RegistryName); },
            [typeof(CosmosDbResourceV1)] = (s, _) => { var r = (CosmosDbResourceV1)s; return new CosmosDbResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.DbName); },
            [typeof(DataExplorerClusterResourceV1)] = (s, _) => { var r = (DataExplorerClusterResourceV1)s; return new DataExplorerClusterResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ClusterName); },
            [typeof(DataFactoryResourceV1)] = (s, _) => { var r = (DataFactoryResourceV1)s; return new DataFactoryResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.FactoryName, r.PipelineName); },
            [typeof(DataShareResourceV1)] = (s, _) => { var r = (DataShareResourceV1)s; return new DataShareResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AccountName, r.ShareName); },
            [typeof(DeviceProvisioningServiceResourceV1)] = (s, _) => { var r = (DeviceProvisioningServiceResourceV1)s; return new DeviceProvisioningServiceResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.DeviceProvisioningServiceName); },
            [typeof(DnsZoneResourceV1)] = (s, _) => { var r = (DnsZoneResourceV1)s; return new DnsZoneResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ZoneName); },
            [typeof(EventHubsResourceV1)] = (s, _) => { var r = (EventHubsResourceV1)s; return new EventHubResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.Namespace, r.TopicName); },
            [typeof(ExpressRouteCircuitResourceV1)] = (s, _) => { var r = (ExpressRouteCircuitResourceV1)s; return new ExpressRouteCircuitResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ExpressRouteCircuitName); },
            [typeof(FileStorageResourceV1)] = (s, _) => { var r = (FileStorageResourceV1)s; return new FileStorageResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AccountName); },
            [typeof(FrontDoorResourceV1)] = (s, _) => { var r = (FrontDoorResourceV1)s; return new FrontDoorResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.Name); },
            [typeof(FunctionAppResourceV1)] = (s, _) => { var r = (FunctionAppResourceV1)s; return new FunctionAppResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.FunctionAppName, r.SlotName); },
            [typeof(GenericResourceV1)] = (s, _) => { var r = (GenericResourceV1)s; return new GenericAzureResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.Filter, r.ResourceUri); },
            [typeof(IoTHubResourceV1)] = (s, _) => { var r = (IoTHubResourceV1)s; return new IoTHubResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.IoTHubName); },
            [typeof(KeyVaultResourceV1)] = (s, _) => { var r = (KeyVaultResourceV1)s; return new KeyVaultResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.VaultName); },
            [typeof(KubernetesServiceResourceV1)] = (s, _) => { var r = (KubernetesServiceResourceV1)s; return new KubernetesServiceResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ClusterName); },
            [typeof(LoadBalancerResourceV1)] = (s, _) => { var r = (LoadBalancerResourceV1)s; return new LoadBalancerResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.LoadBalancerName); },
            [typeof(LogAnalyticsResourceV1)] = (s, _) => { var r = (LogAnalyticsResourceV1)s; return new LogAnalyticsResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.WorkspaceName, r.WorkspaceId); },
            [typeof(LogicAppResourceV1)] = (s, _) => { var r = (LogicAppResourceV1)s; return new LogicAppResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.WorkflowName); },
            [typeof(MariaDbResourceV1)] = (s, _) => { var r = (MariaDbResourceV1)s; return new MariaDbResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ServerName); },
            [typeof(MongoClusterResourceV1)] = (s, _) => { var r = (MongoClusterResourceV1)s; return new MongoClusterResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ClusterName); },
            [typeof(MonitorAutoscaleResourceV1)] = (s, _) => { var r = (MonitorAutoscaleResourceV1)s; return new MonitorAutoscaleResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AutoscaleSettingsName); },
            [typeof(MySqlResourceV1)] = (s, _) => { var r = (MySqlResourceV1)s; return new MySqlResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ServerName, r.Type); },
            [typeof(NatGatewayResourceV1)] = (s, _) => { var r = (NatGatewayResourceV1)s; return new NatGatewayResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.NatGatewayName); },
            [typeof(NetworkGatewayResourceV1)] = (s, _) => { var r = (NetworkGatewayResourceV1)s; return new NetworkGatewayResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.NetworkGatewayName); },
            [typeof(NetworkInterfaceResourceV1)] = (s, _) => { var r = (NetworkInterfaceResourceV1)s; return new NetworkInterfaceResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.NetworkInterfaceName); },
            [typeof(PostgreSqlResourceV1)] = (s, _) => { var r = (PostgreSqlResourceV1)s; return new PostgreSqlResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ServerName, r.Type); },
            [typeof(PowerBiDedicatedResourceV1)] = (s, _) => { var r = (PowerBiDedicatedResourceV1)s; return new PowerBiDedicatedResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.CapacityName); },
            [typeof(PublicIpAddressResourceV1)] = (s, _) => { var r = (PublicIpAddressResourceV1)s; return new PublicIpAddressResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.PublicIpAddressName); },
            [typeof(RedisCacheResourceV1)] = (s, _) => { var r = (RedisCacheResourceV1)s; return new RedisCacheResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.CacheName); },
            [typeof(RedisEnterpriseCacheResourceV1)] = (s, _) => { var r = (RedisEnterpriseCacheResourceV1)s; return new RedisEnterpriseCacheResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.CacheName); },
            [typeof(ServiceBusNamespaceResourceV1)] = (s, _) => { var r = (ServiceBusNamespaceResourceV1)s; return new ServiceBusNamespaceResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.Namespace, r.QueueName, r.TopicName); },
            [typeof(SqlDatabaseResourceV1)] = (s, _) => { var r = (SqlDatabaseResourceV1)s; return new SqlDatabaseResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ServerName, r.DatabaseName); },
            [typeof(SqlElasticPoolResourceV1)] = (s, _) => { var r = (SqlElasticPoolResourceV1)s; return new SqlElasticPoolResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ServerName, r.PoolName); },
            [typeof(SqlManagedInstanceResourceV1)] = (s, _) => { var r = (SqlManagedInstanceResourceV1)s; return new SqlManagedInstanceResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.InstanceName); },
            [typeof(SqlServerResourceV1)] = (s, _) => { var r = (SqlServerResourceV1)s; return new SqlServerResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ServerName); },
            [typeof(StorageAccountResourceV1)] = (s, _) => { var r = (StorageAccountResourceV1)s; return new StorageAccountResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AccountName); },
            [typeof(StorageQueueResourceV1)] = (s, m) => { var r = (StorageQueueResourceV1)s; return new StorageQueueResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.AccountName, r.QueueName, m.MapSecret(r.SasToken)); },
            [typeof(SynapseApacheSparkPoolResourceV1)] = (s, _) => { var r = (SynapseApacheSparkPoolResourceV1)s; return new SynapseApacheSparkPoolResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.WorkspaceName, r.PoolName); },
            [typeof(SynapseSqlPoolResourceV1)] = (s, _) => { var r = (SynapseSqlPoolResourceV1)s; return new SynapseSqlPoolResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.WorkspaceName, r.PoolName); },
            [typeof(SynapseWorkspaceResourceV1)] = (s, _) => { var r = (SynapseWorkspaceResourceV1)s; return new SynapseWorkspaceResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.WorkspaceName); },
            [typeof(TrafficManagerResourceV1)] = (s, _) => { var r = (TrafficManagerResourceV1)s; return new TrafficManagerResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.Name); },
            [typeof(VirtualMachineResourceV1)] = (s, _) => { var r = (VirtualMachineResourceV1)s; return new VirtualMachineResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.VirtualMachineName); },
            [typeof(VirtualMachineScaleSetResourceV1)] = (s, _) => { var r = (VirtualMachineScaleSetResourceV1)s; return new VirtualMachineScaleSetResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.ScaleSetName); },
            [typeof(VirtualNetworkResourceV1)] = (s, _) => { var r = (VirtualNetworkResourceV1)s; return new VirtualNetworkResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.VirtualNetworkName); },
            [typeof(WebAppResourceV1)] = (s, _) => { var r = (WebAppResourceV1)s; return new WebAppResourceDefinition(r.SubscriptionId, r.ResourceGroupName, r.WebAppName, r.SlotName); },
        };

        public MetricsDeclaration Map(MetricsDeclarationV1 source)
        {
            if (source == null) return null;

            return new MetricsDeclaration
            {
                AzureMetadata = MapAzureMetadata(source.AzureMetadata),
                MetricDefaults = MapMetricDefaults(source.MetricDefaults),
                Metrics = source.Metrics?.Select(MapMetricDefinition).ToList() ?? new List<MetricDefinition>()
            };
        }

        public AzureMetadata MapAzureMetadata(AzureMetadataV1 source)
        {
            if (source == null) return null;

            return new AzureMetadata
            {
                TenantId = source.TenantId,
                SubscriptionId = source.SubscriptionId,
                ResourceGroupName = source.ResourceGroupName,
                Cloud = source.Cloud,
                Endpoints = MapAzureEndpoints(source.Endpoints)
            };
        }

        public AzureEndpoints MapAzureEndpoints(AzureEndpointsV1 source)
        {
            if (source == null) return null;

            return new AzureEndpoints
            {
                AuthenticationEndpoint = source.AuthenticationEndpoint,
                ResourceManagerEndpoint = source.ResourceManagerEndpoint,
                GraphEndpoint = source.GraphEndpoint,
                ManagementEndpoint = source.ManagementEndpoint,
                StorageEndpointSuffix = source.StorageEndpointSuffix,
                KeyVaultSuffix = source.KeyVaultSuffix,
                MetricsQueryAudience = source.MetricsQueryAudience,
                MetricsClientAudience = source.MetricsClientAudience,
                LogAnalyticsEndpoint = source.LogAnalyticsEndpoint
            };
        }

        public MetricDefaults MapMetricDefaults(MetricDefaultsV1 source)
        {
            if (source == null) return null;

            return new MetricDefaults
            {
                Aggregation = MapAggregation(source.Aggregation),
                Scraping = MapScraping(source.Scraping),
                Limit = source.Limit,
                Labels = source.Labels != null
                    ? new Dictionary<string, string>(source.Labels)
                    : new Dictionary<string, string>()
            };
        }

        public Aggregation MapAggregation(AggregationV1 source)
            => source == null ? null : new Aggregation { Interval = source.Interval };

        public ScrapingModel MapScraping(ScrapingV1 source)
            => source == null ? null : new ScrapingModel { Schedule = source.Schedule };

        public MetricDimension MapMetricDimension(MetricDimensionV1 source)
            => source == null ? null : new MetricDimension { Name = source.Name };

        public AzureMetricConfiguration MapAzureMetricConfiguration(AzureMetricConfigurationV1 source)
        {
            if (source == null) return null;

            return new AzureMetricConfiguration
            {
                MetricName = source.MetricName,
                Limit = source.Limit,
                Dimensions = source.Dimensions?.Select(MapMetricDimension).ToList(),
#pragma warning disable CS0618 // Type or member is obsolete
                Dimension = MapMetricDimension(source.Dimension),
#pragma warning restore CS0618
                Aggregation = MapMetricAggregation(source.Aggregation)
            };
        }

        public LogAnalyticsConfiguration MapLogAnalyticsConfiguration(LogAnalyticsConfigurationV1 source)
        {
            if (source == null) return null;

            return new LogAnalyticsConfiguration
            {
                Query = source.Query,
                Aggregation = MapAggregation(source.Aggregation)
            };
        }

        public MetricAggregation MapMetricAggregation(MetricAggregationV1 source)
        {
            if (source == null) return null;

            return new MetricAggregation
            {
                Interval = source.Interval,
                Type = source.Type.GetValueOrDefault()
            };
        }

        public AzureResourceDiscoveryGroup MapAzureResourceDiscoveryGroup(AzureResourceDiscoveryGroupDefinitionV1 source)
            => source == null ? null : new AzureResourceDiscoveryGroup { Name = source.Name };

        public Secret MapSecret(SecretV1 source)
        {
            if (source == null) return null;

            return new Secret
            {
                RawValue = source.RawValue,
                EnvironmentVariable = source.EnvironmentVariable
            };
        }

        public PrometheusMetricDefinition MapPrometheusMetricDefinition(MetricDefinitionV1 source)
        {
            if (source == null) return null;

            return new PrometheusMetricDefinition(
                source.Name,
                source.Description,
                source.Labels ?? new Dictionary<string, string>());
        }

        public MetricDefinition MapMetricDefinition(MetricDefinitionV1 source)
        {
            if (source == null) return null;

            return new MetricDefinition
            {
                AzureMetricConfiguration = MapAzureMetricConfiguration(source.AzureMetricConfiguration),
                LogAnalyticsConfiguration = MapLogAnalyticsConfiguration(source.LogAnalyticsConfiguration),
                PrometheusMetricDefinition = MapPrometheusMetricDefinition(source),
                Scraping = MapScraping(source.Scraping),
                ResourceType = source.ResourceType.GetValueOrDefault(),
                ResourceDiscoveryGroups = source.ResourceDiscoveryGroups?.Select(MapAzureResourceDiscoveryGroup).ToList()
                    ?? new List<AzureResourceDiscoveryGroup>(),
                Resources = source.Resources?.Select(MapAzureResourceDefinition).ToList()
                    ?? new List<AzureResourceDefinition>()
            };
        }

        public AzureResourceDefinition MapAzureResourceDefinition(AzureResourceDefinitionV1 source)
        {
            if (source == null) return null;

            var sourceType = source.GetType();
            if (resourceMappers.TryGetValue(sourceType, out var mapper))
            {
                return mapper(source, this);
            }

            throw new NotSupportedException(
                $"No mapping defined for resource type '{sourceType.Name}'. " +
                "Register it in V1ConfigurationMapper.resourceMappers.");
        }
    }
}
