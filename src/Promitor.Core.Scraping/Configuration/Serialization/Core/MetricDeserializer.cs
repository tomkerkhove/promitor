using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Core
{
    internal abstract class MetricDeserializer
    {
        protected ILogger Logger { get; private set; }

        internal MetricDeserializer WithLogger(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
            return this;
        }

        internal abstract MetricDefinition Deserialize(YamlMappingNode metricNode);

        protected virtual TMetricDefinition Deserialize<TMetricDefinition>(YamlMappingNode metricNode)
            where TMetricDefinition : MetricDefinition, new() => DeserializeInternal<TMetricDefinition>(metricNode);

        protected virtual TMetricDefinition DeserializeInternal<TMetricDefinition>(YamlMappingNode metricNode)
            where TMetricDefinition : MetricDefinition, new()
        {
            Guard.NotNull(metricNode, nameof(metricNode));

            var name = metricNode.Children[new YamlScalarNode("name")];
            var description = metricNode.Children[new YamlScalarNode("description")];
            var azureMetricConfigurationNode = (YamlMappingNode)metricNode.Children[new YamlScalarNode("azureMetricConfiguration")];

            var azureMetricConfigurationDeserializer = new AzureMetricConfigurationDeserializer(Logger);
            var azureMetricConfiguration = azureMetricConfigurationDeserializer.Deserialize(azureMetricConfigurationNode);

            var metricDefinition = new TMetricDefinition
            {
                Name = name?.ToString(),
                Description = description?.ToString(),
                AzureMetricConfiguration = azureMetricConfiguration
            };

            return metricDefinition;
        }
    }
}
