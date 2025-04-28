using Promitor.Core.Contracts;

namespace Promitor.Core.Contracts.ResourceTypes
{
    public class EventHubClusterResourceDefinition : AzureResourceDefinition
    {
        public EventHubClusterResourceDefinition(string subscriptionId, string resourceGroupName, string clusterName)
            : base(ResourceType.EventHubCluster, subscriptionId, resourceGroupName, clusterName)
        {
            ClusterName = clusterName;
        }

        public string ClusterName { get; }
    }
}