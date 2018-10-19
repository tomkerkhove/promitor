using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Integrations.AzureMonitor;
using Promitor.Scraper.Host.Configuration.Model;
using System;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Host.Configuration.Serialization
{
    internal class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfiguration>
    {
        private readonly YamlScalarNode _metricTypeNode = new YamlScalarNode("metricType");
        private readonly YamlScalarNode _metricNameNode = new YamlScalarNode("metricName");
        private readonly YamlScalarNode _aggregationNode = new YamlScalarNode("aggregation"); 
        
        internal override AzureMetricConfiguration Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node)); 
            
            var metricName = node.Children[_metricNameNode];
            AggregationType aggregationType = AggregationType.None;
            MetricType metricType = MetricType.NotSpecified;

            if (node.Children.ContainsKey(_aggregationNode))
            {
                YamlNode rawAggregation = node.Children[_aggregationNode];
                Enum.TryParse(rawAggregation?.ToString(), out aggregationType);
            }

            if (node.Children.ContainsKey(_metricTypeNode))
            {
                YamlNode rawMetricType = node.Children[_metricTypeNode];
                Enum.TryParse(rawMetricType?.ToString(), out metricType);
            }

            var azureMetricConfiguration = new AzureMetricConfiguration
            {
                MetricName = metricName?.ToString(),
                Aggregation = aggregationType,
                MetricType = metricType
            };
            return azureMetricConfiguration;
        }
    }
}