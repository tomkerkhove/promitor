namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class GenericAzureMetricDefinition : MetricDefinition
    {
        public string Filter { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.Generic;
        public string ResourceUri { get; set; }
    }
}