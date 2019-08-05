namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class ContainerInstanceMetricDefinition : MetricDefinition
    {
        public string ContainerGroup { get; set; }
        public override ResourceType ResourceType { get; } = this.ResourceType.ContainerInstance;
    }
}