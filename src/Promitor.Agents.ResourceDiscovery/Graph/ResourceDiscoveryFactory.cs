using System;
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
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
