namespace Promitor.Core.Contracts.ResourceTypes
{
    public class CognitiveServicesAccountResourceDefinition : AzureResourceDefinition
    {
        public CognitiveServicesAccountResourceDefinition(string subscriptionId, string resourceGroupName, string cognitiveServicesAccountName)
            : base(ResourceType.CognitiveServicesAccount, subscriptionId, resourceGroupName, cognitiveServicesAccountName)
        {
            CognitiveServicesAccountName = cognitiveServicesAccountName;
        }

        public string CognitiveServicesAccountName { get; }
    }
}
