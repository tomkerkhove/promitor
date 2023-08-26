using System;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes;
using Promitor.Core.Contracts;

namespace Promitor.Agents.Scraper.Validation.Factories
{
    internal static class MetricValidatorFactory
    {
        internal static IMetricValidator GetValidatorFor(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ApiManagement:
                    return new ApiManagementMetricValidator();
                case ResourceType.ApplicationGateway:
                    return new ApplicationGatewayMetricValidator();
                case ResourceType.ApplicationInsights:
                    return new ApplicationInsightsMetricValidator();
                case ResourceType.AppPlan:
                    return new AppPlanMetricValidator();
                case ResourceType.AutomationAccount:
                    return new AutomationAccountMetricValidator();
                case ResourceType.BlobStorage:
                    return new BlobStorageMetricValidator();
                case ResourceType.Cdn:
                    return new CdnMetricValidator();
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceMetricValidator();
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryMetricValidator();
                case ResourceType.CosmosDb:
                    return new CosmosDbMetricValidator();
                case ResourceType.DataExplorerCluster:
                    return new DataExplorerClusterMetricValidator();
                case ResourceType.DataFactory:
                    return new DataFactoryMetricValidator();
                case ResourceType.DataShare:
                    return new DataShareMetricValidator();
                case ResourceType.DeviceProvisioningService:
                    return new DeviceProvisioningServiceMetricValidator();
                case ResourceType.EventHubs:
                    return new EventHubsMetricValidator();
                case ResourceType.ExpressRouteCircuit:
                    return new ExpressRouteCircuitMetricsValidator();
                case ResourceType.FileStorage:
                    return new FileStorageMetricValidator();
                case ResourceType.FrontDoor:
                    return new FrontDoorMetricValidator();
                case ResourceType.FunctionApp:
                    return new FunctionAppMetricValidator();
                case ResourceType.Generic:
                    return new GenericAzureMetricValidator();
                case ResourceType.IoTHub:
                    return new IoTHubMetricValidator();
                case ResourceType.KeyVault:
                    return new KeyVaultMetricValidator();
                case ResourceType.KubernetesService:
                    return new KubernetesServiceMetricValidator();
                case ResourceType.LoadBalancer:
                    return new LoadBalancerMetricValidator();
                case ResourceType.LogAnalytics:
                    return new LogAnalyticsValidator();
                case ResourceType.LogicApp:
                    return new LogicAppMetricValidator();
                case ResourceType.MariaDb:
                    return new MariaDbMetricValidator();
                case ResourceType.MonitorAutoscale:
                    return new MonitorAutoscaleMetricValidator();
                case ResourceType.MySql:
                    return new MySqlMetricValidator();
                case ResourceType.NatGateway:
                    return new NatGatewayMetricValidator();
                case ResourceType.NetworkGateway:
                    return new NetworkGatewayMetricValidator();
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceMetricValidator();
                case ResourceType.PostgreSql:
                    return new PostgreSqlMetricValidator();
                case ResourceType.PowerBiDedicated:
                    return new PowerBiDedicatedMetricValidator();
                case ResourceType.PublicIpAddress:
                    return new PublicIpAddressMetricValidator();
                case ResourceType.RedisCache:
                    return new RedisCacheMetricValidator();
                case ResourceType.RedisEnterpriseCache:
                    return new RedisEnterpriseCacheMetricValidator();
                case ResourceType.ServiceBusNamespace:
                    return new ServiceBusNamespaceMetricValidator();
                case ResourceType.SqlDatabase:
                    return new SqlDatabaseMetricValidator();
                case ResourceType.SqlElasticPool:
                    return new SqlElasticPoolMetricValidator();
                case ResourceType.SqlManagedInstance:
                    return new SqlManagedInstanceMetricValidator();
                case ResourceType.SqlServer:
                    return new SqlServerMetricValidator();
                case ResourceType.StorageAccount:
                    return new StorageAccountMetricValidator();
                case ResourceType.StorageQueue:
                    return new StorageQueueMetricValidator();
                case ResourceType.SynapseApacheSparkPool:
                    return new SynapseApacheSparkPoolMetricValidator();
                case ResourceType.SynapseSqlPool:
                    return new SynapseSqlPoolMetricValidator();
                case ResourceType.SynapseWorkspace:
                    return new SynapseWorkspaceMetricValidator();
                case ResourceType.TrafficManager:
                    return new TrafficManagerMetricValidator();
                case ResourceType.VirtualMachineScaleSet:
                    return new VirtualMachineScaleSetMetricValidator();
                case ResourceType.VirtualNetwork:
                    return new VirtualNetworkMetricValidator();
                case ResourceType.VirtualMachine:
                    return new VirtualMachineMetricValidator();
                case ResourceType.WebApp:
                    return new WebAppMetricValidator();
            }

            throw new ArgumentOutOfRangeException(nameof(resourceType), $"No validation rules are defined for metric type '{resourceType}'");
        }
    }
}
