namespace Promitor.Core.Contracts.ResourceTypes
{
    public class KubernetesResourceDefinition : AzureResourceDefinition
    {
        public KubernetesResourceDefinition(string subscriptionId, string resourceGroupName, string clusterName)
            : base(ResourceType.Kubernetes, subscriptionId, resourceGroupName, clusterName)
        {
            ClusterName = clusterName;
        }

        public string ClusterName { get; }
    }
}