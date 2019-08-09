using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class ContainerRegistryMetricDefinitionV1 : MetricDefinitionV1
    {
        public string RegistryName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ContainerRegistry;

        public override MetricDefinition Build()
        {
            return new ContainerRegistryMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                RegistryName,
                Labels,
                Scraping.Build());
        }
    }
}