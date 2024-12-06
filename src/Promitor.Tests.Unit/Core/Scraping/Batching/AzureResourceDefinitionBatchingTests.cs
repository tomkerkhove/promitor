using System;
using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Batching;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;

namespace Promitor.Tests.Unit.Core.Scraping.Batching
{
    [Category("Unit")]
    public class AzureResourceDefinitionBatchingTests
    {
        private readonly IMapper _mapper; // to model instantiation happen
        private readonly static string AzureMetricNameBase = "promitor_batch_test_metric";
        private readonly static PrometheusMetricDefinition PrometheusMetricDefinitionTest =
            new("promitor_batch_test", "test", new Dictionary<string, string>());
        private readonly static string SubscriptionIdTest = "subscription";
        private readonly static AzureMetricConfigurationV1 AzureMetricConfigurationBase = new AzureMetricConfigurationV1 
            {
                MetricName = AzureMetricNameBase,
                Aggregation = new MetricAggregationV1
                {
                    Type = PromitorMetricAggregationType.Average
                },
            };
        private readonly static LogAnalyticsConfigurationV1 LogAnalyticsConfigurationBase = new LogAnalyticsConfigurationV1 
            {
                Query = "A eq B",
                Aggregation = new AggregationV1
                {
                    Interval = TimeSpan.FromMinutes(60)
                },
            };
        private readonly static ScrapingV1 ScrapingBase = new ScrapingV1
            {
                Schedule = "5 4 3 2 1"
            };
        private readonly static string ResourceGroupNameTest = "batch_test_group";
        private readonly static int BatchSize = 50;

        public AzureResourceDefinitionBatchingTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void IdenticalBatchPropertiesShouldBatchTogether()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(AzureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(ScrapingBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(LogAnalyticsConfigurationBase);
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType: ResourceType.StorageAccount, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 10
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions(scrapeDefinitions, maxBatchSize: BatchSize);
            // expect one batch of 10
            Assert.Single(groupedScrapeDefinitions);
            Assert.Equal(10, groupedScrapeDefinitions[0].ScrapeDefinitions.Count);
        }

        [Fact]
        public void BatchShouldSplitAccordingToConfiguredBatchSize()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(AzureMetricConfigurationBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(LogAnalyticsConfigurationBase);
            var testBatchSize = 10;

            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(ScrapingBase);
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 25
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions(scrapeDefinitions, maxBatchSize: testBatchSize);
            // expect three batches adding up to total size
            Assert.Equal(3, groupedScrapeDefinitions.Count);
            Assert.Equal(25, CountTotalScrapeDefinitions(groupedScrapeDefinitions));
        }

        [Fact]
        public void DifferentBatchPropertiesShouldBatchSeparately()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(AzureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(ScrapingBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(LogAnalyticsConfigurationBase);
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 10
            );
            var differentScrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType:  ResourceType.BlobStorage, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 10
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions([.. scrapeDefinitions, .. differentScrapeDefinitions], maxBatchSize: BatchSize);
            // expect two batch of 10 each
            Assert.Equal(2, groupedScrapeDefinitions.Count);
            Assert.Equal(10, groupedScrapeDefinitions[0].ScrapeDefinitions.Count);
            Assert.Equal(10, groupedScrapeDefinitions[1].ScrapeDefinitions.Count);
        }

        [Fact]
        public void DifferentAggregationIntervalsShouldBatchSeparately()
        {
            var azureMetricConfiguration5MInterval = _mapper.Map<AzureMetricConfiguration>(AzureMetricConfigurationBase);
            azureMetricConfiguration5MInterval.Aggregation.Interval = TimeSpan.FromMinutes(5);
            var azureMetricConfiguration2MInterval = _mapper.Map<AzureMetricConfiguration>(AzureMetricConfigurationBase);
            azureMetricConfiguration5MInterval.Aggregation.Interval = TimeSpan.FromMinutes(2);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(LogAnalyticsConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(ScrapingBase);
            var scrapeDefinitions5M = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration5MInterval, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 10
            );
            var differentScrapeDefinitions2M = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration2MInterval, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType:  ResourceType.BlobStorage, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 10
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions([.. scrapeDefinitions5M, .. differentScrapeDefinitions2M], maxBatchSize: BatchSize);
            // expect two batch of 10 each
            Assert.Equal(2, groupedScrapeDefinitions.Count);
            Assert.Equal(10, groupedScrapeDefinitions[0].ScrapeDefinitions.Count);
            Assert.Equal(10, groupedScrapeDefinitions[1].ScrapeDefinitions.Count);
        }


        [Fact]
        public void MixedBatchShouldSplitAccordingToConfiguredBatchSize()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(AzureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(ScrapingBase);
            var logAnalyticsConfiguration = new LogAnalyticsConfiguration();
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType: ResourceType.StorageAccount, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 130
            );
            var differentScrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType: ResourceType.BlobStorage, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 120
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions([.. scrapeDefinitions, .. differentScrapeDefinitions], maxBatchSize: BatchSize);
            // expect two batch of 10 each
            Assert.Equal(6, groupedScrapeDefinitions.Count);
            Assert.Equal(250, CountTotalScrapeDefinitions(groupedScrapeDefinitions));
        }

        [Fact]
        public void BatchConstructionShouldBeAgnosticToResourceGroup()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(AzureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(ScrapingBase);
            var logAnalyticsConfiguration = new LogAnalyticsConfiguration();
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: SubscriptionIdTest, resourceGroupName: ResourceGroupNameTest, 10
            );
            var differentScrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: PrometheusMetricDefinitionTest, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: SubscriptionIdTest, resourceGroupName: "group2", 10
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions([.. scrapeDefinitions, .. differentScrapeDefinitions], maxBatchSize: BatchSize);
            // expect two batch of 10 each
            Assert.Single(groupedScrapeDefinitions);
            Assert.Equal(20, groupedScrapeDefinitions[0].ScrapeDefinitions.Count);
        }
    
        private static List<ScrapeDefinition<IAzureResourceDefinition>> BuildScrapeDefinitionBatch(
            AzureMetricConfiguration azureMetricConfiguration,
            LogAnalyticsConfiguration logAnalyticsConfiguration,
            PrometheusMetricDefinition prometheusMetricDefinition,
            Promitor.Core.Scraping.Configuration.Model.Scraping scraping,
            ResourceType resourceType,
            string subscriptionId,
            string resourceGroupName,
            int size)
        {
            // builds a batch of scrape definitions of specified size, each sharing properties passed in through function parameters
            var batch = new List<ScrapeDefinition<IAzureResourceDefinition>>();
            for (var resoureceIdCounter = 0; resoureceIdCounter <  size; resoureceIdCounter++)
            {
                var resourceName = "resource" + resoureceIdCounter.ToString();
                var resourceDefinition = new AzureResourceDefinition(resourceType, subscriptionId, resourceGroupName, resourceName: resourceName, uniqueName: resourceName);
                batch.Add(new ScrapeDefinition<IAzureResourceDefinition>(azureMetricConfiguration, logAnalyticsConfiguration, prometheusMetricDefinition, scraping, resourceDefinition, subscriptionId, resourceGroupName));
            } 
            return batch;
        }

        private static int CountTotalScrapeDefinitions(List<BatchScrapeDefinition<IAzureResourceDefinition>> groupedScrapeDefinitions) 
        {
            var count = 0;
            groupedScrapeDefinitions.ForEach(batch => count += batch.ScrapeDefinitions.Count);
            return count;
        }
    }
}