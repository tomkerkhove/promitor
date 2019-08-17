using System;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using Bogus;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.MetricsDeclaration
{
    [Category("Unit")]
    public class YamlSerializationTests
    {
        protected readonly IMapper Mapper;

        public YamlSerializationTests()
        {
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            Mapper = mapperConfiguration.CreateMapper();
        }

        protected void AssertMetricDefinition(MetricDefinition deserializedMetricDefinition, MetricDefinitionV1 metricDefinition)
        {
            Assert.NotNull(deserializedMetricDefinition);
            Assert.NotNull(deserializedMetricDefinition.PrometheusMetricDefinition);
            Assert.Equal(metricDefinition.Name, deserializedMetricDefinition.PrometheusMetricDefinition.Name);
            Assert.Equal(metricDefinition.Description, deserializedMetricDefinition.PrometheusMetricDefinition.Description);
            Assert.Equal(metricDefinition.ResourceType, deserializedMetricDefinition.ResourceType);
            Assert.NotNull(deserializedMetricDefinition.PrometheusMetricDefinition.Labels);
            Assert.Equal(metricDefinition.Labels, deserializedMetricDefinition.PrometheusMetricDefinition.Labels);
            Assert.NotEmpty(deserializedMetricDefinition.Resources);
            Assert.Equal(deserializedMetricDefinition.Resources.Single().ResourceGroupName, metricDefinition.ResourceGroupName);

            foreach (var label in metricDefinition.Labels)
            {
                var deserializedLabel = deserializedMetricDefinition.PrometheusMetricDefinition.Labels[label.Key];
                Assert.NotNull(deserializedLabel);
                Assert.Equal(label.Value, deserializedLabel);
            }

            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(metricDefinition.AzureMetricConfiguration.MetricName, deserializedMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration.Aggregation);
            Assert.Equal(metricDefinition.AzureMetricConfiguration.Aggregation.Type, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(metricDefinition.AzureMetricConfiguration.Aggregation.Interval, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Interval);
        }

        protected void AssertMetricDefaults(Promitor.Core.Scraping.Configuration.Model.MetricsDeclaration deserializedConfiguration, MetricDefaultsV1 metricDefaults)
        {
            var deserializedMetricDefaults = deserializedConfiguration.MetricDefaults;
            Assert.NotNull(deserializedMetricDefaults);
            Assert.NotNull(deserializedMetricDefaults.Aggregation);
            Assert.Equal(metricDefaults.Aggregation.Interval, deserializedMetricDefaults.Aggregation.Interval);
        }

        protected void AssertAzureMetadata(Promitor.Core.Scraping.Configuration.Model.MetricsDeclaration deserializedConfiguration, AzureMetadataV1 azureMetadata)
        {
            Assert.NotNull(deserializedConfiguration.AzureMetadata);
            Assert.Equal(azureMetadata.TenantId, deserializedConfiguration.AzureMetadata.TenantId);
            Assert.Equal(azureMetadata.ResourceGroupName, deserializedConfiguration.AzureMetadata.ResourceGroupName);
            Assert.Equal(azureMetadata.SubscriptionId, deserializedConfiguration.AzureMetadata.SubscriptionId);
        }

        protected AzureMetricConfigurationV1 GenerateBogusAzureMetricConfiguration()
        {
            var bogusMetricAggregation = new Faker<MetricAggregationV1>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(aggregation => aggregation.Type, faker => faker.PickRandom<Microsoft.Azure.Management.Monitor.Fluent.Models.AggregationType>())
                .RuleFor(aggregation => aggregation.Interval, faker => TimeSpan.FromMinutes(faker.Random.Int()))
                .Generate();

            var bogusMetricConfiguration = new Faker<AzureMetricConfigurationV1>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.MetricName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Aggregation, faker => bogusMetricAggregation)
                .Generate();

            return bogusMetricConfiguration;
        }

        protected MetricDefaultsV1 GenerateBogusMetricDefaults(string defaultScrapingInterval)
        {
            var bogusAggregationGenerator = new Faker<AggregationV1>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(aggregation => aggregation.Interval, faker => TimeSpan.FromMinutes(faker.Random.Int()));

            var generatedAggregation = bogusAggregationGenerator.Generate();
            var metricDefaults = new MetricDefaultsV1
            {
                Aggregation = generatedAggregation,
            };

            if (!string.IsNullOrWhiteSpace(defaultScrapingInterval))
            {
                metricDefaults.Scraping.Schedule = defaultScrapingInterval;
            }

            return metricDefaults;
        }

        protected AzureMetadataV1 GenerateBogusAzureMetadata()
        {
            var bogusGenerator = new Faker<AzureMetadataV1>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metadata => metadata.TenantId, faker => faker.Finance.Account())
                .RuleFor(metadata => metadata.ResourceGroupName, faker => faker.Name.FirstName())
                .RuleFor(metadata => metadata.SubscriptionId, faker => faker.Finance.Account());

            return bogusGenerator.Generate();
        }

        protected ScrapingV1 GenerateBogusScrapingInterval(string testInterval)
        {
            var bogusGenerator = new Faker<ScrapingV1>()
                .RuleFor(scraping => scraping.Schedule, faker => testInterval);

            return bogusGenerator.Generate();
        }
    }
}