﻿using Promitor.Agents.ResourceDiscovery.Graph.Exceptions;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public class ResourceDiscoveryFactory
    {
        public static ResourceDiscoveryQuery UseResourceDiscoveryFor(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ApiManagement:
                    return new ApiManagementDiscoveryQuery();
                case ResourceType.ApplicationGateway:
                    return new ApplicationGatewayDiscoveryQuery();
                case ResourceType.AppPlan:
                    return new AppPlanDiscoveryQuery();
                case ResourceType.AutomationAccount:
                    return new AutomationAccountResourceDiscoveryQuery();
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceDiscoveryQuery();
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryDiscoveryQuery();
                case ResourceType.CosmosDb:
                    return new CosmosDbDiscoveryQuery();
                case ResourceType.DeviceProvisioningService:
                    return new DeviceProvisioningServiceDiscoveryQuery();
                case ResourceType.EventHubs:
                    return new EventHubsDiscoveryQuery();
                case ResourceType.ExpressRouteCircuit:
                    return new ExpressRouteCircuitDiscoveryQuery();
                case ResourceType.FrontDoor:
                    return new FrontDoorDiscoveryQuery();
                case ResourceType.FunctionApp:
                    return new FunctionAppDiscoveryQuery();
                case ResourceType.IoTHub:
                    return new IoTHubDiscoveryQuery();
                case ResourceType.KeyVault:
                    return new KeyVaultDiscoveryQuery();
                case ResourceType.KubernetesService:
                    return new KubernetesServiceDiscoveryQuery();
                case ResourceType.LogicApp:
                    return new LogicAppDiscoveryQuery();
                case ResourceType.MonitorAutoscale:
                    return new MonitorAutoscaleDiscoveryQuery();
                case ResourceType.NetworkGateway:
                    return new NetworkGatewayDiscoveryQuery();
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceDiscoveryQuery();
                case ResourceType.RedisCache:
                    return new RedisCacheDiscoveryQuery();
                case ResourceType.PostgreSql:
                    return new PostgreSqlDiscoveryQuery();
                case ResourceType.ServiceBusNamespace:
                    return new ServiceBusNamespaceDiscoveryQuery();
                case ResourceType.SqlDatabase:
                    return new SqlDatabaseDiscoveryQuery();
                case ResourceType.SqlElasticPool:
                    return new SqlElasticPoolDiscoveryQuery();
                case ResourceType.SqlManagedInstance:
                    return new SqlManagedInstanceDiscoveryQuery();
                case ResourceType.StorageAccount:
                    return new StorageAccountDiscoveryQuery();
                case ResourceType.SynapseApacheSparkPool:
                    return new SynapseApacheSparkPoolDiscoveryQuery();
                case ResourceType.SynapseSqlPool:
                    return new SynapseSqlPoolDiscoveryQuery();
                case ResourceType.SynapseWorkspace:
                    return new SynapseWorkspaceDiscoveryQuery();
                case ResourceType.VirtualMachine:
                    return new VirtualMachineDiscoveryQuery();
                case ResourceType.VirtualMachineScaleSet:
                    return new VirtualMachineScaleSetDiscoveryQuery();
                case ResourceType.WebApp:
                    return new WebAppDiscoveryQuery();
                default:
                    throw new ResourceTypeNotSupportedException(resourceType);
            }
        }
    }
}
