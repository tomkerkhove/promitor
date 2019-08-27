using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class V1Deserializer : Deserializer<MetricsDeclarationV1>
    {
        private readonly IDeserializer<AzureMetadataV1> _azureMetadataDeserializer;
        private readonly IDeserializer<MetricDefaultsV1> _defaultsDeserializer;
        private readonly IDeserializer<MetricDefinitionV1> _metricsDeserializer;

        public V1Deserializer(IDeserializer<AzureMetadataV1> azureMetadataDeserializer,
            IDeserializer<MetricDefaultsV1> defaultsDeserializer,
            IDeserializer<MetricDefinitionV1> metricsDeserializer,
            ILogger logger) : base(logger)
        {
            _azureMetadataDeserializer = azureMetadataDeserializer;
            _defaultsDeserializer = defaultsDeserializer;
            _metricsDeserializer = metricsDeserializer;
        }

        public override MetricsDeclarationV1 Deserialize(YamlMappingNode rootNode)
        {
            ValidateVersion(rootNode);

            var azureMetadata = DeserializeAzureMetadata(rootNode);
            var metricDefaults = DeserializeMetricDefaults(rootNode);
            var metrics = DeserializeMetrics(rootNode);

            return new MetricsDeclarationV1
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = metrics
            };
        }

        private static void ValidateVersion(YamlMappingNode rootNode)
        {
            var versionFound = rootNode.Children.TryGetValue("version", out var versionNode);
            if (!versionFound)
            {
                throw new System.Exception("No 'version' element was found in the metrics config");
            }

            if (versionNode.ToString() != SpecVersion.v1.ToString())
            {
                throw new System.Exception($"A 'version' element with a value of '{SpecVersion.v1}' was expected but the value '{versionNode}' was found");
            }
        }

        private AzureMetadataV1 DeserializeAzureMetadata(YamlMappingNode rootNode)
        {
            if (rootNode.Children.TryGetValue("azureMetadata", out var azureMetadataNode))
            {
                return _azureMetadataDeserializer.Deserialize((YamlMappingNode)azureMetadataNode);
            }

            return null;
        }

        private MetricDefaultsV1 DeserializeMetricDefaults(YamlMappingNode rootNode)
        {
            if (rootNode.Children.TryGetValue("metricDefaults", out var defaultsNode))
            {
                return _defaultsDeserializer.Deserialize((YamlMappingNode)defaultsNode);
            }

            return null;
        }

        private List<MetricDefinitionV1> DeserializeMetrics(YamlMappingNode rootNode)
        {
            if (rootNode.Children.TryGetValue("metrics", out var metricsNode))
            {
                return _metricsDeserializer.Deserialize((YamlSequenceNode)metricsNode);
            }

            return null;
        }
    }
}
