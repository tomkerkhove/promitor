namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ApplicationInsightsResourceDefinition : AzureResourceDefinition
    {
        public ApplicationInsightsResourceDefinition(string subscriptionId, string resourceGroupName, string name)
            : base(ResourceType.ApplicationInsights, subscriptionId, resourceGroupName, name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
