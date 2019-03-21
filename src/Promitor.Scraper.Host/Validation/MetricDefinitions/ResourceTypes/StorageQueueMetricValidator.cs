using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class StorageQueueMetricValidator: IMetricValidator<StorageQueueMetricDefinition>
    {
        private readonly ISet<string> _validMetricNames = new HashSet<string>(new[] {"MessageCount", "Duration"});
        
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

            if (!_validMetricNames.Contains(metricDefinition.AzureMetricConfiguration.MetricName))
            {
                errorMessages.Add($"Invalid metric name {metricDefinition.AzureMetricConfiguration.MetricName}");
            }

            return errorMessages;
        }
    }
}