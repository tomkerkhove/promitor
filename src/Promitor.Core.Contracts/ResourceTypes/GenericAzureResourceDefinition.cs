namespace Promitor.Core.Contracts.ResourceTypes
{
    public class GenericAzureResourceDefinition : AzureResourceDefinition
    {
        public GenericAzureResourceDefinition(string subscriptionId, string resourceGroupName, string filter, string resourceUri)
            : base(ResourceType.Generic, subscriptionId, resourceGroupName, resourceUri, resourceUri)
        {
            Filter = filter;
            ResourceUri = resourceUri;
        }

        public string Filter { get; }
        public string ResourceUri { get; }
    }
}