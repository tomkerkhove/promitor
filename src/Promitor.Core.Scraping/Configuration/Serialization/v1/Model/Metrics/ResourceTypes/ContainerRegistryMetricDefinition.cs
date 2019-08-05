namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class ContainerRegistryMetricDefinition : MetricDefinition
    {
        public string RegistryName { get; set; }
        public override ResourceType ResourceType { get; } = this.ResourceType.ContainerRegistry;
    }
}