using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;
using MetricDefinition = Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition;

namespace Promitor.Scraper.Tests.Unit.Serialization.MetricsDeclaration
{
    [Category("Unit")]
    public class MetricsDeclarationWithAzureStorageQueueYamlSerializationTests : YamlSerializationTests<StorageQueueMetricDefinition>
    {
        [Fact]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForAzureStorageQueue_SucceedsWithIdenticalOutput()
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var azureStorageQueueMetricDefinition = GenerateBogusAzureStorageQueueMetricDefinition();
            var metricDefaults = GenerateBogusMetricDefaults();
            var scrapingConfiguration = new Core.Scraping.Configuration.Model.MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinition>
                {
                    azureStorageQueueMetricDefinition
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
            AssertMetricDefinition(deserializedMetricDefinition, azureStorageQueueMetricDefinition);
            var deserializedAzureStorageQueueMetricDefinition = deserializedMetricDefinition as StorageQueueMetricDefinition;
            AssertAzureStorageQueueMetricDefinition(deserializedAzureStorageQueueMetricDefinition, azureStorageQueueMetricDefinition);
        }

        private static void AssertAzureStorageQueueMetricDefinition(StorageQueueMetricDefinition storageQueueMetricDefinition, StorageQueueMetricDefinition serviceBusMetricDefinition)
        {
            Assert.NotNull(storageQueueMetricDefinition);
            Assert.Equal(serviceBusMetricDefinition.AccountName, storageQueueMetricDefinition.AccountName);
            Assert.Equal(serviceBusMetricDefinition.QueueName, storageQueueMetricDefinition.QueueName);
            Assert.Equal(serviceBusMetricDefinition.SasToken, storageQueueMetricDefinition.SasToken);
        }
        
        private StorageQueueMetricDefinition GenerateBogusAzureStorageQueueMetricDefinition()
        {
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();
            var bogusGenerator = new Faker<StorageQueueMetricDefinition>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.StorageQueue)
                .RuleFor(metricDefinition => metricDefinition.AccountName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.QueueName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.SasToken, faker => $"?sig={Base64UrlEncoder.Encode(faker.Lorem.Sentence(wordCount: 3))}")
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration);

            return bogusGenerator.Generate();
        }
    }
}