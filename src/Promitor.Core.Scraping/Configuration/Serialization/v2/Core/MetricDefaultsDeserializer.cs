using System;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class MetricDefaultsDeserializer : IDeserializer<MetricDefaultsV2>
    {
        private const string AggregationTag = "aggregation";
        private const string ScrapingTag = "scraping";

        private readonly IDeserializer<AggregationV2> _aggregationDeserializer;
        private readonly IDeserializer<ScrapingV2> _scrapingDeserializer;

        public MetricDefaultsDeserializer(
            IDeserializer<AggregationV2> aggregationDeserializer, IDeserializer<ScrapingV2> scrapingDeserializer)
        {
            _aggregationDeserializer = aggregationDeserializer;
            _scrapingDeserializer = scrapingDeserializer;
        }

        public MetricDefaultsV2 Deserialize(YamlNode node)
        {
            Guard.NotNull(node, nameof(node));

            var mappingNode = node as YamlMappingNode;
            if (mappingNode == null)
            {
                throw new ArgumentException(
                    $"Expected a YamlMappingNode but received '{node.GetType()}'", nameof(node));
            }

            var defaults = new MetricDefaultsV2();

            if (mappingNode.Children.TryGetValue(AggregationTag, out var aggregationNode))
            {
                defaults.Aggregation = _aggregationDeserializer.Deserialize(aggregationNode);
            }

            if (mappingNode.Children.TryGetValue(ScrapingTag, out var scrapingNode))
            {
                defaults.Scraping = _scrapingDeserializer.Deserialize(scrapingNode);
            }

            return defaults;
        }
    }
}
