using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class VirtualMachineMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>Deserializes the specified Virtual Machine metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node containing 'virtualMachineName' parameter pointing to an instance of a Virtual Machine</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="VirtualMachineMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<VirtualMachineMetricDefinition>(metricNode);
            var virtualMachineName = metricNode.Children[new YamlScalarNode("virtualMachineName")];

            metricDefinition.VirtualMachineName = virtualMachineName?.ToString();

            return metricDefinition;
        }
    }
}