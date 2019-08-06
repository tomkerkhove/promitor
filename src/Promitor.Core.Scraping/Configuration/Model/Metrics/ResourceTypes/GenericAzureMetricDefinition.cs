namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class GenericAzureMetricDefinition : AzureResourceDefinition
    {
        public GenericAzureMetricDefinition() : base(ResourceType.Generic)
        {
        }

        public GenericAzureMetricDefinition(string resourceGroupName, string filter, string resourceUri)
            : base(ResourceType.Generic, resourceGroupName)
        {
            Filter = filter;
            ResourceUri = resourceUri;
        }

        public string Filter { get; set; }
        public string ResourceUri { get; set; }
    }
}