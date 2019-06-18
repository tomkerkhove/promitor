using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    /// <summary>
    /// Defines a deserializer for the Redis Cache resource type
    /// </summary>
    internal class RedisCacheMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>
        /// Deserializes the specified Redis Cache metric node from the YAML configuration file.
        /// </summary>
        /// <param name="metricNode">The metric node to deserialize to Redis Cache configuration</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="RedisCacheMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<RedisCacheMetricDefinition>(metricNode);

            var cacheName = metricNode.Children[new YamlScalarNode("cacheName")];
            metricDefinition.CacheName = cacheName?.ToString();

            return metricDefinition;
        }
    }
}
