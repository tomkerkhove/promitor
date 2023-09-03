using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureResourceDeserializerFactory : IAzureResourceDeserializerFactory
    {
        private readonly IDeserializer<SecretV1> _secretDeserializer;
        private readonly ILoggerFactory _loggerFactory;

        public AzureResourceDeserializerFactory(IDeserializer<SecretV1> secretDeserializer, ILoggerFactory loggerFactory)
        {
            _secretDeserializer = secretDeserializer;
            _loggerFactory = loggerFactory;
        }

        public IDeserializer<AzureResourceDefinitionV1> GetDeserializerFor(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ApiManagement:
                    var apiManagementLogger = _loggerFactory.CreateLogger<ApiManagementDeserializer>();
                    return new ApiManagementDeserializer(apiManagementLogger);
                case ResourceType.ApplicationGateway:
                    var applicationGatewayLogger = _loggerFactory.CreateLogger<ApplicationGatewayDeserializer>();
                    return new ApplicationGatewayDeserializer(applicationGatewayLogger);
                case ResourceType.ApplicationInsights:
                    var applicationInsightsLogger = _loggerFactory.CreateLogger<ApplicationInsightsDeserializer>();
                    return new ApplicationInsightsDeserializer(applicationInsightsLogger);
                case ResourceType.AppPlan:
                    var appPlanLogger = _loggerFactory.CreateLogger<AppPlanDeserializer>();
                    return new AppPlanDeserializer(appPlanLogger);
                case ResourceType.AutomationAccount:
                    var automationLogger = _loggerFactory.CreateLogger<AutomationAccountDeserializer>();
                    return new AutomationAccountDeserializer(automationLogger);
                case ResourceType.BlobStorage:
                    var blobStorageLogger = _loggerFactory.CreateLogger<BlobStorageDeserializer>();
                    return new BlobStorageDeserializer(blobStorageLogger);
                case ResourceType.Cdn:
                    var cdnLogger = _loggerFactory.CreateLogger<CdnDeserializer>();
                    return new CdnDeserializer(cdnLogger);
                case ResourceType.ContainerInstance:
                    var containerInstanceLogger = _loggerFactory.CreateLogger<ContainerInstanceDeserializer>();
                    return new ContainerInstanceDeserializer(containerInstanceLogger);
                case ResourceType.ContainerRegistry:
                    var containerRegistryLogger = _loggerFactory.CreateLogger<ContainerRegistryDeserializer>();
                    return new ContainerRegistryDeserializer(containerRegistryLogger);
                case ResourceType.CosmosDb:
                    var cosmosDbLogger = _loggerFactory.CreateLogger<CosmosDbDeserializer>();
                    return new CosmosDbDeserializer(cosmosDbLogger);
                case ResourceType.DataExplorerCluster:
                    var dataExplorerClusterLogger = _loggerFactory.CreateLogger<DataExplorerClusterDeserializer>();
                    return new DataExplorerClusterDeserializer(dataExplorerClusterLogger);
                case ResourceType.DataFactory:
                    var dataFactoryDeserializer = _loggerFactory.CreateLogger<DataFactoryDeserializer>();
                    return new DataFactoryDeserializer(dataFactoryDeserializer);
                case ResourceType.DataShare:
                    var dataShareLogger = _loggerFactory.CreateLogger<DataShareDeserializer>();
                    return new DataShareDeserializer(dataShareLogger);
                case ResourceType.DeviceProvisioningService:
                    var deviceProvisioningServiceLogger = _loggerFactory.CreateLogger<DeviceProvisioningServiceDeserializer>();
                    return new DeviceProvisioningServiceDeserializer(deviceProvisioningServiceLogger);
                case ResourceType.EventHubs:
                    var eventHubsLogger = _loggerFactory.CreateLogger<EventHubsDeserializer>();
                    return new EventHubsDeserializer(eventHubsLogger);
                case ResourceType.ExpressRouteCircuit:
                    var expressRouteCircuitLogger = _loggerFactory.CreateLogger<ExpressRouteCircuitDeserializer>();
                    return new ExpressRouteCircuitDeserializer(expressRouteCircuitLogger);
                case ResourceType.FileStorage:
                    var fileStorageLogger = _loggerFactory.CreateLogger<FileStorageDeserializer>();
                    return new FileStorageDeserializer(fileStorageLogger);
                case ResourceType.FrontDoor:
                    var frontDoorDeserializer = _loggerFactory.CreateLogger<FrontDoorDeserializer>();
                    return new FrontDoorDeserializer(frontDoorDeserializer);
                case ResourceType.FunctionApp:
                    var functionAppLogger = _loggerFactory.CreateLogger<FunctionAppDeserializer>();
                    return new FunctionAppDeserializer(functionAppLogger);
                case ResourceType.Generic:
                    var genericLogger = _loggerFactory.CreateLogger<GenericResourceDeserializer>();
                    return new GenericResourceDeserializer(genericLogger);
                case ResourceType.IoTHub:
                    var iotHubLogger = _loggerFactory.CreateLogger<IoTHubDeserializer>();
                    return new IoTHubDeserializer(iotHubLogger);
                case ResourceType.KeyVault:
                    var keyVaultLogger = _loggerFactory.CreateLogger<KeyVaultDeserializer>();
                    return new KeyVaultDeserializer(keyVaultLogger);
                case ResourceType.KubernetesService:
                    var kubernetesServiceLogger = _loggerFactory.CreateLogger<KubernetesServiceDeserializer>();
                    return new KubernetesServiceDeserializer(kubernetesServiceLogger);
                case ResourceType.LoadBalancer:
                    var loadBalancerLogger = _loggerFactory.CreateLogger<LoadBalancerDeserializer>();
                    return new LoadBalancerDeserializer(loadBalancerLogger);
                case ResourceType.LogAnalytics:
                    var logAnalyticsLogger = _loggerFactory.CreateLogger<LogAnalyticsDeserializer>();
                    return new LogAnalyticsDeserializer(logAnalyticsLogger);
                case ResourceType.LogicApp:
                    var logicAppLogger = _loggerFactory.CreateLogger<LogicAppDeserializer>();
                    return new LogicAppDeserializer(logicAppLogger);
                case ResourceType.MariaDb:
                    var mariaDbLogger = _loggerFactory.CreateLogger<MariaDbDeserializer>();
                    return new MariaDbDeserializer(mariaDbLogger);
                case ResourceType.MonitorAutoscale:
                    var monitorAutoscaleLogger = _loggerFactory.CreateLogger<MonitorAutoscaleDeserializer>();
                    return new MonitorAutoscaleDeserializer(monitorAutoscaleLogger);
                case ResourceType.MySql:
                    var mySqlLogger = _loggerFactory.CreateLogger<MySqlDeserializer>();
                    return new MySqlDeserializer(mySqlLogger);
                case ResourceType.NatGateway:
                    var natGatewayLogger = _loggerFactory.CreateLogger<NatGatewayDeserializer>();
                    return new NatGatewayDeserializer(natGatewayLogger);
                case ResourceType.NetworkGateway:
                    var networkGatewayLogger = _loggerFactory.CreateLogger<NetworkGatewayDeserializer>();
                    return new NetworkGatewayDeserializer(networkGatewayLogger);
                case ResourceType.NetworkInterface:
                    var networkLogger = _loggerFactory.CreateLogger<NetworkInterfaceDeserializer>();
                    return new NetworkInterfaceDeserializer(networkLogger);
                case ResourceType.PostgreSql:
                    var postgreSqlLogger = _loggerFactory.CreateLogger<PostgreSqlDeserializer>();
                    return new PostgreSqlDeserializer(postgreSqlLogger);
                case ResourceType.PowerBiDedicated:
                    var powerBiDedicatedLogger = _loggerFactory.CreateLogger<PowerBiDedicatedDeserializer>();
                    return new PowerBiDedicatedDeserializer(powerBiDedicatedLogger);
                case ResourceType.PublicIpAddress:
                    var publicIpAddressLogger = _loggerFactory.CreateLogger<PublicIpAddressDeserializer>();
                    return new PublicIpAddressDeserializer(publicIpAddressLogger);
                case ResourceType.RedisCache:
                    var redisCacheLogger = _loggerFactory.CreateLogger<RedisCacheDeserializer>();
                    return new RedisCacheDeserializer(redisCacheLogger);
                case ResourceType.RedisEnterpriseCache:
                    var redisEnterpriseCacheLogger = _loggerFactory.CreateLogger<RedisEnterpriseCacheDeserializer>();
                    return new RedisEnterpriseCacheDeserializer(redisEnterpriseCacheLogger);
                case ResourceType.ServiceBusNamespace:
                    var serviceBusLogger = _loggerFactory.CreateLogger<ServiceBusNamespaceDeserializer>();
                    return new ServiceBusNamespaceDeserializer(serviceBusLogger);
                case ResourceType.SqlDatabase:
                    var sqlDatabaseLogger = _loggerFactory.CreateLogger<SqlDatabaseDeserializer>();
                    return new SqlDatabaseDeserializer(sqlDatabaseLogger);
                case ResourceType.SqlElasticPool:
                    var sqlElasticPoolLogger = _loggerFactory.CreateLogger<SqlElasticPoolDeserializer>();
                    return new SqlElasticPoolDeserializer(sqlElasticPoolLogger);
                case ResourceType.SqlManagedInstance:
                    var sqlManagedInstanceLogger = _loggerFactory.CreateLogger<SqlManagedInstanceDeserializer>();
                    return new SqlManagedInstanceDeserializer(sqlManagedInstanceLogger);
                case ResourceType.SqlServer:
                    var sqlServerLogger = _loggerFactory.CreateLogger<SqlServerDeserializer>();
                    return new SqlServerDeserializer(sqlServerLogger);
                case ResourceType.StorageAccount:
                    var storageAccountLogger = _loggerFactory.CreateLogger<StorageAccountDeserializer>();
                    return new StorageAccountDeserializer(storageAccountLogger);
                case ResourceType.StorageQueue:
                    var storageQueueLogger = _loggerFactory.CreateLogger<StorageQueueDeserializer>();
                    return new StorageQueueDeserializer(_secretDeserializer, storageQueueLogger);
                case ResourceType.SynapseApacheSparkPool:
                    var synapseApacheSparkPoolLogger = _loggerFactory.CreateLogger<SynapseApacheSparkPoolDeserializer>();
                    return new SynapseApacheSparkPoolDeserializer(synapseApacheSparkPoolLogger);
                case ResourceType.SynapseSqlPool:
                    var synapseSqlPoolLogger = _loggerFactory.CreateLogger<SynapseSqlPoolDeserializer>();
                    return new SynapseSqlPoolDeserializer(synapseSqlPoolLogger);
                case ResourceType.SynapseWorkspace:
                    var synapseWorkspaceLogger = _loggerFactory.CreateLogger<SynapseWorkspaceDeserializer>();
                    return new SynapseWorkspaceDeserializer(synapseWorkspaceLogger);
                case ResourceType.TrafficManager:
                    var trafficManagerLogger = _loggerFactory.CreateLogger<TrafficManagerDeserializer>();
                    return new TrafficManagerDeserializer(trafficManagerLogger);
                case ResourceType.VirtualMachine:
                    var virtualMachineLogger = _loggerFactory.CreateLogger<VirtualMachineDeserializer>();
                    return new VirtualMachineDeserializer(virtualMachineLogger);
                case ResourceType.VirtualMachineScaleSet:
                    var virtualMachineScaleSetLogger = _loggerFactory.CreateLogger<VirtualMachineScaleSetDeserializer>();
                    return new VirtualMachineScaleSetDeserializer(virtualMachineScaleSetLogger);
                case ResourceType.VirtualNetwork:
                    var virtualNetworkLogger = _loggerFactory.CreateLogger<VirtualNetworkDeserializer>();
                    return new VirtualNetworkDeserializer(virtualNetworkLogger);
                case ResourceType.WebApp:
                    var webAppLogger = _loggerFactory.CreateLogger<WebAppDeserializer>();
                    return new WebAppDeserializer(webAppLogger);
                default:
                    throw new ArgumentOutOfRangeException($"Resource Type {resourceType} not supported.");
            }
        }
    }
}
