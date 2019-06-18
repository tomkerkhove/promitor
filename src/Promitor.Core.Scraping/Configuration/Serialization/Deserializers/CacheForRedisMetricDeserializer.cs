using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    /// <summary>
    /// Defines a deserializer for the Azure Cache for Redis resource type
    /// </summary>
    internal class CacheForRedisMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>
        /// Deserializes the specified Cache for Redis metric node from the YAML configuration file.
        /// </summary>
        /// <param name="metricNode">The metric node to deserialize to Cache for Redis configuration</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="CacheForRedisMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<CacheForRedisMetricDefinition>(metricNode);

            var cacheName = metricNode.Children[new YamlScalarNode("cacheName")];
            metricDefinition.CacheName = cacheName?.ToString();

            return metricDefinition;
        }
    }
}
