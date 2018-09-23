using System;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Scraper.Host.Configuration.Model;
using GuardNet;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Host.Configuration.Serialization
{
    internal class AzureMetricConfigurationDeserializer:Deserializer<AzureMetricConfiguration>
    {
        internal override AzureMetricConfiguration Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            var metricName = node.Children[new YamlScalarNode("metricName")];
            var rawAggregation = node.Children[new YamlScalarNode("aggregation")];

            Enum.TryParse(rawAggregation?.ToString(), out AggregationType aggregationType);

            var azureMetricConfiguration = new AzureMetricConfiguration
            {
                MetricName = metricName?.ToString(),
                Aggregation = aggregationType
            };

            return azureMetricConfiguration;
        }
    }
}