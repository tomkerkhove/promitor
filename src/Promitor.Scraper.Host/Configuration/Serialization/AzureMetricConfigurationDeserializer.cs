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
        private readonly YamlScalarNode _dataGranularityNode = new YamlScalarNode("dataGranularity");
        private readonly YamlScalarNode _specifiedGranularityNode = new YamlScalarNode("specifiedGranularity");
        private readonly YamlScalarNode _metricNameNode = new YamlScalarNode("metricName");
        private readonly YamlScalarNode _aggregationNode = new YamlScalarNode("aggregation"); 
        
        internal override AzureMetricConfiguration Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node)); 
            
            var metricName = node.Children[_metricNameNode];
            AggregationType aggregationType = AggregationType.None;
            DataGranularity dataGranularityType = DataGranularity.NotSpecified;
            TimeSpan granularity = TimeSpan.Zero;
            DataGranularityDescriptor dataGranularity = DataGranularityDescriptor.Default;

            if (node.Children.ContainsKey(_aggregationNode))
            {
                YamlNode rawAggregation = node.Children[_aggregationNode];
                if (!Enum.TryParse(rawAggregation?.ToString(), out aggregationType))
                {
                    throw new Exception("Could not deserialize configuration, unrecognized aggregation type");
                }
            }

            if (node.Children.ContainsKey(_dataGranularityNode))
            {
                YamlNode rawGranularityType = node.Children[_dataGranularityNode];
                if (!Enum.TryParse(rawGranularityType?.ToString(), out dataGranularityType))
                {
                    throw new Exception("Could not deserialize configuration, unrecognized data granularity type");
                }

                if (dataGranularityType == DataGranularity.Specified)
                {
                    if (node.Children.ContainsKey(_specifiedGranularityNode))
                    {
                        YamlNode specifiedGranularityType = node.Children[_specifiedGranularityNode];
                        granularity = TimeSpan.Parse(specifiedGranularityType?.ToString());
                    }

                    dataGranularity = new DataGranularityDescriptor { DataGranularity = dataGranularityType, SpecifiedTimeSpan = granularity };
                }
                else if (dataGranularityType == DataGranularity.Lowest)
                {
                    dataGranularity = new DataGranularityDescriptor { DataGranularity = dataGranularityType, SpecifiedTimeSpan = TimeSpan.Zero };
                }
                else
                {
                    throw new Exception("Could not deserialize configuration, unrecognized data granularity type");
                }
            }

            var azureMetricConfiguration = new AzureMetricConfiguration
            {
                MetricName = metricName?.ToString(),
                Aggregation = aggregationType,
                DataGranularity = dataGranularity
            };
            return azureMetricConfiguration;
        }
    }
}