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
                case ResourceType.AppPlan:
                    return new AppPlanDiscoveryQuery();
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryDiscoveryQuery();
                case ResourceType.FunctionApp:
                    return new FunctionAppDiscoveryQuery();
                case ResourceType.LogicApp:
                    return new LogicAppDiscoveryQuery();
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
