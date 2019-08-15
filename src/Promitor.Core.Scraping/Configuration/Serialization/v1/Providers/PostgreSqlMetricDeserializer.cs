﻿using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Defines a deserializer for the PostgreSQL Server resource type
    /// </summary>
    internal class PostgreSqlMetricDeserializer : MetricDeserializer
    {
        /// <summary>
        /// Deserializes the specified PostgreSQL Server metric node from the YAML configuration file.
        /// </summary>
        /// <param name="metricNode">The metric node to deserialize to PostgreSQL Server configuration</param>
        /// <returns>A new <see cref="MetricDefinitionV1"/> object (strongly typed as a <see cref="PostgreSqlMetricDefinitionV1"/>) </returns>
        internal override MetricDefinitionV1 Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<PostgreSqlMetricDefinitionV1>(metricNode);

            var serverName = metricNode.Children[new YamlScalarNode("serverName")];
            metricDefinition.ServerName = serverName?.ToString();

            return metricDefinition;
        }
    }
}
