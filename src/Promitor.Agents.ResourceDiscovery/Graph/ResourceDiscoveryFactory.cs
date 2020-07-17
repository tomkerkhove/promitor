using Promitor.Agents.ResourceDiscovery.Graph.Exceptions;
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
                case ResourceType.AppPlan:
                    return new AppPlanDiscoveryQuery();
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceDiscoveryQuery();
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryDiscoveryQuery();
                case ResourceType.CosmosDb:
                    return new CosmosDbDiscoveryQuery();
                case ResourceType.DeviceProvisioningService:
                    return new DeviceProvisioningServiceDiscoveryQuery();
                case ResourceType.FunctionApp:
                    return new FunctionAppDiscoveryQuery();
                case ResourceType.IoTHub:
                    return new IoTHubDiscoveryQuery();
                case ResourceType.KeyVault:
                    return new KeyVaultDiscoveryQuery();
                case ResourceType.LogicApp:
                    return new LogicAppDiscoveryQuery();
                case ResourceType.RedisCache:
                    return new RedisCacheDiscoveryQuery();
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
