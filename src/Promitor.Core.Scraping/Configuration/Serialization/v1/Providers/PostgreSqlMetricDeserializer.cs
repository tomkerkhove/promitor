﻿using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Defines a deserializer for the PostgreSQL Server resource type
    /// </summary>
    internal class PostgreSqlMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>
        /// Deserializes the specified PostgreSQL Server metric node from the YAML configuration file.
        /// </summary>
        /// <param name="metricNode">The metric node to deserialize to PostgreSQL Server configuration</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="PostgreSqlMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<PostgreSqlMetricDefinition>(metricNode);

            var serverName = metricNode.Children[new YamlScalarNode("serverName")];
            metricDefinition.ServerName = serverName?.ToString();

            return metricDefinition;
        }
    }
}
