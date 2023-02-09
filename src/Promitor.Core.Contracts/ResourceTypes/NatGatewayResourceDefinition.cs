namespace Promitor.Core.Contracts.ResourceTypes
{
    public class NatGatewayResourceDefinition : AzureResourceDefinition
    {
        public NatGatewayResourceDefinition(string subscriptionId, string resourceGroupName, string natGatewayName)
            : base(ResourceType.NatGateway, subscriptionId, resourceGroupName, natGatewayName)
        {
            NatGatewayName = natGatewayName;
        }

        public string NatGatewayName { get; }
    }
}
