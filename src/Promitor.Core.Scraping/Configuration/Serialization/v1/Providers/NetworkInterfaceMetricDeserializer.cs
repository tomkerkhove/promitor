using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class NetworkInterfaceMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Network Interface metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node containing 'networkInterfaceName' parameter pointing to an instance of a Network Interface</param>
        /// <returns>A new <see cref="MetricDefinitionV1"/> object (strongly typed as a <see cref="NetworkInterfaceMetricDefinitionV1"/>) </returns>
        internal override MetricDefinitionV1 Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<NetworkInterfaceMetricDefinitionV1>(metricNode);
            var networkInterfaceName = metricNode.Children[new YamlScalarNode("networkInterfaceName")];

            metricDefinition.NetworkInterfaceName = networkInterfaceName?.ToString();

            return metricDefinition;
        }
    }
}