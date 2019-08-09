using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class ContainerInstanceMetricDefinitionV1 : MetricDefinitionV1
    {
        public string ContainerGroup { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ContainerInstance;
    }
}