using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDefinitionDeserializer : Deserializer<MetricDefinitionV1>
    {
        private const string ResourcesTag = "resources";
        private const string ResourceCollectionsTag = "resourceCollections";
        private readonly IDeserializer<AzureResourceCollectionDefinitionV1> _azureResourceCollectionDeserializer;
        private readonly IAzureResourceDeserializerFactory _azureResourceDeserializerFactory;

        public MetricDefinitionDeserializer(IDeserializer<AzureMetricConfigurationV1> azureMetricConfigurationDeserializer,
            IDeserializer<ScrapingV1> scrapingDeserializer,
            IDeserializer<AzureResourceCollectionDefinitionV1> azureResourceCollectionDeserializer,
            IAzureResourceDeserializerFactory azureResourceDeserializerFactory,
            ILogger<MetricDefinitionDeserializer> logger) : base(logger)
        {
            _azureResourceCollectionDeserializer = azureResourceCollectionDeserializer;
            _azureResourceDeserializerFactory = azureResourceDeserializerFactory;

            Map(definition => definition.Name)
                .IsRequired();
            Map(definition => definition.Description)
                .IsRequired();
            Map(definition => definition.ResourceType)
                .IsRequired();
            Map(definition => definition.AzureMetricConfiguration)
                .IsRequired()
                .MapUsingDeserializer(azureMetricConfigurationDeserializer);
            Map(definition => definition.Labels);
            Map(definition => definition.Scraping)
                .MapUsingDeserializer(scrapingDeserializer);
            IgnoreField(ResourceCollectionsTag);
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

            if (node.Children.TryGetValue(ResourceCollectionsTag, out var resourceCollectionNode))
            {
                metricDefinition.ResourceCollections = _azureResourceCollectionDeserializer.Deserialize((YamlSequenceNode)resourceCollectionNode, errorReporter);
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
        }
    }
}
