using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Integrations.AzureStorage;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class StorageQueueScraper : Scraper<StorageQueueResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Storage/storageAccounts/{2}/queueServices";
        private readonly AzureStorageQueueClient _azureStorageQueueClient;
        public StorageQueueScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
            _azureStorageQueueClient = new AzureStorageQueueClient(scraperConfiguration.Logger);
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, StorageQueueResourceDefinition resource, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            Guard.NotNull(scrapeDefinition, nameof(scrapeDefinition));
            Guard.NotNull(scrapeDefinition.AzureMetricConfiguration, nameof(scrapeDefinition.AzureMetricConfiguration));
            Guard.NotNull(resource.SasToken, nameof(resource.SasToken));
            Guard.NotNullOrEmpty(scrapeDefinition.AzureMetricConfiguration.MetricName, nameof(scrapeDefinition.AzureMetricConfiguration.MetricName));

            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.AccountName);
            var sasToken = resource.SasToken.GetSecretValue();
            double foundMetricValue;

            switch (scrapeDefinition.AzureMetricConfiguration.MetricName.ToLowerInvariant())
            {
                case AzureStorageConstants.Queues.Metrics.TimeSpentInQueue:
                    foundMetricValue = await _azureStorageQueueClient.GetQueueMessageTimeSpentInQueueAsync(resource.AccountName, resource.QueueName, sasToken);
                    break;
                case AzureStorageConstants.Queues.Metrics.MessageCount:
                    foundMetricValue = await _azureStorageQueueClient.GetQueueMessageCountAsync(resource.AccountName, resource.QueueName, sasToken);
                    break;
                default:
                    throw new InvalidMetricNameException(scrapeDefinition.AzureMetricConfiguration.MetricName, resource.ResourceType.ToString());
            }

            var labels = new Dictionary<string, string>
            {
                {"queue_name", resource.QueueName}
            };

            var measuredMetrics = new List<MeasuredMetric>
            {
                MeasuredMetric.CreateWithoutDimensions(foundMetricValue)
            };

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resource.AccountName, resourceUri, measuredMetrics, labels);
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, StorageQueueResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.AccountName);
        }
    }
}