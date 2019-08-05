namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class GenericAzureMetricDefinition : MetricDefinition
    {
        public string Filter { get; set; }
        public override ResourceType ResourceType { get; } = this.ResourceType.Generic;
        public string ResourceUri { get; set; }
    }
}