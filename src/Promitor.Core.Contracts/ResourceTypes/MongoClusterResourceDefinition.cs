namespace Promitor.Core.Contracts.ResourceTypes
{
    public class MongoClusterResourceDefinition : AzureResourceDefinition
    {
        public MongoClusterResourceDefinition(string subscriptionId, string resourceGroupName, string clusterName)
            : base(ResourceType.MongoCluster, subscriptionId, resourceGroupName, clusterName)
        {
            ClusterName = clusterName;
        }

        public string ClusterName { get; }
    }
}