using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Scraper.Host.Configuration.Model;
using System;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Host.Configuration.Serialization
{
    internal class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfiguration>
    {
        private readonly YamlScalarNode _metricNode = new YamlScalarNode("metricName");
        private readonly YamlScalarNode _aggregationNode = new YamlScalarNode("aggregation"); 
        
        internal override AzureMetricConfiguration Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node)); 
            
            var metricName = node.Children[_metricNode];
            AggregationType aggregationType = AggregationType.None;

            if (node.Children.ContainsKey(_aggregationNode))
            {
                var rawAggregation = node.Children[_aggregationNode];
                Enum.TryParse(rawAggregation?.ToString(), out aggregationType);
            }

            var azureMetricConfiguration = new AzureMetricConfiguration
            {
                MetricName = metricName?.ToString(),
                Aggregation = aggregationType
            };
            return azureMetricConfiguration;
        }
    }
}