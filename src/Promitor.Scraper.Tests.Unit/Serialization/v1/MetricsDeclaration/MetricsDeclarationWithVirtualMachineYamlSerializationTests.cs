using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.MetricsDeclaration
{
    [Category(category: "Unit")]
    public class MetricsDeclarationWithVirtualMachineYamlSerializationTests : YamlSerializationTests
    {
        [Theory]
        [InlineData("promitor1", @"* */1 * * * *", @"* */2 * * * *")]
        [InlineData(null, null, null)]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForVirtualMachine_SucceedsWithIdenticalOutput(string resourceGroupName, string defaultScrapingInterval, string metricScrapingInterval)
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var virtualMachineMetricDefinition = GenerateBogusVirtualMachineMetricDefinition(resourceGroupName, metricScrapingInterval);
            var metricDefaults = GenerateBogusMetricDefaults(defaultScrapingInterval);
            var scrapingConfiguration = new MetricsDeclarationV1
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinitionV1>
                {
                    virtualMachineMetricDefinition
                }
            };
            var configurationSerializer = new ConfigurationSerializer(NullLogger.Instance, Mapper);

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
            AssertMetricDefinition(deserializedMetricDefinition, virtualMachineMetricDefinition);
            AssertVirtualMachineMetricDefinition(deserializedMetricDefinition, virtualMachineMetricDefinition);
        }

        private static void AssertVirtualMachineMetricDefinition(MetricDefinition deserializedVirtualMachineMetricDefinition, VirtualMachineMetricDefinitionV1 virtualMachineMetricDefinition)
        {
            var deserializedResource = deserializedVirtualMachineMetricDefinition.Resources.Single() as VirtualMachineResourceDefinition;

            Assert.NotNull(deserializedResource);
            Assert.Equal(virtualMachineMetricDefinition.VirtualMachineName, deserializedResource.VirtualMachineName);
            Assert.NotNull(deserializedVirtualMachineMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(virtualMachineMetricDefinition.AzureMetricConfiguration.MetricName, deserializedVirtualMachineMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.NotNull(deserializedVirtualMachineMetricDefinition.AzureMetricConfiguration.Aggregation);
            Assert.Equal(virtualMachineMetricDefinition.AzureMetricConfiguration.Aggregation.Type, deserializedVirtualMachineMetricDefinition.AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(virtualMachineMetricDefinition.AzureMetricConfiguration.Aggregation.Interval, deserializedVirtualMachineMetricDefinition.AzureMetricConfiguration.Aggregation.Interval);
        }

        private VirtualMachineMetricDefinitionV1 GenerateBogusVirtualMachineMetricDefinition(string resourceGroupName, string metricScrapingInterval)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<VirtualMachineMetricDefinitionV1>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.VirtualMachine)
                .RuleFor(metricDefinition => metricDefinition.VirtualMachineName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .RuleFor(metricDefinition => metricDefinition.Scraping, faker => bogusScrapingInterval)
                .RuleFor(metricDefinition => metricDefinition.Labels, faker => new Dictionary<string, string> { { faker.Name.FirstName(), faker.Random.Guid().ToString() } });

            return bogusGenerator.Generate();
        }
    }
}