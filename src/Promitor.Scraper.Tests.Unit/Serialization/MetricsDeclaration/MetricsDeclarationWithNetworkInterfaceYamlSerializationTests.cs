using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.Core;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.MetricsDeclaration
{
    [Category(category: "Unit")]
    public class MetricsDeclarationWithNetworkInterfaceYamlSerializationTests : YamlSerializationTests<NetworkInterfaceMetricDefinition>
    {
        [Theory]
        [InlineData("promitor1")]
        [InlineData(data: null)]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForNetworkInterface_SucceedsWithIdenticalOutput(string resourceGroupName)
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var networkInterfaceMetricDefinition = GenerateBogusNetworkInterfaceMetricDefinition(resourceGroupName);
            var metricDefaults = GenerateBogusMetricDefaults();
            var scrapingConfiguration = new Core.Scraping.Configuration.Model.MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinition>
                {
                    networkInterfaceMetricDefinition
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
            AssertMetricDefinition(deserializedMetricDefinition, networkInterfaceMetricDefinition);
            var deserializedNetworkInterfaceMetricDefinition = deserializedMetricDefinition as NetworkInterfaceMetricDefinition;
            AssertNetworkInterfaceMetricDefinition(deserializedNetworkInterfaceMetricDefinition, networkInterfaceMetricDefinition);
        }

        private static void AssertNetworkInterfaceMetricDefinition(NetworkInterfaceMetricDefinition deserializedNetworkInterfaceMetricDefinition, NetworkInterfaceMetricDefinition networkInterfaceMetricDefinition)
        {
            Assert.NotNull(deserializedNetworkInterfaceMetricDefinition);
            Assert.Equal(networkInterfaceMetricDefinition.NetworkInterfaceName, deserializedNetworkInterfaceMetricDefinition.NetworkInterfaceName);
        }

        private NetworkInterfaceMetricDefinition GenerateBogusNetworkInterfaceMetricDefinition(string resourceGroupName)
        {
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();
            Faker<NetworkInterfaceMetricDefinition> bogusGenerator = new Faker<NetworkInterfaceMetricDefinition>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.NetworkInterface)
                .RuleFor(metricDefinition => metricDefinition.NetworkInterfaceName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .Ignore(metricDefinition => metricDefinition.ResourceGroupName);

            return bogusGenerator.Generate();
        }
    }
}