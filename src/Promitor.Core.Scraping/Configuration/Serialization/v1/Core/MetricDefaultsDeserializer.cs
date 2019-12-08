using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDefaultsDeserializer : Deserializer<MetricDefaultsV1>
    {
        private const string AggregationTag = "aggregation";
        private const string ScrapingTag = "scraping";

        private readonly IDeserializer<AggregationV1> _aggregationDeserializer;
        private readonly IDeserializer<ScrapingV1> _scrapingDeserializer;

        public MetricDefaultsDeserializer(
            IDeserializer<AggregationV1> aggregationDeserializer,
            IDeserializer<ScrapingV1> scrapingDeserializer,
            ILogger<MetricDefaultsDeserializer> logger) : base(logger)
        {
            _aggregationDeserializer = aggregationDeserializer;
            _scrapingDeserializer = scrapingDeserializer;
        }

        public override MetricDefaultsV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var defaults = new MetricDefaultsV1();

            defaults.Aggregation = node.DeserializeChild(AggregationTag, _aggregationDeserializer, errorReporter);
            defaults.Scraping = node.DeserializeChild(ScrapingTag, _scrapingDeserializer, errorReporter);

            return defaults;
        }
    }
}
