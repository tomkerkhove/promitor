using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricAggregationDeserializer : Deserializer<MetricAggregationV1>
    {
        public MetricAggregationDeserializer(ILogger<MetricAggregationDeserializer> logger) : base(logger)
        {
            MapRequired(aggregation => aggregation.Type);
            MapOptional(aggregation => aggregation.Interval);
        }
    }
}
