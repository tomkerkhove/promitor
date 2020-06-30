using System;
using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Integrations.AzureStorage;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class StorageQueueMetricValidator : IMetricValidator
    {
        private readonly ISet<string> _validMetricNames = new HashSet<string>(new[]
        {
            AzureStorageConstants.Queues.Metrics.MessageCount,
            AzureStorageConstants.Queues.Metrics.TimeSpentInQueue
        });

        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<StorageQueueResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.AccountName))
                {
                    yield return "No Azure Storage Account Name is configured";
                }

                if (string.IsNullOrWhiteSpace(resourceDefinition.QueueName))
                {
                    yield return "No Azure Storage Queue Name is configured";
                }

                var configuredSasToken = resourceDefinition.SasToken?.GetSecretValue();
                if (string.IsNullOrWhiteSpace(configuredSasToken))
                {
                    yield return "No Azure Storage SAS Token is configured";
                }

                if (!_validMetricNames.Any(metricName => metricName.Equals(metricDefinition.AzureMetricConfiguration.MetricName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    yield return $"Invalid metric name {metricDefinition.AzureMetricConfiguration.MetricName}";
                }
            }
        }
    }
}