using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        private readonly MetricAggregationDeserializer _metricAggregationDeserializer;
        private readonly YamlScalarNode _metricNode = new YamlScalarNode("metricName");
        private readonly YamlScalarNode _aggregationNode = new YamlScalarNode("aggregation");

        internal AzureMetricConfigurationDeserializer(ILogger logger) : base(logger)
        {
            _metricAggregationDeserializer = new MetricAggregationDeserializer(logger);
        }

        public override AzureMetricConfigurationV1 Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            var metricName = node.Children[_metricNode];

            MetricAggregationV1 metricAggregation = null;
            if (node.Children.ContainsKey(_aggregationNode))
            {
                var aggregationNode = (YamlMappingNode) node.Children[_aggregationNode];
                metricAggregation = _metricAggregationDeserializer.Deserialize(aggregationNode);
            }

            var azureMetricConfiguration = new AzureMetricConfigurationV1
            {
                MetricName = metricName?.ToString(),
                Aggregation = metricAggregation
            };

            return azureMetricConfiguration;
        }
    }
}