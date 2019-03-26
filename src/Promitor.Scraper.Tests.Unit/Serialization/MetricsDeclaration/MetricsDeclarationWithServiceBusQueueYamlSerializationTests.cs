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
    public class ServiceBusQueueYamlSerializationTests : YamlSerializationTests
    {
        [Fact]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForServiceBus_SucceedsWithIdenticalOutput()
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var serviceBusMetricDefinition = GenerateBogusServiceBusMetricDefinition();
            var metricDefaults = GenerateBogusMetricDefaults();
            var scrapingConfiguration = new Core.Scraping.Configuration.Model.MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinition>
                {
                    serviceBusMetricDefinition
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
            AssertMetricDefinition(deserializedMetricDefinition, serviceBusMetricDefinition);
            var deserializedServiceBusMetricDefinition = deserializedMetricDefinition as ServiceBusQueueMetricDefinition;
            AssertServiceBusQueueMetricDefinition(deserializedServiceBusMetricDefinition, serviceBusMetricDefinition, deserializedMetricDefinition);
        }

        private static void AssertServiceBusQueueMetricDefinition(ServiceBusQueueMetricDefinition deserializedServiceBusMetricDefinition, ServiceBusQueueMetricDefinition serviceBusMetricDefinition, MetricDefinition deserializedMetricDefinition)
        {
            Assert.NotNull(deserializedServiceBusMetricDefinition);
            Assert.Equal(serviceBusMetricDefinition.Namespace, deserializedServiceBusMetricDefinition.Namespace);
            Assert.Equal(serviceBusMetricDefinition.QueueName, deserializedServiceBusMetricDefinition.QueueName);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(serviceBusMetricDefinition.AzureMetricConfiguration.MetricName, deserializedMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration.Aggregation);
            Assert.Equal(serviceBusMetricDefinition.AzureMetricConfiguration.Aggregation.Type, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(serviceBusMetricDefinition.AzureMetricConfiguration.Aggregation.Interval, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Interval);
        }
        private ServiceBusQueueMetricDefinition GenerateBogusServiceBusMetricDefinition()
        {
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();
            var bogusGenerator = new Faker<ServiceBusQueueMetricDefinition>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.ServiceBusQueue)
                .RuleFor(metricDefinition => metricDefinition.Namespace, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.QueueName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration);

            return bogusGenerator.Generate();
        }
    }
}