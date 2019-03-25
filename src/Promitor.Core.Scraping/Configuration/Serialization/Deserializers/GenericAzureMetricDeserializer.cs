using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    internal class GenericAzureMetricDeserializer : MetricDeserializer
    {
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
