namespace Promitor.Core.Contracts.ResourceTypes
{
    public class DataExplorerClusterResourceDefinition : AzureResourceDefinition
    {
        public DataExplorerClusterResourceDefinition(string subscriptionId, string resourceGroupName, string clusterName)
            : base(ResourceType.DataExplorerCluster, subscriptionId, resourceGroupName, clusterName)
        {
            ClusterName = clusterName;
        }

        public string ClusterName { get; }
    }
}