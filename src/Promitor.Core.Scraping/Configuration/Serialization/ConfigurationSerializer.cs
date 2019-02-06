using System.Collections.Generic;
using System.IO;
using System.Linq;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Serialization.Yaml;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public class ConfigurationSerializer
    {
        private readonly ILogger logger;
        public ConfigurationSerializer(ILogger logger)
        {
            this.logger = logger;
        }

        public MetricsDeclaration Deserialize(string rawMetricsDeclaration)
        {
            Guard.NotNullOrWhitespace(rawMetricsDeclaration, nameof(rawMetricsDeclaration));

            var input = new StringReader(rawMetricsDeclaration);
            var metricsDeclarationYamlStream = new YamlStream();
            metricsDeclarationYamlStream.Load(input);

            var metricsDeclaration = InterpretYamlStream(metricsDeclarationYamlStream);

            return metricsDeclaration;
        }

        private MetricsDeclaration InterpretYamlStream(YamlStream metricsDeclarationYamlStream)
        {
            var document = metricsDeclarationYamlStream.Documents.First();
            var rootNode = (YamlMappingNode) document.RootNode;

            AzureMetadata azureMetadata = null;
            if (rootNode.Children.ContainsKey("azureMetadata"))
            {
                var azureMetadataNode = (YamlMappingNode) rootNode.Children[new YamlScalarNode("azureMetadata")];
                var azureMetadataSerializer = new AzureMetadataDeserializer(logger);
                azureMetadata = azureMetadataSerializer.Deserialize(azureMetadataNode);
            }

            MetricDefaults metricDefaults = null;
            if (rootNode.Children.ContainsKey("metricDefaults"))
            {
                var metricDefaultsNode = (YamlMappingNode) rootNode.Children[new YamlScalarNode("metricDefaults")];
                var metricDefaultsSerializer = new MetricDefaultsDeserializer(logger);
                metricDefaults = metricDefaultsSerializer.Deserialize(metricDefaultsNode);
            }

            List<MetricDefinition> metrics = null;
            if (rootNode.Children.ContainsKey("metrics"))
            {
                var metricsNode = (YamlSequenceNode) rootNode.Children[new YamlScalarNode("metrics")];
                var metricsDeserializer = new MetricsDeserializer(logger);
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

        public string Serialize(MetricsDeclaration metricsDeclaration)
        {
            Guard.NotNull(metricsDeclaration, nameof(metricsDeclaration));

            var serializer = YamlSerialization.CreateSerializer();
            var rawMetricsDeclaration = serializer.Serialize(metricsDeclaration);
            return rawMetricsDeclaration;
        }
    }
}