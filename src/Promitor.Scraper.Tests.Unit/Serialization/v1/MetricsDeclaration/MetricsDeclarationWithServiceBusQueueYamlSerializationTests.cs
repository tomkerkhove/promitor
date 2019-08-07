using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.MetricsDeclaration
{
    [Category("Unit")]
    public class ServiceBusQueueYamlSerializationTests : YamlSerializationTests<ServiceBusQueueMetricDefinition>
    {
        [Theory]
        [InlineData("promitor1", @"* */1 * * * *", @"* */2 * * * *")]
        [InlineData(null, null, null)]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForServiceBus_SucceedsWithIdenticalOutput(string resourceGroupName, string defaultScrapingInterval, string metricScrapingInterval)
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var serviceBusMetricDefinition = GenerateBogusServiceBusMetricDefinition(resourceGroupName, metricScrapingInterval);
            var metricDefaults = GenerateBogusMetricDefaults(defaultScrapingInterval);
            var scrapingConfiguration = new MetricsDeclarationBuilder()
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadataBuilder = azureMetadata,
                MetricDefaultsBuilder = metricDefaults,
                Metrics = new List<MetricDefinitionBuilder>
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
            AssertAzureMetadata(deserializedConfiguration, azureMetadata.Build());
            AssertMetricDefaults(deserializedConfiguration, metricDefaults.Build());
            Assert.NotNull(deserializedConfiguration.Metrics);
            Assert.Single(deserializedConfiguration.Metrics);
            var deserializedMetricDefinition = deserializedConfiguration.Metrics.FirstOrDefault();
            AssertMetricDefinition(deserializedMetricDefinition, serviceBusMetricDefinition.Build<ServiceBusQueueMetricDefinition>());
            var deserializedServiceBusMetricDefinition = deserializedMetricDefinition as ServiceBusQueueMetricDefinition;
            AssertServiceBusQueueMetricDefinition(deserializedServiceBusMetricDefinition, serviceBusMetricDefinition.Build<ServiceBusQueueMetricDefinition>());
        }

        private static void AssertServiceBusQueueMetricDefinition(ServiceBusQueueMetricDefinition deserializedServiceBusMetricDefinition, ServiceBusQueueMetricDefinition serviceBusMetricDefinition)
        {
            Assert.NotNull(deserializedServiceBusMetricDefinition);
            Assert.Equal(serviceBusMetricDefinition.Namespace, deserializedServiceBusMetricDefinition.Namespace);
            Assert.Equal(serviceBusMetricDefinition.QueueName, deserializedServiceBusMetricDefinition.QueueName);
        }

        private ServiceBusQueueMetricDefinitionBuilder GenerateBogusServiceBusMetricDefinition(string resourceGroupName, string metricScrapingInterval)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<ServiceBusQueueMetricDefinitionBuilder>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.ServiceBusQueue)
                .RuleFor(metricDefinition => metricDefinition.Namespace, faker => faker.Name.LastName())
                .RuleFor(metricDefinition => metricDefinition.QueueName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfigurationBuilder, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .RuleFor(metricDefinition => metricDefinition.ScrapingBuilder, faker => bogusScrapingInterval)
                .RuleFor(metricDefinition => metricDefinition.Labels, faker => new Dictionary<string, string>
                {
                    { faker.Name.FirstName(), faker.Random.Guid().ToString() },
                    { faker.Name.FirstName(), faker.Random.Guid().ToString() }
                });

            return bogusGenerator.Generate();
        }
    }
}