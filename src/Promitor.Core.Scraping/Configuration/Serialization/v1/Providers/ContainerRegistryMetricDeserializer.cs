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
        /// <returns>A new <see cref="MetricDefinitionBuilder"/> object (strongly typed as a <see cref="ContainerRegistryMetricDefinitionBuilder"/>) </returns>
        internal override MetricDefinitionBuilder Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<ContainerRegistryMetricDefinitionBuilder>(metricNode);

            var registryName = metricNode.Children[new YamlScalarNode("registryName")];
            metricDefinition.RegistryName = registryName?.ToString();

            return metricDefinition;
        }
    }
}
