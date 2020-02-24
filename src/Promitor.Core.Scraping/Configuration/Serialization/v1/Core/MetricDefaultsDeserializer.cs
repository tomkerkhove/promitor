using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDefaultsDeserializer : Deserializer<MetricDefaultsV1>
    {
        public MetricDefaultsDeserializer(
            IDeserializer<AggregationV1> aggregationDeserializer,
            IDeserializer<ScrapingV1> scrapingDeserializer,
            ILogger<MetricDefaultsDeserializer> logger) : base(logger)
        {
            MapOptional(defaults => defaults.Aggregation, aggregationDeserializer);
            MapRequired(defaults => defaults.Scraping, scrapingDeserializer);
        }
    }
}
