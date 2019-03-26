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
    public class MetricsDeclarationWithVirtualMachineYamlSerializationTests : YamlSerializationTests<VirtualMachineMetricDefinition>
    {
        [Theory]
        [InlineData("promitor1")]
        [InlineData(null)]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForVirtualMachine_SucceedsWithIdenticalOutput(string resourceGroupName)
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var virtualMachineMetricDefinition = GenerateBogusVirtualMachineMetricDefinition(resourceGroupName);
            var metricDefaults = GenerateBogusMetricDefaults();
            var scrapingConfiguration = new Core.Scraping.Configuration.Model.MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinition>
                {
                    virtualMachineMetricDefinition
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
            AssertMetricDefinition(deserializedMetricDefinition, virtualMachineMetricDefinition);
            var deserializedVirtualMachineMetricDefinition = deserializedMetricDefinition as VirtualMachineMetricDefinition;
            AssertVirtualMachineMetricDefinition(deserializedVirtualMachineMetricDefinition, virtualMachineMetricDefinition);
        }

        private static void AssertVirtualMachineMetricDefinition(VirtualMachineMetricDefinition deserializedVirtualMachineMetricDefinition, VirtualMachineMetricDefinition virtualMachineMetricDefinition)
        {
            Assert.NotNull(deserializedVirtualMachineMetricDefinition);
            Assert.Equal(virtualMachineMetricDefinition.VirtualMachineName, deserializedVirtualMachineMetricDefinition.VirtualMachineName);
        }
        private VirtualMachineMetricDefinition GenerateBogusVirtualMachineMetricDefinition(string resourceGroupName)
        {
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();
            var bogusGenerator = new Faker<VirtualMachineMetricDefinition>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.VirtualMachine)
                .RuleFor(metricDefinition => metricDefinition.VirtualMachineName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration)
                .Ignore(metricDefinition => metricDefinition.ResourceGroupName);

            return bogusGenerator.Generate();
        }
    }
}