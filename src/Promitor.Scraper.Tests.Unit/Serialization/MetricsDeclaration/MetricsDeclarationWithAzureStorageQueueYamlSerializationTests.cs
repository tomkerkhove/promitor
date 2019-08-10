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
using Xunit;
using MetricDefinition = Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition;

namespace Promitor.Scraper.Tests.Unit.Serialization.MetricsDeclaration
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
            var scrapingConfiguration = new Core.Scraping.Configuration.Model.MetricsDeclaration
            {
                Version = SpecVersion.v1.ToString(),
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
            AssertAzureStorageQueueMetricDefinition(deserializedAzureStorageQueueMetricDefinition, azureStorageQueueMetricDefinition, deserializedMetricDefinition);
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

        private StorageQueueMetricDefinition GenerateBogusAzureStorageQueueMetricDefinition(string resourceGroupName, string metricScrapingInterval, string sasTokenRawValue, string sasTokenEnvironmentVariable)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<StorageQueueMetricDefinition>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.StorageQueue)
                .RuleFor(metricDefinition => metricDefinition.AccountName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.QueueName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.SasToken, faker => new Secret
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