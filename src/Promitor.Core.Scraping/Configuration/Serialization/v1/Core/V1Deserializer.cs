using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Serialization.Enum;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class V1Deserializer : Deserializer<MetricsDeclarationV1>
    {
        private readonly IDeserializer<MetricDefinitionV1> _metricsDeserializer;

        public V1Deserializer(IDeserializer<AzureMetadataV1> azureMetadataDeserializer,
            IDeserializer<MetricDefaultsV1> defaultsDeserializer,
            IDeserializer<MetricDefinitionV1> metricsDeserializer,
            ILogger<V1Deserializer> logger) : base(logger)
        {
            _metricsDeserializer = metricsDeserializer;

            Map(definition => definition.Version)
                .IsRequired()
                .MapUsing(GetVersion);
            Map(definition => definition.AzureMetadata)
                .IsRequired()
                .MapUsingDeserializer(azureMetadataDeserializer);
            Map(definition => definition.MetricDefaults)
                .IsRequired()
                .MapUsingDeserializer(defaultsDeserializer);
            Map(definition => definition.Metrics)
                .IsRequired()
                .MapUsing(DeserializeMetrics);
        }

        private static object GetVersion(string value, KeyValuePair<YamlNode, YamlNode> node, IErrorReporter errorReporter)
        {
            if (value != SpecVersion.v1.ToString())
            {
                errorReporter.ReportError(node.Value, $"A 'version' element with a value of '{SpecVersion.v1}' was expected but the value '{value}' was found");
            }

            return SpecVersion.v1.ToString();
        }

        private IReadOnlyCollection<MetricDefinitionV1> DeserializeMetrics(
            string value, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {
            return _metricsDeserializer.Deserialize((YamlSequenceNode)nodePair.Value, errorReporter);
        }
    }
}
