using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;
using MetricDefinition = Promitor.Core.Scraping.Configuration.Model.Metrics.MetricDefinition;

namespace Promitor.Scraper.Tests.Unit.Serialization.MetricsDeclaration
{
    [Category("Unit")]
    public class MetricsDeclarationWithRedisCacheYamlSerializationTests : YamlSerializationTests<RedisCacheMetricDefinition>
    {
        [Theory]
        [InlineData("promitor1", @"* */1 * * * *", @"* */2 * * * *")]
        [InlineData(null, null, null)]
        public void YamlSerialization_SerializeAndDeserializeConfigForRedisCache_SucceedsWithIdenticalOutput(string resourceGroupName, string defaultScrapingInterval, string metricScrapingInterval)
        {
            // Arrange
            var azureMetadata = GenerateBogusAzureMetadata();
            var redisCacheMetricDefinition = GenerateBogusRedisCacheMetricDefinition(resourceGroupName, metricScrapingInterval);
            var metricDefaults = GenerateBogusMetricDefaults(defaultScrapingInterval);
            var scrapingConfiguration = new Core.Scraping.Configuration.Model.MetricsDeclaration()
            {
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinition>()
                {
                    redisCacheMetricDefinition
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
            AssertMetricDefinition(deserializedMetricDefinition, redisCacheMetricDefinition);
            var deserializedServiceBusMetricDefinition = deserializedMetricDefinition as RedisCacheMetricDefinition;
            AssertRedisCacheMetricDefinition(deserializedServiceBusMetricDefinition, redisCacheMetricDefinition);
        }

        private static void AssertRedisCacheMetricDefinition(RedisCacheMetricDefinition deserializedServiceBusMetricDefinition, RedisCacheMetricDefinition redisCacheMetricDefinition)
        {
            Assert.NotNull(deserializedServiceBusMetricDefinition);
            Assert.Equal(redisCacheMetricDefinition.CacheName, deserializedServiceBusMetricDefinition.CacheName);
        }

        private RedisCacheMetricDefinition GenerateBogusRedisCacheMetricDefinition(string resourceGroupName, string metricScrapingInterval)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<RedisCacheMetricDefinition>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Lorem.Word())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => Core.Scraping.Configuration.Model.ResourceType.RedisCache)
                .RuleFor(metricDefinition => metricDefinition.CacheName, faker => faker.Lorem.Word())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .RuleFor(metricDefinition => metricDefinition.Scraping, faker => bogusScrapingInterval)
                .Ignore(metricDefinition => metricDefinition.ResourceGroupName);

            return bogusGenerator.Generate();
        }
    }
}
