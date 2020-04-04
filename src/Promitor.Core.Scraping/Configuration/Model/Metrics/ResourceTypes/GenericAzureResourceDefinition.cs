namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class GenericAzureResourceDefinition : AzureResourceDefinition
    {
        public GenericAzureResourceDefinition(string subscriptionId, string resourceGroupName, string filter, string resourceUri)
            : base(ResourceType.Generic, subscriptionId, resourceGroupName)
        {
            Filter = filter;
            ResourceUri = resourceUri;
        }

        public string Filter { get; }
        public string ResourceUri { get; }

        /// <inheritdoc />
        public override string GetResourceName() => null;
    }
}