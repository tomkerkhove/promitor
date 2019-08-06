namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceMetricDefinition : AzureResourceDefinition
    {
        public NetworkInterfaceMetricDefinition() : base(ResourceType.NetworkInterface)
        {
        }

        public NetworkInterfaceMetricDefinition(string resourceGroupName, string networkInterfaceName)
            : base(ResourceType.NetworkInterface, resourceGroupName)
        {
            NetworkInterfaceName = networkInterfaceName;
        }

        public string NetworkInterfaceName { get; set; }
    }
}