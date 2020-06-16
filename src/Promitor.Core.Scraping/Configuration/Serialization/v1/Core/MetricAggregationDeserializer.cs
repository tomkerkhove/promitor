using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricAggregationDeserializer : Deserializer<MetricAggregationV1>
    {
        public MetricAggregationDeserializer(ILogger<MetricAggregationDeserializer> logger) : base(logger)
        {
            Map(aggregation => aggregation.Type)
                .IsRequired();
            Map(aggregation => aggregation.Interval);
        }
    }
}
