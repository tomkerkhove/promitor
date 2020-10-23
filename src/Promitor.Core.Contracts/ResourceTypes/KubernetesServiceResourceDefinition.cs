namespace Promitor.Core.Contracts.ResourceTypes
{
    public class KubernetesServiceResourceDefinition : AzureResourceDefinition
    {
        public KubernetesServiceResourceDefinition(string subscriptionId, string resourceGroupName, string clusterName)
            : base(ResourceType.KubernetesService, subscriptionId, resourceGroupName, clusterName)
        {
            ClusterName = clusterName;
        }

        public string ClusterName { get; }
    }
}