using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class GenericAzureMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Generic Azure metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to query an arbitrary Azure resource</param>
        /// <returns>A new <see cref="MetricDefinitionBuilder" /> object (strongly typed as a <see cref="GenericAzureMetricDefinitionBuilder" />) </returns>
        internal override MetricDefinitionBuilder Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<GenericAzureMetricDefinitionBuilder>(metricNode);

            if (metricNode.Children.TryGetValue(new YamlScalarNode(value: "filter"), out var filterNode))
            {
                metricDefinition.Filter = filterNode?.ToString();
            }

            var resourceUri = metricNode.Children[new YamlScalarNode(value: "resourceUri")];

            metricDefinition.ResourceUri = resourceUri?.ToString();

            return metricDefinition;
        }
    }
}