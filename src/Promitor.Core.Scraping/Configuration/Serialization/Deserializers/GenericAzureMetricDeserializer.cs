using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    internal class GenericAzureMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Generic Azure metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node containing 'filter' and 'resourceUri' parameters pointing to an arbitrary Azure resource</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="GenericAzureMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.Deserialize<GenericAzureMetricDefinition>(metricNode);

            if (metricNode.Children.TryGetValue(new YamlScalarNode("filter"), out var filterNode))
            {
                metricDefinition.Filter = filterNode?.ToString();
            }

            var resourceUri = metricNode.Children[new YamlScalarNode("resourceUri")];

            metricDefinition.ResourceUri = resourceUri?.ToString();

            return metricDefinition;
        }
    }
}
