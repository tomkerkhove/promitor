namespace Promitor.Core.Contracts.ResourceTypes
{
    public class NetworkGatewayResourceDefinition : AzureResourceDefinition
    {
        public NetworkGatewayResourceDefinition(string subscriptionId, string resourceGroupName, string networkGatewayName)
            : base(ResourceType.NetworkGateway, subscriptionId, resourceGroupName, networkGatewayName)
        {
            NetworkGatewayName = networkGatewayName;
        }

        public string NetworkGatewayName { get; }
    }
}
