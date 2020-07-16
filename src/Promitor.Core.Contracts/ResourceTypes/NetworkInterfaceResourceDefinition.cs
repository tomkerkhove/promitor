namespace Promitor.Core.Contracts.ResourceTypes
{
    public class NetworkInterfaceResourceDefinition : AzureResourceDefinition
    {
        public NetworkInterfaceResourceDefinition(string subscriptionId, string resourceGroupName, string networkInterfaceName)
            : base(ResourceType.NetworkInterface, subscriptionId, resourceGroupName, networkInterfaceName)
        {
            NetworkInterfaceName = networkInterfaceName;
        }

        public string NetworkInterfaceName { get; }
    }
}