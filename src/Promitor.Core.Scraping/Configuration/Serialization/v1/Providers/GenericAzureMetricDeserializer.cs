﻿using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class GenericAzureMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Generic Azure metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to query an arbitrary Azure resource</param>
        /// <returns>A new <see cref="MetricDefinition" /> object (strongly typed as a <see cref="GenericAzureMetricDefinition" />) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<GenericAzureMetricDefinition>(metricNode);

            if (metricNode.Children.TryGetValue(new YamlScalarNode(value: "filter"), out var filterNode))
            {
                metricDefinition.Filter = filterNode?.ToString();
            }
            if (metricNode.Children.TryGetValue(new YamlScalarNode("resourceGroupName"), out var resourceGroupName))
            {
                metricDefinition.ResourceGroupName = resourceGroupName?.ToString();
            }

            var resourceUri = metricNode.Children[new YamlScalarNode(value: "resourceUri")];

            metricDefinition.ResourceUri = resourceUri?.ToString();

            return metricDefinition;
        }
    }
}