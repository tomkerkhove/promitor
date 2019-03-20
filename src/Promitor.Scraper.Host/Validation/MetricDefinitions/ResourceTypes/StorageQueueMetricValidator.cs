using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class StorageQueueMetricValidator: IMetricValidator<StorageQueueMetricDefinition>
    {
        public List<string> Validate(StorageQueueMetricDefinition metricDefinition)
        {
            var errorMessages = new List<string>();
            
            if (string.IsNullOrWhiteSpace(metricDefinition.AccountName))
            {
                errorMessages.Add("No Azure Storage Account Name is configured");
            }
            
            if (string.IsNullOrWhiteSpace(metricDefinition.QueueName))
            {
                errorMessages.Add("No Azure Storage Queue Name is configured");
            }
            
            if (string.IsNullOrWhiteSpace(metricDefinition.SasToken))
            {
                errorMessages.Add("No Azure Storage SAS Token is configured");
            }

            if (metricDefinition.AzureMetricConfiguration.MetricName != "MessageCount")
            {
                errorMessages.Add($"Invalid metric name {metricDefinition.AzureMetricConfiguration.MetricName}");
            }

            return errorMessages;
        }
    }
}