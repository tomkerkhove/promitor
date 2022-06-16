namespace Promitor.Core.Contracts.ResourceTypes
{
    public class LoadBalancerResourceDefinition : AzureResourceDefinition
    {
        public LoadBalancerResourceDefinition(string subscriptionId, string resourceGroupName, string loadBalancerName)
            : base(ResourceType.LoadBalancer, subscriptionId, resourceGroupName, loadBalancerName)
        {
            LoadBalancerName = loadBalancerName;
        }

        public string LoadBalancerName { get; }
    }
}