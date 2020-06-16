using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AggregationDeserializer : Deserializer<AggregationV1>
    {
        private static readonly TimeSpan defaultAggregationInterval = TimeSpan.FromMinutes(5);

        public AggregationDeserializer(ILogger<AggregationDeserializer> logger) : base(logger)
        {
            Map(aggregation => aggregation.Interval)
                .WithDefault(defaultAggregationInterval);
        }
    }
}
