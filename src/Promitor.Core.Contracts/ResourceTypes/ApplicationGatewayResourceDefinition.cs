namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ApplicationGatewayResourceDefinition : AzureResourceDefinition
    {
        public ApplicationGatewayResourceDefinition(string subscriptionId, string resourceGroupName, string applicationGatewayName)
            : base(ResourceType.ApplicationGateway, subscriptionId, resourceGroupName, applicationGatewayName)
        {
            ApplicationGatewayName = applicationGatewayName;
        }

        public string ApplicationGatewayName { get; }
    }
}
