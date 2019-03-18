namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class GenericMetricDefinition : MetricDefinition
    {
        public string Filter { get; set; }
        public override ResourceType ResourceType { get; set; } = ResourceType.Generic;
        public string ResourceUri { get; set; }
    }
}