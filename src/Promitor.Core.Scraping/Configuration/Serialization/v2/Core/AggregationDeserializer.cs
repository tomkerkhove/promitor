using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class AggregationDeserializer : Deserializer<AggregationV2>
    {
        private const string IntervalTag = "interval";

        private readonly TimeSpan _defaultAggregationInterval = TimeSpan.FromMinutes(5);

        public AggregationDeserializer(ILogger logger) : base(logger)
        {
        }

        public override AggregationV2 Deserialize(YamlMappingNode node)
        {
            var aggregation = new AggregationV2 {Interval = GetTimespan(node, IntervalTag)};

            if (aggregation.Interval == null)
            {
                aggregation.Interval = _defaultAggregationInterval;
                Logger.LogWarning(
                    "No default aggregation was configured, falling back to {AggregationInterval}",
                    aggregation.Interval?.ToString("g"));
            }

            return aggregation;
        }
    }
}
