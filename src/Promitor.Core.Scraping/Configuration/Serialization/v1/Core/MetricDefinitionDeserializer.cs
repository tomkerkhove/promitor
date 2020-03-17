using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDefinitionDeserializer : Deserializer<MetricDefinitionV1>
    {
        private const string ResourcesTag = "resources";
        private readonly IAzureResourceDeserializerFactory _azureResourceDeserializerFactory;

        public MetricDefinitionDeserializer(IDeserializer<AzureMetricConfigurationV1> azureMetricConfigurationDeserializer,
            IDeserializer<ScrapingV1> scrapingDeserializer,
            IAzureResourceDeserializerFactory azureResourceDeserializerFactory,
            ILogger<MetricDefinitionDeserializer> logger) : base(logger)
        {
            _azureResourceDeserializerFactory = azureResourceDeserializerFactory;

            MapRequired(definition => definition.Name);
            MapRequired(definition => definition.Description);
            MapRequired(definition => definition.ResourceType);
            MapOptional(definition => definition.Labels);
            MapRequired(definition => definition.AzureMetricConfiguration, azureMetricConfigurationDeserializer);
            MapOptional(definition => definition.Scraping, scrapingDeserializer);
            IgnoreField(ResourcesTag);
        }

        public override MetricDefinitionV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var metricDefinition = base.Deserialize(node, errorReporter);

            DeserializeMetrics(node, metricDefinition, errorReporter);

            return metricDefinition;
        }

        private void DeserializeMetrics(YamlMappingNode node, MetricDefinitionV1 metricDefinition, IErrorReporter errorReporter)
        {
            if (metricDefinition.ResourceType == null)
            {
                return;
            }

            var resourceTypeNode = node.Children["resourceType"];
            if (metricDefinition.ResourceType == ResourceType.NotSpecified)
            {
                errorReporter.ReportError(resourceTypeNode, "'resourceType' must not be set to 'NotSpecified'.");
                return;
            }

            if (node.Children.TryGetValue(ResourcesTag, out var metricsNode))
            {
                var resourceDeserializer = _azureResourceDeserializerFactory.GetDeserializerFor(metricDefinition.ResourceType.Value);
                if (resourceDeserializer != null)
                {
                    metricDefinition.Resources = resourceDeserializer.Deserialize((YamlSequenceNode)metricsNode, errorReporter);
                }
                else
                {
                    errorReporter.ReportError(resourceTypeNode, $"Could not find a deserializer for resource type '{metricDefinition.ResourceType}'.");
                }
            }
            else
            {
                errorReporter.ReportError(node, "'resources' is a required field but was not found.");
            }
        }
    }
}
