using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class GenericAzureMetricDefinitionV1 : MetricDefinitionV1
    {
        public string Filter { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.Generic;
        public string ResourceUri { get; set; }
    }
}