using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class VirtualMachineMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Virtual Machine metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node containing 'virtualMachineName' parameter pointing to an instance of a Virtual Machine</param>
        /// <returns>A new <see cref="MetricDefinitionBuilder"/> object (strongly typed as a <see cref="VirtualMachineMetricDefinitionBuilder"/>) </returns>
        internal override MetricDefinitionBuilder Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<VirtualMachineMetricDefinitionBuilder>(metricNode);
            var virtualMachineName = metricNode.Children[new YamlScalarNode("virtualMachineName")];

            metricDefinition.VirtualMachineName = virtualMachineName?.ToString();

            return metricDefinition;
        }
    }
}