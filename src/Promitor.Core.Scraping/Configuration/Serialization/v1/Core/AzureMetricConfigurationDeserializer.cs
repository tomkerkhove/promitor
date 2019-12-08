using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        private const string MetricNameTag = "metricName";
        private const string DimensionTag = "dimension";
        private const string AggregationTag = "aggregation";
        private readonly IDeserializer<MetricDimensionV1> _dimensionDeserializer;
        private readonly IDeserializer<MetricAggregationV1> _aggregationDeserializer;

        public AzureMetricConfigurationDeserializer(IDeserializer<MetricDimensionV1> dimensionDeserializer, IDeserializer<MetricAggregationV1> aggregationDeserializer, ILogger<AzureMetricConfigurationDeserializer> logger)
            : base(logger)
        {
            _dimensionDeserializer = dimensionDeserializer;
            _aggregationDeserializer = aggregationDeserializer;
        }

        public override AzureMetricConfigurationV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            return new AzureMetricConfigurationV1
            {
                MetricName = node.GetString(MetricNameTag),
                Dimension = DeserializeDimension(node, errorReporter),
                Aggregation = DeserializeAggregation(node, errorReporter)
            };
        }

        private MetricAggregationV1 DeserializeAggregation(YamlMappingNode node, IErrorReporter errorReporter)
        {
            if (node.Children.TryGetValue(AggregationTag, out var aggregationNode))
            {
                return _aggregationDeserializer.Deserialize((YamlMappingNode) aggregationNode, errorReporter);
            }

            return null;
        }

        private MetricDimensionV1 DeserializeDimension(YamlMappingNode node, IErrorReporter errorReporter)
        {
            if (node.Children.TryGetValue(DimensionTag, out var aggregationNode))
            {
                return _dimensionDeserializer.Deserialize((YamlMappingNode)aggregationNode, errorReporter);
            }

            return null;
        }
    }
}
