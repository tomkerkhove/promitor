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
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.MetricsDeclaration
{
    [Category("Unit")]
    public class MetricsDeclarationWithRedisCacheYamlSerializationTests : YamlSerializationTests
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
            var scrapingConfiguration = new MetricsDeclarationV1
            {
                Version = SpecVersion.v1.ToString(),
                AzureMetadata = azureMetadata,
                MetricDefaults = metricDefaults,
                Metrics = new List<MetricDefinitionV1>
                {
                    redisCacheMetricDefinition
                }
            };
            var configurationSerializer = new ConfigurationSerializer(NullLogger.Instance, Mapper, V2Deserializer.Object);

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
            AssertRedisCacheMetricDefinition(deserializedMetricDefinition, redisCacheMetricDefinition);
        }

        private static void AssertRedisCacheMetricDefinition(MetricDefinition deserializedRedisCacheMetricDefinition, RedisCacheMetricDefinitionV1 redisCacheMetricDefinition)
        {
            var deserializedResource = deserializedRedisCacheMetricDefinition.Resources.Single() as RedisCacheResourceDefinition;

            Assert.NotNull(deserializedResource);
            Assert.Equal(redisCacheMetricDefinition.CacheName, deserializedResource.CacheName);
        }

        private RedisCacheMetricDefinitionV1 GenerateBogusRedisCacheMetricDefinition(string resourceGroupName, string metricScrapingInterval)
        {
            var bogusScrapingInterval = GenerateBogusScrapingInterval(metricScrapingInterval);
            var bogusAzureMetricConfiguration = GenerateBogusAzureMetricConfiguration();

            var bogusGenerator = new Faker<RedisCacheMetricDefinitionV1>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.Name, faker => faker.Lorem.Word())
                .RuleFor(metricDefinition => metricDefinition.Description, faker => faker.Lorem.Sentence(wordCount: 6))
                .RuleFor(metricDefinition => metricDefinition.ResourceType, faker => ResourceType.RedisCache)
                .RuleFor(metricDefinition => metricDefinition.CacheName, faker => faker.Lorem.Word())
                .RuleFor(metricDefinition => metricDefinition.AzureMetricConfiguration, faker => bogusAzureMetricConfiguration)
                .RuleFor(metricDefinition => metricDefinition.ResourceGroupName, faker => resourceGroupName)
                .RuleFor(metricDefinition => metricDefinition.Scraping, faker => bogusScrapingInterval)
                .RuleFor(metricDefinition => metricDefinition.Labels, faker => new Dictionary<string, string> { { faker.Name.FirstName(), faker.Random.Guid().ToString() } });

            return bogusGenerator.Generate();
        }
    }
}
