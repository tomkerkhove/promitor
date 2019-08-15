using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class GenericAzureMetricDefinition : MetricDefinition
    {
        public GenericAzureMetricDefinition()
        {
        }

        public GenericAzureMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string filter, string resourceUri, Dictionary<string, string> labels, Scraping scraping)
            : base(name, description, resourceGroupName, labels, scraping, azureMetricConfiguration)
        {
            Filter = filter;
            ResourceUri = resourceUri;
        }

        public string Filter { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.Generic;
        public string ResourceUri { get; set; }
    }
}