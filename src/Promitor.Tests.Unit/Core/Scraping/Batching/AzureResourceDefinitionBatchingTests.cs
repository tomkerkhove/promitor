using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using AutoMapper;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Batching;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;

namespace Promitor.Tests.Unit.Core.Metrics
{
    [Category("Unit")]
    public class AzureResourceDefinitionBatchingTests
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
        private readonly static string _resourceGroupName = "batch_test_group";
        private readonly static int _batchSize = 50;

        public AzureResourceDefinitionBatchingTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void IdenticalBatchPropertiesShouldBatchTogether()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType: ResourceType.StorageAccount, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 10
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions(scrapeDefinitions, maxBatchSize: _batchSize, CancellationToken.None);
            // expect one batch of 10
            Assert.Single(groupedScrapeDefinitions);
            Assert.Equal(10, groupedScrapeDefinitions[0].ScrapeDefinitions.Count);
        }

        [Fact]
        public void BatchShouldSplitAccordingToConfiguredBatchSize()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);

            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 130
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions(scrapeDefinitions, maxBatchSize: _batchSize, CancellationToken.None);
            // expect three batches adding up to total size
            Assert.Equal(3, groupedScrapeDefinitions.Count);
            Assert.Equal(130, CountTotalScrapeDefinitions(groupedScrapeDefinitions));
        }

        [Fact]
        public void DifferentBatchPropertiesShouldBatchSeparately()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 10
            );
            var differentScrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType:  ResourceType.BlobStorage, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 10
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions([.. scrapeDefinitions, .. differentScrapeDefinitions], maxBatchSize: _batchSize, CancellationToken.None);
            // expect two batch of 10 each
            Assert.Equal(2, groupedScrapeDefinitions.Count);
            Assert.Equal(10, groupedScrapeDefinitions[0].ScrapeDefinitions.Count);
            Assert.Equal(10, groupedScrapeDefinitions[1].ScrapeDefinitions.Count);
        }

        [Fact]
        public void DifferentAggregationIntervalsShouldBatchSeparately()
        {
            var azureMetricConfiguration5mInterval = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            azureMetricConfiguration5mInterval.Aggregation.Interval = TimeSpan.FromMinutes(5);
            var azureMetricConfiguration2mInterval = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            azureMetricConfiguration5mInterval.Aggregation.Interval = TimeSpan.FromMinutes(2);
            var logAnalyticsConfiguration = _mapper.Map<LogAnalyticsConfiguration>(_logAnalyticsConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var scrapeDefinitions5m = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration5mInterval, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 10
            );
            var differentScrapeDefinitions2m = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration2mInterval, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType:  ResourceType.BlobStorage, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 10
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions([.. scrapeDefinitions5m, .. differentScrapeDefinitions2m], maxBatchSize: _batchSize, CancellationToken.None);
            // expect two batch of 10 each
            Assert.Equal(2, groupedScrapeDefinitions.Count);
            Assert.Equal(10, groupedScrapeDefinitions[0].ScrapeDefinitions.Count);
            Assert.Equal(10, groupedScrapeDefinitions[1].ScrapeDefinitions.Count);
        }


        [Fact]
        public void MixedBatchShouldSplitAccordingToConfiguredBatchSize()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var logAnalyticsConfiguration = new LogAnalyticsConfiguration();
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType: ResourceType.StorageAccount, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 130
            );
            var differentScrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType: ResourceType.BlobStorage, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 120
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions([.. scrapeDefinitions, .. differentScrapeDefinitions], maxBatchSize: _batchSize, CancellationToken.None);
            // expect two batch of 10 each
            Assert.Equal(6, groupedScrapeDefinitions.Count);
            Assert.Equal(250, CountTotalScrapeDefinitions(groupedScrapeDefinitions));
        }

        [Fact]
        public void BatchConstructionShouldBeAgnosticToResourceGroup()
        {
            var azureMetricConfiguration = _mapper.Map<AzureMetricConfiguration>(_azureMetricConfigurationBase);
            var scraping = _mapper.Map<Promitor.Core.Scraping.Configuration.Model.Scraping>(_scrapingBase);
            var logAnalyticsConfiguration = new LogAnalyticsConfiguration();
            var scrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: _subscriptionId, resourceGroupName: _resourceGroupName, 10
            );
            var differentScrapeDefinitions = BuildScrapeDefinitionBatch(
                azureMetricConfiguration: azureMetricConfiguration, logAnalyticsConfiguration: logAnalyticsConfiguration, prometheusMetricDefinition: _prometheusMetricDefinition, scraping: scraping, 
                resourceType:  ResourceType.StorageAccount, subscriptionId: _subscriptionId, resourceGroupName: "group2", 10
            );
            var groupedScrapeDefinitions = AzureResourceDefinitionBatching.GroupScrapeDefinitions([.. scrapeDefinitions, .. differentScrapeDefinitions], maxBatchSize: _batchSize, CancellationToken.None);
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