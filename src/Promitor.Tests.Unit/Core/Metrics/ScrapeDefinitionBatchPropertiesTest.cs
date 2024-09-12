using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AutoMapper;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;

namespace Promitor.Tests.Unit.Core.Metrics
{
    [Category("Unit")]
    public class ScrapeDefinitionBatchPropertiesTest
    {
        private readonly IMapper _mapper; // to model instantiation happen
        private readonly static string _azureMetricNameBase = "promitor_batch_test_metric";
        private readonly static PrometheusMetricDefinition _prometheusMetricDefinition =
            new("promitor_batch_test", "test", new Dictionary<string, string>());
        private readonly static string _subscriptionId = "subscription";
        private readonly static AzureMetricConfigurationV1 _azureMetricConfigurationBase = new AzureMetricConfigurationV1 
            {
                MetricName = _azureMetricNameBase,
                Aggregation = new MetricAggregationV1
                {
                    Type = PromitorMetricAggregationType.Average
                },
            };
        private readonly static LogAnalyticsConfigurationV1 _logAnalyticsConfigurationBase = new LogAnalyticsConfigurationV1 
            {
                Query = "A eq B",
                Aggregation = new AggregationV1
                {
                    Interval = TimeSpan.FromMinutes(60)
                },
            };
        private readonly static ScrapingV1 _scrapingBase = new ScrapingV1
            {
                Schedule = "5 4 3 2 1"
            };

        public ScrapeDefinitionBatchPropertiesTest()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void BuildBatchHashKeySameResultNoDimensions()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);

            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var batchProperties = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);
            var batchProperties2 = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);

            var hashCode1 = batchProperties.GetHashCode();
            var hashCode2 = batchProperties2.GetHashCode();
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void BuildBatchHashKeySameResultIdenticalDimensions()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            azureMetricConfiguration.Dimensions = [new MetricDimension{Name = "Dimension1"},  new MetricDimension{Name = "Dimension2"}];
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);

            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);

            var batchProperties = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);
            var batchProperties2 = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);

            var hashCode1 = batchProperties.GetHashCode();
            var hashCode2 = batchProperties2.GetHashCode();
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void BuildBatchHashKeyDifferentResultDifferentDimensions()
        {
            var azureMetricConfiguration1 = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            azureMetricConfiguration1.Dimensions = [new MetricDimension{Name = "Dimension1"},  new MetricDimension{Name = "Dimension2"}];
            var azureMetricConfiguration2 = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            azureMetricConfiguration2.Dimensions = [new MetricDimension{Name = "DiffDimension1"},  new MetricDimension{Name = "DiffDimension2"}];
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);

            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);

            var batchProperties = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration1, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);
            var batchProperties2 = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration2, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);

            var hashCode1 = batchProperties.GetHashCode();
            var hashCode2 = batchProperties2.GetHashCode();
            Assert.NotEqual(hashCode1, hashCode2);
        }

         [Fact]
        public void BuildBatchHashKeyDifferentResultDifferentMetricName()
        {
            var azureMetricConfiguration1 = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            azureMetricConfiguration1.Dimensions = [new MetricDimension{Name = "Dimension1"},  new MetricDimension{Name = "Dimension2"}];
            var azureMetricConfiguration2 = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            azureMetricConfiguration2.Dimensions = [new MetricDimension{Name = "Dimension1"},  new MetricDimension{Name = "Dimension2"}];
            azureMetricConfiguration2.MetricName = "diffName";
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);

            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);

            var batchProperties = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration1, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);
            var batchProperties2 = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration2, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);

            var hashCode1 = batchProperties.GetHashCode();
            var hashCode2 = batchProperties2.GetHashCode();
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void BuildBatchHashKeyDifferentResultDifferentSubscription()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);


            var batchProperties = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);
            var batchProperties2 = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: "subscription2");

            var hashCode1 = batchProperties.GetHashCode();
            var hashCode2 = batchProperties2.GetHashCode();
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void BuildBatchHashKeyDifferentResultDifferentResourceType()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);

            var batchProperties = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping, subscriptionId: _subscriptionId);
            var batchProperties2 = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.LoadBalancer, scraping: scraping, subscriptionId: "subscription2");

            var hashCode1 = batchProperties.GetHashCode();
            var hashCode2 = batchProperties2.GetHashCode();
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void BuildBatchHashKeyDifferentResultDifferentSchedule()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var scraping1 = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var scraping2 = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            scraping2.Schedule = "6 4 3 2 1";
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);


            var batchProperties = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping1, subscriptionId: _subscriptionId);
            var batchProperties2 = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.StorageAccount, scraping: scraping2, subscriptionId: "subscription2");

            var hashCode1 = batchProperties.GetHashCode();
            var hashCode2 = batchProperties2.GetHashCode();
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void BuildBatchHashKeyTest()
        {
            AzureMetricConfigurationV1 _azureMetricConfigurationTest1 = new AzureMetricConfigurationV1 
            {
                MetricName = "availabilityResults/availabilityPercentage",
                Aggregation = new MetricAggregationV1
                {
                    Type = PromitorMetricAggregationType.Average
                },
            };
            AzureMetricConfigurationV1 _azureMetricConfigurationTest2 = new AzureMetricConfigurationV1 
            {
                MetricName = "availabilityResults/availabilityPercentage",
                Dimensions = [new MetricDimensionV1{Name = "availabilityResult/name"}],
                Aggregation = new MetricAggregationV1
                {
                    Type = PromitorMetricAggregationType.Average
                },
            };
            var azureMetricConfiguration1 = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationTest1);
            var azureMetricConfiguration2 = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationTest2);

            var scraping1 = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var scraping2 = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);


            var batchProperties = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration1, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.ApplicationInsights, scraping: scraping1, subscriptionId: _subscriptionId);
            var batchProperties2 = new ScrapeDefinitionBatchProperties(azureMetricConfiguration: azureMetricConfiguration2, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, resourceType: Promitor.Core.Contracts.ResourceType.ApplicationInsights, scraping: scraping2, subscriptionId: _subscriptionId);

            var hashCode1 = batchProperties.GetHashCode();
            var hashCode2 = batchProperties2.GetHashCode();
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}