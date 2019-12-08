using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricAggregationDeserializer : Deserializer<MetricAggregationV1>
    {
        private const string TypeTag = "type";
        private const string IntervalTag = "interval";

        public MetricAggregationDeserializer(ILogger<MetricAggregationDeserializer> logger) : base(logger)
        {
        }

        public override MetricAggregationV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var aggregationType = node.GetEnum<AggregationType>(TypeTag);

            var interval = node.GetTimeSpan(IntervalTag);

            return new MetricAggregationV1
            {
                Type = aggregationType,
                Interval = interval
            };
        }
    }
}
