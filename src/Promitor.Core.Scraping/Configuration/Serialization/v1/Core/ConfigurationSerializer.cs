using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.Interfaces;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class ConfigurationSerializer : IVersionedConfigurationSerializer
    {
        public ConfigurationSerializer(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }

        public MetricsDeclaration InterpretYamlStream(YamlMappingNode rootNode)
        {
            AzureMetadata azureMetadata = null;
            if (rootNode.Children.ContainsKey("azureMetadata"))
            {
                var azureMetadataNode = (YamlMappingNode) rootNode.Children[new YamlScalarNode("azureMetadata")];
                var azureMetadataSerializer = new AzureMetadataDeserializer(Logger);
                azureMetadata = azureMetadataSerializer.Deserialize(azureMetadataNode);
            }

            MetricDefaults metricDefaults = null;
            if (rootNode.Children.ContainsKey("metricDefaults"))
            {
                var metricDefaultsNode = (YamlMappingNode) rootNode.Children[new YamlScalarNode("metricDefaults")];
                var metricDefaultsSerializer = new MetricDefaultsDeserializer(Logger);
                metricDefaults = metricDefaultsSerializer.Deserialize(metricDefaultsNode);
            }

            List<MetricDefinition> metrics = null;
            if (rootNode.Children.ContainsKey("metrics"))
            {
                var metricsNode = (YamlSequenceNode) rootNode.Children[new YamlScalarNode("metrics")];
                var metricsDeserializer = new MetricsDeserializer(Logger);
                metrics = metricsDeserializer.Deserialize(metricsNode);
            }

            var metricsDeclaration = new MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = metrics
            };

            return metricsDeclaration;
        }
    }
}