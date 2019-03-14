using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;
using MetricDefinition = Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition;

namespace Promitor.Scraper.Tests.Unit.Serialization.MetricsDeclaration
{
    [Category("Unit")]
    public class MetricsDeclarationWithAzureQueueYamlSerializationTests : YamlSerializationTests
    {
        [Fact]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForAzureQueue_SucceedsWithIdenticalOutput()
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var azureQueueMetricDefinition = GenerateBogusAzureQueueMetricDefinition();
            var metricDefaults = GenerateBogusMetricDefaults();
            var scrapingConfiguration = new Core.Scraping.Configuration.Model.MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinition>
                {
                    azureQueueMetricDefinition
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
            AssertMetricDefinition(deserializedMetricDefinition, azureQueueMetricDefinition);
            var deserializedAzureQueueMetricDefinition = deserializedMetricDefinition as AzureQueueMetricDefinition;
            AssertAzureQueueMetricDefinition(deserializedAzureQueueMetricDefinition, azureQueueMetricDefinition, deserializedMetricDefinition);
        }

        private static void AssertAzureQueueMetricDefinition(AzureQueueMetricDefinition deserializedServiceBusMetricDefinition, AzureQueueMetricDefinition serviceBusMetricDefinition, MetricDefinition deserializedMetricDefinition)
        {
            Assert.NotNull(deserializedServiceBusMetricDefinition);
            Assert.Equal(serviceBusMetricDefinition.AccountName, deserializedServiceBusMetricDefinition.AccountName);
            Assert.Equal(serviceBusMetricDefinition.QueueName, deserializedServiceBusMetricDefinition.QueueName);
            Assert.Equal(serviceBusMetricDefinition.SasToken, deserializedServiceBusMetricDefinition.SasToken);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(serviceBusMetricDefinition.AzureMetricConfiguration.MetricName, deserializedMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration.Aggregation);
            Assert.Equal(serviceBusMetricDefinition.AzureMetricConfiguration.Aggregation.Type, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(serviceBusMetricDefinition.AzureMetricConfiguration.Aggregation.Interval, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Interval);
        }
        
        private AzureQueueMetricDefinition GenerateBogusAzureQueueMetricDefinition()
        {
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();
            var bogusGenerator = new Faker<AzureQueueMetricDefinition>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.AzureQueue)
                .RuleFor(metricDefinition => metricDefinition.AccountName, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.QueueName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.SasToken, faker => $"?sig={Base64UrlEncoder.Encode(faker.Lorem.Sentence(wordCount: 3))}")
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration);

            return bogusGenerator.Generate();
        }
    }
}