namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerRegistryMetricDefinition : MetricDefinition
    {
        public string RegistryName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ContainerRegistry;
    }
}