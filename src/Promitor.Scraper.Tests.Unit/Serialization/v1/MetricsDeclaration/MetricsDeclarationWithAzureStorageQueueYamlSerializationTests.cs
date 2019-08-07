using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Xunit;
using MetricDefinition = Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.MetricsDeclaration
{
    [Category("Unit")]
    public class MetricsDeclarationWithAzureStorageQueueYamlSerializationTests : YamlSerializationTests<StorageQueueMetricDefinition>
    {
        [Theory]
        [InlineData("promitor1", @"* */1 * * * *", @"* */2 * * * *", "XYZ", null)]
        [InlineData(null, null, null, null, "SAMPLE_SECRET")]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForAzureStorageQueue_SucceedsWithIdenticalOutput(string resourceGroupName, string defaultScrapingInterval, string metricScrapingInterval, string sasTokenRawValue, string sasTokenEnvironmentVariable)
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var azureStorageQueueMetricDefinition = GenerateBogusAzureStorageQueueMetricDefinition(resourceGroupName, metricScrapingInterval, sasTokenRawValue, sasTokenEnvironmentVariable);
            var metricDefaults = GenerateBogusMetricDefaults(defaultScrapingInterval);
            var scrapingConfiguration = new Core.Scraping.Configuration.Serialization.v1.Model.MetricsDeclarationBuilder
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadataBuilder = azureMetadata,
                MetricDefaultsBuilder = metricDefaults,
                Metrics = new List<MetricDefinitionBuilder>
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
            AssertAzureMetadata(deserializedConfiguration, azureMetadata.Build());
            AssertMetricDefaults(deserializedConfiguration, metricDefaults.Build());
            Assert.NotNull(deserializedConfiguration.Metrics);
            Assert.Single(deserializedConfiguration.Metrics);
            var deserializedMetricDefinition = deserializedConfiguration.Metrics.FirstOrDefault();
            AssertMetricDefinition(deserializedMetricDefinition, azureStorageQueueMetricDefinition.Build<StorageQueueMetricDefinition>());
            var deserializedAzureStorageQueueMetricDefinition = deserializedMetricDefinition as StorageQueueMetricDefinition;
            AssertAzureStorageQueueMetricDefinition(deserializedAzureStorageQueueMetricDefinition, azureStorageQueueMetricDefinition.Build<StorageQueueMetricDefinition>(), deserializedMetricDefinition);
        }

        private static void AssertAzureStorageQueueMetricDefinition(StorageQueueMetricDefinition deserializedStorageQueueMetricDefinition, StorageQueueMetricDefinition storageQueueMetricDefinition, MetricDefinition deserializedMetricDefinition)
        {
            Assert.NotNull(deserializedStorageQueueMetricDefinition);
            Assert.Equal(storageQueueMetricDefinition.AccountName, deserializedStorageQueueMetricDefinition.AccountName);
            Assert.Equal(storageQueueMetricDefinition.QueueName, deserializedStorageQueueMetricDefinition.QueueName);
            Assert.NotNull(deserializedStorageQueueMetricDefinition.SasToken);
            Assert.Equal(storageQueueMetricDefinition.SasToken.RawValue, deserializedStorageQueueMetricDefinition.SasToken.RawValue);
            Assert.Equal(storageQueueMetricDefinition.SasToken.EnvironmentVariable, deserializedStorageQueueMetricDefinition.SasToken.EnvironmentVariable);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(storageQueueMetricDefinition.AzureMetricConfiguration.MetricName, deserializedMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration.Aggregation);
            Assert.Equal(storageQueueMetricDefinition.AzureMetricConfiguration.Aggregation.Type, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(storageQueueMetricDefinition.AzureMetricConfiguration.Aggregation.Interval, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Interval);
        }

        private StorageQueueMetricDefinitionBuilder GenerateBogusAzureStorageQueueMetricDefinition(string resourceGroupName, string metricScrapingInterval, string sasTokenRawValue, string sasTokenEnvironmentVariable)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<StorageQueueMetricDefinitionBuilder>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.StorageQueue)
                .RuleFor(metricDefinition => metricDefinition.AccountName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.QueueName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.SasToken, faker => new SecretBuilder
                {
                    RawValue = sasTokenRawValue,
                    EnvironmentVariable = sasTokenEnvironmentVariable
                })
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfigurationBuilder, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .RuleFor(metricDefinition => metricDefinition.ScrapingBuilder, faker => bogusScrapingInterval)
                .RuleFor(metricDefinition => metricDefinition.Labels, faker => new Dictionary<string, string> { { faker.Name.FirstName(), faker.Random.Guid().ToString() } });

            return bogusGenerator.Generate();
        }
    }
}