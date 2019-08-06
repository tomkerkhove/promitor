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
    public class MetricsDeclarationWithAzureStorageQueueYamlSerializationTests : YamlSerializationTests
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
            var scrapingConfiguration = new Core.Scraping.Configuration.Serialization.v1.Model.MetricsDeclarationV1
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinitionV1>
                {
                    azureStorageQueueMetricDefinition
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
            AssertMetricDefinition(deserializedMetricDefinition, azureStorageQueueMetricDefinition);
            AssertAzureStorageQueueMetricDefinition(deserializedMetricDefinition, azureStorageQueueMetricDefinition);
        }

        private static void AssertAzureStorageQueueMetricDefinition(MetricDefinition deserializedStorageQueueMetricDefinition, StorageQueueMetricDefinitionV1 storageQueueMetricDefinition)
        {
            var deserializedResource = deserializedStorageQueueMetricDefinition.Resources.Single() as StorageQueueMetricDefinition;

            Assert.NotNull(deserializedResource);
            Assert.Equal(storageQueueMetricDefinition.AccountName, deserializedResource.AccountName);
            Assert.Equal(storageQueueMetricDefinition.QueueName, deserializedResource.QueueName);
            Assert.NotNull(deserializedResource.SasToken);
            Assert.Equal(storageQueueMetricDefinition.SasToken.RawValue, deserializedResource.SasToken.RawValue);
            Assert.Equal(storageQueueMetricDefinition.SasToken.EnvironmentVariable, deserializedResource.SasToken.EnvironmentVariable);
            Assert.NotNull(deserializedStorageQueueMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(storageQueueMetricDefinition.AzureMetricConfiguration.MetricName, deserializedStorageQueueMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.NotNull(deserializedStorageQueueMetricDefinition.AzureMetricConfiguration.Aggregation);
            Assert.Equal(storageQueueMetricDefinition.AzureMetricConfiguration.Aggregation.Type, deserializedStorageQueueMetricDefinition.AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(storageQueueMetricDefinition.AzureMetricConfiguration.Aggregation.Interval, deserializedStorageQueueMetricDefinition.AzureMetricConfiguration.Aggregation.Interval);
        }

        private StorageQueueMetricDefinitionV1 GenerateBogusAzureStorageQueueMetricDefinition(string resourceGroupName, string metricScrapingInterval, string sasTokenRawValue, string sasTokenEnvironmentVariable)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<StorageQueueMetricDefinitionV1>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.StorageQueue)
                .RuleFor(metricDefinition => metricDefinition.AccountName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.QueueName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.SasToken, faker => new SecretV1
                {
                    RawValue = sasTokenRawValue,
                    EnvironmentVariable = sasTokenEnvironmentVariable
                })
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .RuleFor(metricDefinition => metricDefinition.Scraping, faker => bogusScrapingInterval)
                .RuleFor(metricDefinition => metricDefinition.Labels, faker => new Dictionary<string, string> { { faker.Name.FirstName(), faker.Random.Guid().ToString() } });

            return bogusGenerator.Generate();
        }
    }
}