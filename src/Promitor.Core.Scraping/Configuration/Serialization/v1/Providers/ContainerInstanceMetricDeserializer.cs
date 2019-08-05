using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class ContainerInstanceMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Container Instances metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Container Instances configuration</param>
        /// <returns>A new <see cref="MetricDefinitionBuilder"/> object (strongly typed as a <see cref="ContainerInstanceMetricDefinitionBuilder"/>) </returns>
        internal override MetricDefinitionBuilder Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<ContainerInstanceMetricDefinitionBuilder>(metricNode);
            
            var containerGroup = metricNode.Children[new YamlScalarNode("containerGroup")];
            metricDefinition.ContainerGroup = containerGroup?.ToString();

            return metricDefinition;
        }
    }
}
