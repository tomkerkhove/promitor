using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class MetricAggregationDeserializer : Deserializer<MetricAggregationV2>
    {
        private const string TypeTag = "type";
        private const string IntervalTag = "interval";

        public MetricAggregationDeserializer(ILogger logger) : base(logger)
        {
        }

        public override MetricAggregationV2 Deserialize(YamlMappingNode node)
        {
            return new MetricAggregationV2
            {
                Type = GetEnum<AggregationType>(node, TypeTag),
                Interval = GetTimespan(node, IntervalTag)
            };
        }
    }
}
