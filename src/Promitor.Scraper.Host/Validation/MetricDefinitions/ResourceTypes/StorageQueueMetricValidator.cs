using System;
using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Integrations.AzureStorage;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class StorageQueueMetricValidator : MetricValidator<StorageQueueMetricDefinition>
    {
        private readonly ISet<string> _validMetricNames = new HashSet<string>(new[]
        {
            AzureStorageConstants.Queues.Metrics.MessageCount,
            AzureStorageConstants.Queues.Metrics.TimeSpentInQueue
        });

        protected override IEnumerable<string> Validate(StorageQueueMetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(metricDefinition.AccountName))
            {
                errorMessages.Add(item: "No Azure Storage Account Name is configured");
            }

            if (string.IsNullOrWhiteSpace(metricDefinition.QueueName))
            {
                errorMessages.Add(item: "No Azure Storage Queue Name is configured");
            }

            if (string.IsNullOrWhiteSpace(metricDefinition.SasToken))
            {
                errorMessages.Add(item: "No Azure Storage SAS Token is configured");
            }

            if (!_validMetricNames.Any(metricName => metricName.Equals(metricDefinition.AzureMetricConfiguration.MetricName, StringComparison.InvariantCultureIgnoreCase)))
            {
                errorMessages.Add($"Invalid metric name {metricDefinition.AzureMetricConfiguration.MetricName}");
            }

            return errorMessages;
        }
    }
}