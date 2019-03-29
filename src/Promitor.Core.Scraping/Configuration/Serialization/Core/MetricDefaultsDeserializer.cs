using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Core
{
    internal class MetricDefaultsDeserializer : Deserializer<MetricDefaults>
    {
        internal MetricDefaultsDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override MetricDefaults Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            var metricDefaults = new MetricDefaults();

            if (node.Children.ContainsKey("aggregation"))
            {
                var metricDefaultsNode = (YamlMappingNode)node.Children[new YamlScalarNode("aggregation")];
                var metricDefaultsSerializer = new AggregationDeserializer(Logger);
                var aggregation = metricDefaultsSerializer.Deserialize(metricDefaultsNode);
                metricDefaults.Aggregation = aggregation;
            }

            if (node.Children.ContainsKey(@"scraping"))
            {
                var metricDefaultsNode = (YamlMappingNode)node.Children[new YamlScalarNode("scraping")];
                var metricDefaultsSerializer = new ScrapingDeserializer(Logger);
                var scraping = metricDefaultsSerializer.Deserialize(metricDefaultsNode);
                metricDefaults.Scraping = scraping;
            }

            return metricDefaults;
        }
    }
}