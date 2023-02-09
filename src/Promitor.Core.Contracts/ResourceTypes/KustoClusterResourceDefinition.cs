namespace Promitor.Core.Contracts.ResourceTypes
{
    public class KustoClusterResourceDefinition : AzureResourceDefinition
    {
        public KustoClusterResourceDefinition(string subscriptionId, string resourceGroupName, string kustoClusterName)
            : base(ResourceType.KustoCluster, subscriptionId, resourceGroupName, kustoClusterName)
        {
            KustoClusterName = kustoClusterName;
        }

        public string KustoClusterName { get; }
    }
}