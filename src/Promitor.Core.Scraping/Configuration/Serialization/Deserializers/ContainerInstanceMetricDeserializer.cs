using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    internal class ContainerInstanceMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>Deserializes the specified Container Instances metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Container Instances configuration</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="ContainerInstanceMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<ContainerInstanceMetricDefinition>(metricNode);
            
            var containerGroup = metricNode.Children[new YamlScalarNode("containerGroup")];
            metricDefinition.ContainerGroup = containerGroup?.ToString();

            return metricDefinition;
        }
    }
}
