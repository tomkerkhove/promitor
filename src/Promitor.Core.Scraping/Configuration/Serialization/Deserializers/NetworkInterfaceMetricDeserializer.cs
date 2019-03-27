using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    internal class NetworkInterfaceMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>Deserializes the specified Network Interface metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node containing 'networkInterfaceName' parameter pointing to an instance of a Network Interface</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="NetworkInterfaceMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<NetworkInterfaceMetricDefinition>(metricNode);
            var networkInterfaceName = metricNode.Children[new YamlScalarNode("networkInterfaceName")];

            metricDefinition.NetworkInterfaceName = networkInterfaceName?.ToString();

            return metricDefinition;
        }
    }
}