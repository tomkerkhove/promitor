using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Xunit;
using MetricDefinition = Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.MetricsDeclaration
{
    [Category("Unit")]
    public class MetricsDeclarationWithContainerInstanceYamlSerializationTests : YamlSerializationTests<ContainerInstanceMetricDefinition>
    {
        [Theory]
        [InlineData("promitor1", @"* */1 * * * *", @"* */2 * * * *")]
        [InlineData(null, null, null)]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForContainerInstance_SucceedsWithIdenticalOutput(string resourceGroupName, string defaultScrapingInterval, string metricScrapingInterval)
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var containerInstanceMetricDefinition = GenerateBogusContainerInstanceMetricDefinition(resourceGroupName, metricScrapingInterval);
            var metricDefaults = GenerateBogusMetricDefaults(defaultScrapingInterval);
            var scrapingConfiguration = new MetricsDeclarationBuilder
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadataBuilder = azureMetadata,
                MetricDefaultsBuilder = metricDefaults,
                Metrics = new List<MetricDefinitionBuilder>
                {
                    containerInstanceMetricDefinition
                }
            };
            var configurationSerializer = new ConfigurationSerializer(NullLogger.Instance);

            // Act
            var serializedConfiguration = configurationSerializer.Serialize(scrapingConfiguration);
            var deserializedConfiguration = configurationSerializer.Deserialize(serializedConfiguration);

            // Assert
            Assert.NotNull(deserializedConfiguration);
            AssertAzureMetadata(deserializedConfiguration, azureMetadata.Build());
            AssertMetricDefaults(deserializedConfiguration, metricDefaults.Build());
            Assert.NotNull(deserializedConfiguration.Metrics);
            Assert.Single(deserializedConfiguration.Metrics);
            var deserializedMetricDefinition = deserializedConfiguration.Metrics.FirstOrDefault();
            AssertMetricDefinition(deserializedMetricDefinition, containerInstanceMetricDefinition.Build<ContainerInstanceMetricDefinition>());
            var deserializedServiceBusMetricDefinition = deserializedMetricDefinition as ContainerInstanceMetricDefinition;
            AssertContainerInstanceMetricDefinition(deserializedServiceBusMetricDefinition, containerInstanceMetricDefinition.Build<ContainerInstanceMetricDefinition>(), deserializedMetricDefinition);
        }

        private static void AssertContainerInstanceMetricDefinition(ContainerInstanceMetricDefinition deserializedContainerInstanceMetricDefinition, ContainerInstanceMetricDefinition containerInstanceMetricDefinition, MetricDefinition deserializedMetricDefinition)
        {
            Assert.NotNull(deserializedContainerInstanceMetricDefinition);
            Assert.Equal(containerInstanceMetricDefinition.ContainerGroup, deserializedContainerInstanceMetricDefinition.ContainerGroup);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(containerInstanceMetricDefinition.AzureMetricConfiguration.MetricName, deserializedMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration.Aggregation);
            Assert.Equal(containerInstanceMetricDefinition.AzureMetricConfiguration.Aggregation.Type, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(containerInstanceMetricDefinition.AzureMetricConfiguration.Aggregation.Interval, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Interval);
        }
        private ContainerInstanceMetricDefinitionBuilder GenerateBogusContainerInstanceMetricDefinition(string resourceGroupName, string metricScrapingInterval)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<ContainerInstanceMetricDefinitionBuilder>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.ContainerInstance)
                .RuleFor(metricDefinition => metricDefinition.ContainerGroup, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfigurationBuilder, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .RuleFor(metricDefinition => metricDefinition.ScrapingBuilder, faker => bogusScrapingInterval)
                .RuleFor(metricDefinition => metricDefinition.Labels, faker => new Dictionary<string, string> { { faker.Name.FirstName(), faker.Random.Guid().ToString() } });

            return bogusGenerator.Generate();
        }
    }
}