namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerInstanceMetricDefinition : MetricDefinition
    {
        public string ContainerGroup { get; set; }
        public override ResourceType ResourceType { get; set; } = ResourceType.ContainerInstance;
    }
}