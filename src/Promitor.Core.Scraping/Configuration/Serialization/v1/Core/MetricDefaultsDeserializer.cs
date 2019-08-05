using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class MetricDefaultsDeserializer : Deserializer<MetricDefaultsBuilder>
    {
        internal MetricDefaultsDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override MetricDefaultsBuilder Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            var metricDefaults = new MetricDefaultsBuilder();

            if (node.Children.ContainsKey("aggregation"))
            {
                var metricDefaultsNode = (YamlMappingNode)node.Children[new YamlScalarNode("aggregation")];
                var metricDefaultsSerializer = new AggregationDeserializer(Logger);
                var aggregationBuilder = metricDefaultsSerializer.Deserialize(metricDefaultsNode);
                metricDefaults.AggregationBuilder = aggregationBuilder;
            }

            if (node.Children.ContainsKey(@"scraping"))
            {
                var scrapingNode = (YamlMappingNode)node.Children[new YamlScalarNode("scraping")];
                var scrapingDeserializer = new ScrapingDeserializer(Logger);
                var scrapingBuilder = scrapingDeserializer.Deserialize(scrapingNode);
                metricDefaults.ScrapingBuilder = scrapingBuilder;
            }

            return metricDefaults;
        }
    }
}