﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        private const string MetricNameTag = "metricName";
        private const string DimensionNameTag = "dimensionName";
        private const string AggregationTag = "aggregation";
        private readonly IDeserializer<MetricAggregationV1> _aggregationDeserializer;

        public AzureMetricConfigurationDeserializer(IDeserializer<MetricAggregationV1> aggregationDeserializer, ILogger<AzureMetricConfigurationDeserializer> logger)
            : base(logger)
        {
            _aggregationDeserializer = aggregationDeserializer;
        }

        public override AzureMetricConfigurationV1 Deserialize(YamlMappingNode node)
        {
            return new AzureMetricConfigurationV1
            {
                MetricName = node.GetString(MetricNameTag),
                DimensionName = node.GetString(DimensionNameTag),
                Aggregation = DeserializeAggregation(node)
            };
        }

        private MetricAggregationV1 DeserializeAggregation(YamlMappingNode node)
        {
            if (node.Children.TryGetValue(AggregationTag, out var aggregationNode))
            {
                return _aggregationDeserializer.Deserialize((YamlMappingNode) aggregationNode);
            }

            return null;
        }
    }
}
