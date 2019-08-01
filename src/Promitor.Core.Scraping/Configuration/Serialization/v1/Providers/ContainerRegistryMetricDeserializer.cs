using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class ContainerRegistryMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>Deserializes the specified Container Registry metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Container Registry configuration</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="ContainerRegistryMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<ContainerRegistryMetricDefinition>(metricNode);

            var registryName = metricNode.Children[new YamlScalarNode("registryName")];
            metricDefinition.RegistryName = registryName?.ToString();

            return metricDefinition;
        }
    }
}
