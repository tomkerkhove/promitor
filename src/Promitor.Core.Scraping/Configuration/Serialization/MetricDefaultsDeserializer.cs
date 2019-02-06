using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    internal class MetricDefaultsDeserializer : Deserializer<MetricDefaults>
    {
        internal MetricDefaultsDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override MetricDefaults Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            Aggregation aggregation = null;
            if (node.Children.ContainsKey("aggregation"))
            {
                var metricDefaultsNode = (YamlMappingNode) node.Children[new YamlScalarNode("aggregation")];
                var metricDefaultsSerializer = new AggregationDeserializer(Logger);
                aggregation = metricDefaultsSerializer.Deserialize(metricDefaultsNode);
            }

            return new MetricDefaults
            {
                Aggregation = aggregation
            };
        }
    }
}