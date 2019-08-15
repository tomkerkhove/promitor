using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class ContainerRegistryMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Container Registry metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Container Registry configuration</param>
        /// <returns>A new <see cref="MetricDefinitionV1"/> object (strongly typed as a <see cref="ContainerRegistryMetricDefinitionV1"/>) </returns>
        internal override MetricDefinitionV1 Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<ContainerRegistryMetricDefinitionV1>(metricNode);

            var registryName = metricNode.Children[new YamlScalarNode("registryName")];
            metricDefinition.RegistryName = registryName?.ToString();

            return metricDefinition;
        }
    }
}
