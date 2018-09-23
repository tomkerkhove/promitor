using System.Collections.Generic;
using System.IO;
using System.Linq;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Serialization;
using GuardNet;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Host.Configuration.Serialization
{
    public class ConfigurationSerializer
    {
        public static MetricsDeclaration Deserialize(string rawMetricsDeclaration)
        {
            Guard.NotNullOrWhitespace(rawMetricsDeclaration, nameof(rawMetricsDeclaration));

            var input = new StringReader(rawMetricsDeclaration);
            var metricsDeclarationYamlStream = new YamlStream();
            metricsDeclarationYamlStream.Load(input);

            var metricsDeclaration = InterpretYamlStream(metricsDeclarationYamlStream);

            return metricsDeclaration;
        }

        private static MetricsDeclaration InterpretYamlStream(YamlStream metricsDeclarationYamlStream)
        {
            var document = metricsDeclarationYamlStream.Documents.First();
            var rootNode = (YamlMappingNode)document.RootNode;

            AzureMetadata azureMetadata = null;
            if (rootNode.Children.ContainsKey("azureMetadata"))
            {
                var azureMetadataNode = (YamlMappingNode)rootNode.Children[new YamlScalarNode("azureMetadata")];
                var azureMetadataSerializer = new AzureMetadataDeserializer();
                azureMetadata = azureMetadataSerializer.Deserialize(azureMetadataNode);
            }

            List<MetricDefinition> metrics = null;
            if (rootNode.Children.ContainsKey("metrics"))
            {
                var metricsNode = (YamlSequenceNode)rootNode.Children[new YamlScalarNode("metrics")];
                var metricsDeserializer = new MetricsDeserializer();
                metrics = metricsDeserializer.Deserialize(metricsNode);
            }

            var metricsDeclaration = new MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                Metrics = metrics
            };

            return metricsDeclaration;
        }

        public static string Serialize(MetricsDeclaration metricsDeclaration)
        {
            Guard.NotNull(metricsDeclaration, nameof(metricsDeclaration));

            var serializer = YamlSerialization.CreateSerializer();
            var rawMetricsDeclaration = serializer.Serialize(metricsDeclaration);
            return rawMetricsDeclaration;
        }
    }
}