namespace Promitor.Core.Contracts.ResourceTypes
{
    public class CognitiveServicesAccountsResourceDefinition : AzureResourceDefinition
    {
        public CognitiveServicesAccountsResourceDefinition(string subscriptionId, string resourceGroupName, string cognitiveServicesAccountsName)
            : base(ResourceType.CognitiveServicesAccounts, subscriptionId, resourceGroupName, cognitiveServicesAccountsName)
        {
            CognitiveServicesAccountsName = cognitiveServicesAccountsName;
        }

        public string CognitiveServicesAccountsName { get; }
    }
}
