using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class MetricDefinitionDeserializer : Deserializer<MetricDefinitionV2>
    {
        private const string NameTag = "name";
        private const string DescriptionTag = "description";
        private const string ResourceTypeTag = "resourceType";
        private const string LabelsTag = "labels";
        private const string AzureMetricConfigurationTag = "azureMetricConfiguration";
        private const string ScrapingTag = "scraping";
        private const string MetricsTag = "metrics";

        private readonly IDeserializer<AzureMetricConfigurationV2> _azureMetricConfigurationDeserializer;
        private readonly IDeserializer<ScrapingV2> _scrapingDeserializer;
        private readonly IAzureResourceDeserializerFactory _azureResourceDeserializerFactory;

        public MetricDefinitionDeserializer(IDeserializer<AzureMetricConfigurationV2> azureMetricConfigurationDeserializer,
            IDeserializer<ScrapingV2> scrapingDeserializer,
            IAzureResourceDeserializerFactory azureResourceDeserializerFactory,
            ILogger logger) : base(logger)
        {
            _azureMetricConfigurationDeserializer = azureMetricConfigurationDeserializer;
            _scrapingDeserializer = scrapingDeserializer;
            _azureResourceDeserializerFactory = azureResourceDeserializerFactory;
        }

        public override MetricDefinitionV2 Deserialize(YamlMappingNode node)
        {
            var metricDefinition = new MetricDefinitionV2
            {
                Name = GetString(node, NameTag),
                Description = GetString(node, DescriptionTag),
                ResourceType = GetEnum<ResourceType>(node, ResourceTypeTag),
                Labels = GetDictionary(node, LabelsTag)
            };

            DeserializeAzureMetricConfiguration(node, metricDefinition);
            DeserializeScraping(node, metricDefinition);
            DeserializeMetrics(node, metricDefinition);

            return metricDefinition;
        }

        private void DeserializeAzureMetricConfiguration(YamlMappingNode node, MetricDefinitionV2 metricDefinition)
        {
            if (node.Children.TryGetValue(AzureMetricConfigurationTag, out var configurationNode))
            {
                metricDefinition.AzureMetricConfiguration =
                    _azureMetricConfigurationDeserializer.Deserialize((YamlMappingNode) configurationNode);
            }
        }

        private void DeserializeScraping(YamlMappingNode node, MetricDefinitionV2 metricDefinition)
        {
            if (node.Children.TryGetValue(ScrapingTag, out var scrapingNode))
            {
                metricDefinition.Scraping = _scrapingDeserializer.Deserialize((YamlMappingNode)scrapingNode);
            }
        }

        private void DeserializeMetrics(YamlMappingNode node, MetricDefinitionV2 metricDefinition)
        {
            if (metricDefinition.ResourceType != ResourceType.NotSpecified &&
                node.Children.TryGetValue(MetricsTag, out var metricsNode))
            {
                var resourceDeserializer = _azureResourceDeserializerFactory.GetDeserializerFor(metricDefinition.ResourceType);
                metricDefinition.Resources = resourceDeserializer.Deserialize((YamlSequenceNode)metricsNode);
            }
        }
    }
}
