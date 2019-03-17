using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class AzureStorageQueueMetricValidator: IMetricValidator<AzureStorageQueueMetricDefinition>
    {
        public List<string> Validate(AzureStorageQueueMetricDefinition metricDefinition)
        {
            var errorMessages = new List<string>();
            
            if (string.IsNullOrWhiteSpace(metricDefinition.AccountName))
            {
                errorMessages.Add("No Azure Storage Queue Account Name is configured");
            }
            
            if (string.IsNullOrWhiteSpace(metricDefinition.QueueName))
            {
                errorMessages.Add("No Azure Storage Queue Name is configured");
            }
            
            if (string.IsNullOrWhiteSpace(metricDefinition.SasToken))
            {
                errorMessages.Add("No Azure Storage Queue SAS Token is configured");
            }

            if (!(metricDefinition.AzureMetricConfiguration.MetricName == "Duration" || metricDefinition.AzureMetricConfiguration.MetricName == "Size"))
            {
                errorMessages.Add($"Invalid metric name {metricDefinition.AzureMetricConfiguration.MetricName}");
            }

            return errorMessages;
        }
    }
}