using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.Core;
using Xunit;
using MetricDefinition = Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition;

namespace Promitor.Scraper.Tests.Unit.Serialization.MetricsDeclaration
{
    [Category("Unit")]
    public class MetricsDeclarationWithContainerRegistryYamlSerializationTests : YamlSerializationTests<ContainerRegistryMetricDefinition>
    {
        [Theory]
        [InlineData("promitor1", @"01:00", @"2:00")]
        [InlineData(null, null, null)]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForContainerRegistry_SucceedsWithIdenticalOutput(string resourceGroupName, string defaultScrapingInterval, string metricScrapingInterval)
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var containerRegistryMetricDefinition = GenerateBogusContainerRegistryMetricDefinition(resourceGroupName, metricScrapingInterval);
            var metricDefaults = GenerateBogusMetricDefaults(defaultScrapingInterval);
            var scrapingConfiguration = new Core.Scraping.Configuration.Model.MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinition>
                {
                    containerRegistryMetricDefinition
                }
            };
            var configurationSerializer = new ConfigurationSerializer(NullLogger.Instance);

            // Act
            var serializedConfiguration = configurationSerializer.Serialize(scrapingConfiguration);
            var deserializedConfiguration = configurationSerializer.Deserialize(serializedConfiguration);

            // Assert
            Assert.NotNull(deserializedConfiguration);
            AssertAzureMetadata(deserializedConfiguration, azureMetadata);
            AssertMetricDefaults(deserializedConfiguration, metricDefaults);
            Assert.NotNull(deserializedConfiguration.Metrics);
            Assert.Single(deserializedConfiguration.Metrics);
            var deserializedMetricDefinition = deserializedConfiguration.Metrics.FirstOrDefault();
            AssertMetricDefinition(deserializedMetricDefinition, containerRegistryMetricDefinition);
            var deserializedContainerRegistryMetricDefinition = deserializedMetricDefinition as ContainerRegistryMetricDefinition;
            AssertContainerRegistryMetricDefinition(deserializedContainerRegistryMetricDefinition, containerRegistryMetricDefinition);
        }

        private static void AssertContainerRegistryMetricDefinition(ContainerRegistryMetricDefinition deserializedServiceBusMetricDefinition, ContainerRegistryMetricDefinition serviceBusMetricDefinition)
        {
            Assert.NotNull(deserializedServiceBusMetricDefinition);
            Assert.Equal(serviceBusMetricDefinition.RegistryName, deserializedServiceBusMetricDefinition.RegistryName);
        }

        private ContainerRegistryMetricDefinition GenerateBogusContainerRegistryMetricDefinition(string resourceGroupName, string metricScrapingInterval)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<ContainerRegistryMetricDefinition>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.ContainerRegistry)
                .RuleFor(metricDefinition => metricDefinition.RegistryName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .RuleFor(metricDefinition => metricDefinition.Scraping, faker => bogusScrapingInterval)
                .Ignore(metricDefinition => metricDefinition.ResourceGroupName);

            return bogusGenerator.Generate();
        }
    }
}