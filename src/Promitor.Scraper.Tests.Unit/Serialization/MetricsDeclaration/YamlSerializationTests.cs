using System;
using System.ComponentModel;
using Bogus;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.MetricsDeclaration
{
    [Category("Unit")]
    public class YamlSerializationTests<TMetricDefinition> where TMetricDefinition : MetricDefinition
    {
        protected void AssertMetricDefinition(MetricDefinition deserializedMetricDefinition, TMetricDefinition metricDefinition)
        {
            Assert.NotNull(deserializedMetricDefinition);
            Assert.Equal(metricDefinition.Name, deserializedMetricDefinition.Name);
            Assert.Equal(metricDefinition.Description, deserializedMetricDefinition.Description);
            Assert.Equal(metricDefinition.ResourceType, deserializedMetricDefinition.ResourceType);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration);
            Assert.Equal(metricDefinition.AzureMetricConfiguration.MetricName, deserializedMetricDefinition.AzureMetricConfiguration.MetricName);
            Assert.NotNull(deserializedMetricDefinition.AzureMetricConfiguration.Aggregation);
            Assert.Equal(metricDefinition.AzureMetricConfiguration.Aggregation.Type, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(metricDefinition.AzureMetricConfiguration.Aggregation.Interval, deserializedMetricDefinition.AzureMetricConfiguration.Aggregation.Interval);
        }

        protected void AssertMetricDefaults(Core.Scraping.Configuration.Model.MetricsDeclaration deserializedConfiguration, MetricDefaults metricDefaults)
        {
            var deserializedMetricDefaults = deserializedConfiguration.MetricDefaults;
            Assert.NotNull(deserializedMetricDefaults);
            Assert.NotNull(deserializedMetricDefaults.Aggregation);
            Assert.Equal(metricDefaults.Aggregation.Interval, deserializedMetricDefaults.Aggregation.Interval);
        }

        protected void AssertAzureMetadata(Core.Scraping.Configuration.Model.MetricsDeclaration deserializedConfiguration, AzureMetadata azureMetadata)
        {
            Assert.NotNull(deserializedConfiguration.AzureMetadata);
            Assert.Equal(azureMetadata.TenantId, deserializedConfiguration.AzureMetadata.TenantId);
            Assert.Equal(azureMetadata.ResourceGroupName, deserializedConfiguration.AzureMetadata.ResourceGroupName);
            Assert.Equal(azureMetadata.SubscriptionId, deserializedConfiguration.AzureMetadata.SubscriptionId);
        }

        protected AzureMetricConfiguration GenerateBogusAzureMetricConfiguration()
        {
            var bogusMetricAggregation = new Faker<MetricAggregation>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(aggregation => aggregation.Type, faker => faker.PickRandom<Microsoft.Azure.Management.Monitor.Fluent.Models.AggregationType>())
                .RuleFor(aggregation => aggregation.Interval, faker => TimeSpan.FromMinutes(faker.Random.Int()))
                .Generate();

            var bogusMetricConfiguration = new Faker<AzureMetricConfiguration>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(metricDefinition => metricDefinition.MetricName, faker => faker.Name.FirstName())
                .RuleFor(metricDefinition => metricDefinition.Aggregation, faker => bogusMetricAggregation)
                .Generate();

            return bogusMetricConfiguration;
        }

        protected MetricDefaults GenerateBogusMetricDefaults()
        {
            var bogusAggregationGenerator = new Faker<Aggregation>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(aggregation => aggregation.Interval, faker => TimeSpan.FromMinutes(faker.Random.Int()));

            var generatedAggregation = bogusAggregationGenerator.Generate();
            var metricDefaults = new MetricDefaults
            {
                Aggregation = generatedAggregation
            };

            return metricDefaults;
        }

        protected AzureMetadata GenerateBogusAzureMetadata()
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