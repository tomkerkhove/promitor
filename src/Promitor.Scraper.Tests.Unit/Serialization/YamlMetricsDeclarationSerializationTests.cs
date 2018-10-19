using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Integrations.AzureMonitor;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Configuration.Serialization;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization
{
    [Category("Unit")]
    public class YamlMetricsDeclarationSerializationTests
    {
        [Fact]
        public void YamlSerialization_SerializeAndDeserializeValidConfigForServiceBus_SucceedsWithIdenticalOutput()
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var serviceBusMetricDefinition = GenerateBogusServiceBusMetricDefinition();
            var scrapingConfiguration = new MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                Metrics = new List<Host.Configuration.Model.Metrics.MetricDefinition>
                {
                    serviceBusMetricDefinition
                }
            };

            // Act
            var serializedConfiguration = ConfigurationSerializer.Serialize(scrapingConfiguration);
            var deserializedConfiguration = ConfigurationSerializer.Deserialize(serializedConfiguration);

            // Assert
            Assert.NotNull(deserializedConfiguration);
            Assert.NotNull(deserializedConfiguration.AzureMetadata);
            Assert.Equal(azureMetadata.TenantId, deserializedConfiguration.AzureMetadata.TenantId);
            Assert.Equal(azureMetadata.ResourceGroupName, deserializedConfiguration.AzureMetadata.ResourceGroupName);
            Assert.Equal(azureMetadata.SubscriptionId, deserializedConfiguration.AzureMetadata.SubscriptionId);
            Assert.NotNull(deserializedConfiguration.Metrics);
            Assert.Single(deserializedConfiguration.Metrics);
            var deserializedMetricDefinition = deserializedConfiguration.Metrics.FirstOrDefault();
            Assert.NotNull(deserializedMetricDefinition);
            Assert.Equal(serviceBusMetricDefinition.Name, deserializedMetricDefinition.Name);
            Assert.Equal(serviceBusMetricDefinition.Description, deserializedMetricDefinition.Description);
            Assert.Equal(serviceBusMetricDefinition.ResourceType, deserializedMetricDefinition.ResourceType);
            var deserializedServiceBusMetricDefinition = deserializedMetricDefinition as ServiceBusQueueMetricDefinition;
            Assert.NotNull(deserializedServiceBusMetricDefinition);
            Assert.Equal(serviceBusMetricDefinition.Namespace, deserializedServiceBusMetricDefinition.Namespace);
            Assert.Equal(serviceBusMetricDefinition.QueueName, deserializedServiceBusMetricDefinition.QueueName);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(serviceBusMetricDefinition.AzureMetricConfiguration.MetricName, deserializedMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.Equal(serviceBusMetricDefinition.AzureMetricConfiguration.Aggregation, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation);
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

        private AzureMetricConfiguration GenerateBogusAzureMetricConfiguration()
        {
            var bogusGenerator = new Faker<AzureMetricConfiguration>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.MetricName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Aggregation, faker => faker.PickRandom<AggregationType>())
                .RuleFor(metricDefinition => metricDefinition.MetricType, faker => faker.PickRandom<MetricType>());

            return bogusGenerator.Generate();

        }

        private AzureMetadata GenerateBogusAzureMetadata()
        {
            var bogusGenerator = new Faker<AzureMetadata>()
                            .StrictMode(ensureRulesForAllProperties: true)
                            .RuleFor(metadata => metadata.TenantId, faker => faker.Finance.Account())
                            .RuleFor(metadata => metadata.ResourceGroupName, faker => faker.Name.FirstName())
                            .RuleFor(metadata => metadata.SubscriptionId, faker => faker.Finance.Account());

            return bogusGenerator.Generate();
        }
    }
}