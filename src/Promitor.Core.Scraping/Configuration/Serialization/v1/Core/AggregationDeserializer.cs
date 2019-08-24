using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AggregationDeserializer : Deserializer<AggregationV1>
    {
        private const string IntervalTag = "interval";

        private readonly TimeSpan _defaultAggregationInterval = TimeSpan.FromMinutes(5);

        public AggregationDeserializer(ILogger logger) : base(logger)
        {
        }

        public override AggregationV1 Deserialize(YamlMappingNode node)
        {
            var interval = node.GetTimeSpan(IntervalTag);

            var aggregation = new AggregationV1 {Interval = interval};

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
