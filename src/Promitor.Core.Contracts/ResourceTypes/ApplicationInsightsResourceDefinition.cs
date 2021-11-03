namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ApplicationInsightsResourceDefinition : AzureResourceDefinition
    {
        public ApplicationInsightsResourceDefinition(string subscriptionId, string resourceGroupName, string applicationInsightsName)
            : base(ResourceType.ApplicationInsights, subscriptionId, resourceGroupName, applicationGatewayName)
        {
            Name = applicationInsightsName;
        }

        public string Name { get; }
    }
}
