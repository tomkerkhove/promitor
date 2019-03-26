using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Core
{
    internal class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfiguration>
    {
        private readonly MetricAggregationDeserializer _metricAggregationDeserializer;
        private readonly YamlScalarNode _metricNode = new YamlScalarNode("metricName");
        private readonly YamlScalarNode _aggregationNode = new YamlScalarNode("aggregation");

        internal AzureMetricConfigurationDeserializer(ILogger logger) : base(logger)
        {
            _metricAggregationDeserializer = new MetricAggregationDeserializer(logger);
        }

        internal override AzureMetricConfiguration Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            var metricName = node.Children[_metricNode];

            MetricAggregation metricAggregation = null;
            if (node.Children.ContainsKey(_aggregationNode))
            {
                var aggregationNode = (YamlMappingNode) node.Children[_aggregationNode];
                metricAggregation = _metricAggregationDeserializer.Deserialize(aggregationNode);
            }

            var azureMetricConfiguration = new AzureMetricConfiguration
            {
                MetricName = metricName?.ToString(),
                Aggregation = metricAggregation
            };

            return azureMetricConfiguration;
        }
    }
}