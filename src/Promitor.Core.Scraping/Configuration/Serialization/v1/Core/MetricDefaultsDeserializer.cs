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
            Map(defaults => defaults.Aggregation)
                .MapUsingDeserializer(aggregationDeserializer);
            Map(defaults => defaults.Scraping)
                .IsRequired()
                .MapUsingDeserializer(scrapingDeserializer);
        }
    }
}
