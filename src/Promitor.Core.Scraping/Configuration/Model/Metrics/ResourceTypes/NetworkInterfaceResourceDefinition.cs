namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceResourceDefinition : AzureResourceDefinition
    {
        public NetworkInterfaceResourceDefinition() : base(ResourceType.NetworkInterface)
        {
        }

        public NetworkInterfaceResourceDefinition(string resourceGroupName, string networkInterfaceName)
            : base(ResourceType.NetworkInterface, resourceGroupName)
        {
            NetworkInterfaceName = networkInterfaceName;
        }

        public string NetworkInterfaceName { get; set; }
    }
}