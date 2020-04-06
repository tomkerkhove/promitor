namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceResourceDefinition : AzureResourceDefinition
    {
        public NetworkInterfaceResourceDefinition(string subscriptionId, string resourceGroupName, string networkInterfaceName)
            : base(ResourceType.NetworkInterface, subscriptionId, resourceGroupName)
        {
            NetworkInterfaceName = networkInterfaceName;
        }

        public string NetworkInterfaceName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => NetworkInterfaceName;
    }
}