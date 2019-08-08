using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class V2Serializer : Deserializer<MetricsDeclarationV2>
    {
        private readonly IDeserializer<AzureMetadataV2> _azureMetadataDeserializer;
        private readonly IDeserializer<MetricDefaultsV2> _defaultsDeserializer;
        private readonly IDeserializer<MetricDefinitionV2> _metricsDeserializer;

        public V2Serializer(IDeserializer<AzureMetadataV2> azureMetadataDeserializer,
            IDeserializer<MetricDefaultsV2> defaultsDeserializer,
            IDeserializer<MetricDefinitionV2> metricsDeserializer,
            ILogger logger) : base(logger)
        {
            _azureMetadataDeserializer = azureMetadataDeserializer;
            _defaultsDeserializer = defaultsDeserializer;
            _metricsDeserializer = metricsDeserializer;
        }

        public override MetricsDeclarationV2 Deserialize(YamlMappingNode rootNode)
        {
            ValidateVersion(rootNode);

            var azureMetadata = DeserializeAzureMetadata(rootNode);
            var metricDefaults = DeserializeMetricDefaults(rootNode);
            var metrics = DeserializeMetrics(rootNode);

            return new MetricsDeclarationV2
            {
                Version = SpecVersion.v2.ToString(),
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

            if (versionNode.ToString() != SpecVersion.v2.ToString())
            {
                throw new System.Exception($"A 'version' element with a value of '{SpecVersion.v2}' was expected but the value '{versionNode}' was found");
            }
        }

        private AzureMetadataV2 DeserializeAzureMetadata(YamlMappingNode rootNode)
        {
            if (rootNode.Children.TryGetValue("azureMetadata", out var azureMetadataNode))
            {
                return _azureMetadataDeserializer.Deserialize((YamlMappingNode)azureMetadataNode);
            }

            return null;
        }

        private MetricDefaultsV2 DeserializeMetricDefaults(YamlMappingNode rootNode)
        {
            if (rootNode.Children.TryGetValue("metricDefaults", out var defaultsNode))
            {
                return _defaultsDeserializer.Deserialize((YamlMappingNode)defaultsNode);
            }

            return null;
        }

        private List<MetricDefinitionV2> DeserializeMetrics(YamlMappingNode rootNode)
        {
            if (rootNode.Children.TryGetValue("metrics", out var metricsNode))
            {
                return _metricsDeserializer.Deserialize((YamlSequenceNode)metricsNode);
            }

            return null;
        }
    }
}
