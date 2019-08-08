using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class MetricDefaultsDeserializer : Deserializer<MetricDefaultsV2>
    {
        private const string AggregationTag = "aggregation";
        private const string ScrapingTag = "scraping";

        private readonly IDeserializer<AggregationV2> _aggregationDeserializer;
        private readonly IDeserializer<ScrapingV2> _scrapingDeserializer;

        public MetricDefaultsDeserializer(
            IDeserializer<AggregationV2> aggregationDeserializer,
            IDeserializer<ScrapingV2> scrapingDeserializer,
            ILogger logger) : base(logger)
        {
            _aggregationDeserializer = aggregationDeserializer;
            _scrapingDeserializer = scrapingDeserializer;
        }

        public override MetricDefaultsV2 Deserialize(YamlMappingNode node)
        {
            var defaults = new MetricDefaultsV2();

            if (node.Children.TryGetValue(AggregationTag, out var aggregationNode))
            {
                defaults.Aggregation = _aggregationDeserializer.Deserialize((YamlMappingNode)aggregationNode);
            }

            if (node.Children.TryGetValue(ScrapingTag, out var scrapingNode))
            {
                defaults.Scraping = _scrapingDeserializer.Deserialize((YamlMappingNode)scrapingNode);
            }

            return defaults;
        }
    }
}
