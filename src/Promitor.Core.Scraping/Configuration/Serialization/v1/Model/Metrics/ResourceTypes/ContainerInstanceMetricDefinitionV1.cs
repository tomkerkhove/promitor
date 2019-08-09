using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class ContainerInstanceMetricDefinitionV1 : MetricDefinitionV1
    {
        public string ContainerGroup { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ContainerInstance;

        public override MetricDefinition Build()
        {
            return new ContainerInstanceMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                ContainerGroup,
                Labels,
                Scraping.Build());
        }
    }
}