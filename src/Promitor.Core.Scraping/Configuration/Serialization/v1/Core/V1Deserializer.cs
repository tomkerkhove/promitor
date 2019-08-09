using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class V1Deserializer
    {
        public V1Deserializer(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }

        public MetricsDeclarationV1 Deserialize(YamlMappingNode rootNode)
        {
            AzureMetadataV1 azureMetadata = null;
            if (rootNode.Children.ContainsKey("azureMetadata"))
            {
                var azureMetadataNode = (YamlMappingNode) rootNode.Children[new YamlScalarNode("azureMetadata")];
                var azureMetadataSerializer = new AzureMetadataDeserializer(Logger);
                azureMetadata = azureMetadataSerializer.Deserialize(azureMetadataNode);
            }

            MetricDefaultsV1 metricDefaults = null;
            if (rootNode.Children.ContainsKey("metricDefaults"))
            {
                var metricDefaultsNode = (YamlMappingNode) rootNode.Children[new YamlScalarNode("metricDefaults")];
                var metricDefaultsSerializer = new MetricDefaultsDeserializer(Logger);
                metricDefaults = metricDefaultsSerializer.Deserialize(metricDefaultsNode);
            }

            List<MetricDefinitionV1> metrics = null;
            if (rootNode.Children.ContainsKey("metrics"))
            {
                var metricsNode = (YamlSequenceNode) rootNode.Children[new YamlScalarNode("metrics")];
                var metricsDeserializer = new MetricsDeserializer(Logger);
                metrics = metricsDeserializer.Deserialize(metricsNode);
            }

            var metricsDeclaration = new MetricsDeclarationV1
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = metrics
            };

            return metricsDeclaration;
        }
    }
}