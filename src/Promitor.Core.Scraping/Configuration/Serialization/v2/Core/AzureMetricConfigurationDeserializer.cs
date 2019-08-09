using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV2>
    {
        private const string MetricNameTag = "metricName";
        private const string AggregationTag = "aggregation";
        private readonly IDeserializer<MetricAggregationV2> _aggregationDeserializer;

        public AzureMetricConfigurationDeserializer(IDeserializer<MetricAggregationV2> aggregationDeserializer, ILogger logger)
            : base(logger)
        {
            _aggregationDeserializer = aggregationDeserializer;
        }

        public override AzureMetricConfigurationV2 Deserialize(YamlMappingNode node)
        {
            return new AzureMetricConfigurationV2
            {
                MetricName = GetString(node, MetricNameTag),
                Aggregation = DeserializeAggregation(node)
            };
        }

        private MetricAggregationV2 DeserializeAggregation(YamlMappingNode node)
        {
            if (node.Children.TryGetValue(AggregationTag, out var aggregationNode))
            {
                return _aggregationDeserializer.Deserialize((YamlMappingNode) aggregationNode);
            }

            return null;
        }
    }
}
