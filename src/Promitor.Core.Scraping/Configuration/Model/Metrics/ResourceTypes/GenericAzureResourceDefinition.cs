namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class GenericAzureResourceDefinition : AzureResourceDefinition
    {
        public GenericAzureResourceDefinition(string resourceGroupName, string filter, string resourceUri)
            : base(ResourceType.Generic, resourceGroupName)
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