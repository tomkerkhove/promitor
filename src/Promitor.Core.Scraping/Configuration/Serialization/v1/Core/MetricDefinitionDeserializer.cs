using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDefinitionDeserializer : Deserializer<MetricDefinitionV1>
    {
        private const string NameTag = "name";
        private const string DescriptionTag = "description";
        private const string ResourceTypeTag = "resourceType";
        private const string LabelsTag = "labels";
        private const string AzureMetricConfigurationTag = "azureMetricConfiguration";
        private const string ScrapingTag = "scraping";
        private const string ResourcesTag = "resources";

        private readonly IDeserializer<AzureMetricConfigurationV1> _azureMetricConfigurationDeserializer;
        private readonly IDeserializer<ScrapingV1> _scrapingDeserializer;
        private readonly IAzureResourceDeserializerFactory _azureResourceDeserializerFactory;

        public MetricDefinitionDeserializer(IDeserializer<AzureMetricConfigurationV1> azureMetricConfigurationDeserializer,
            IDeserializer<ScrapingV1> scrapingDeserializer,
            IAzureResourceDeserializerFactory azureResourceDeserializerFactory,
            ILogger<MetricDefinitionDeserializer> logger) : base(logger)
        {
            _azureMetricConfigurationDeserializer = azureMetricConfigurationDeserializer;
            _scrapingDeserializer = scrapingDeserializer;
            _azureResourceDeserializerFactory = azureResourceDeserializerFactory;
        }

        public override MetricDefinitionV1 Deserialize(YamlMappingNode node)
        {
            var name = node.GetString(NameTag);
            var description = node.GetString(DescriptionTag);
            var resourceType = node.GetEnum<ResourceType>(ResourceTypeTag);
            var labels = node.GetDictionary(LabelsTag);

            var metricDefinition = new MetricDefinitionV1
            {
                Name = name,
                Description = description,
                ResourceType = resourceType,
                Labels = labels
            };

            DeserializeAzureMetricConfiguration(node, metricDefinition);
            DeserializeScraping(node, metricDefinition);
            DeserializeMetrics(node, metricDefinition);

            return metricDefinition;
        }

        private void DeserializeAzureMetricConfiguration(YamlMappingNode node, MetricDefinitionV1 metricDefinition)
        {
            if (node.Children.TryGetValue(AzureMetricConfigurationTag, out var configurationNode))
            {
                metricDefinition.AzureMetricConfiguration =
                    _azureMetricConfigurationDeserializer.Deserialize((YamlMappingNode) configurationNode);
            }
        }

        private void DeserializeScraping(YamlMappingNode node, MetricDefinitionV1 metricDefinition)
        {
            if (node.Children.TryGetValue(ScrapingTag, out var scrapingNode))
            {
                metricDefinition.Scraping = _scrapingDeserializer.Deserialize((YamlMappingNode)scrapingNode);
            }
        }

        private void DeserializeMetrics(YamlMappingNode node, MetricDefinitionV1 metricDefinition)
        {
            if (metricDefinition.ResourceType != null &&
                metricDefinition.ResourceType != ResourceType.NotSpecified &&
                node.Children.TryGetValue(ResourcesTag, out var metricsNode))
            {
                var resourceDeserializer = _azureResourceDeserializerFactory.GetDeserializerFor(metricDefinition.ResourceType.Value);
                metricDefinition.Resources = resourceDeserializer.Deserialize((YamlSequenceNode)metricsNode);
            }
        }
    }
}
