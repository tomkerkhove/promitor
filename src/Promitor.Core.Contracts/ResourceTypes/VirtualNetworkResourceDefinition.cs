namespace Promitor.Core.Contracts.ResourceTypes
{
    public class VirtualNetworkResourceDefinition : AzureResourceDefinition
    {
        public VirtualNetworkResourceDefinition(string subscriptionId, string resourceGroupName, string virtualNetworkName)
            : base(ResourceType.VirtualNetwork, subscriptionId, resourceGroupName, virtualNetworkName)
        {
            VirtualNetworkName = virtualNetworkName;
        }

        public string VirtualNetworkName { get; }
    }
}