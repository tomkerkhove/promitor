using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AggregationDeserializer : Deserializer<AggregationV1>
    {
        private const string IntervalTag = "interval";

        private static readonly TimeSpan DefaultAggregationInterval = TimeSpan.FromMinutes(5);

        public AggregationDeserializer(ILogger<AggregationDeserializer> logger) : base(logger)
        {
            MapOptional(aggregation => aggregation.Interval, DefaultAggregationInterval);
        }

        // TODO: Figure out if we want to make Interval required depending on the context
    }
}
