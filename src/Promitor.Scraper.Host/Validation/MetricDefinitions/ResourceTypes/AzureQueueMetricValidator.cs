using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class AzureQueueMetricValidator: IMetricValidator<AzureQueueMetricDefinition>
    {
        public List<string> Validate(AzureQueueMetricDefinition metricDefinition)
        {
            var errorMessages = new List<string>();
            
            if (string.IsNullOrWhiteSpace(metricDefinition.AccountName))
            {
                errorMessages.Add("No Azure Queue Account Name is configured");
            }
            
            if (string.IsNullOrWhiteSpace(metricDefinition.QueueName))
            {
                errorMessages.Add("No Azure Queue Name is configured");
            }
            
            if (string.IsNullOrWhiteSpace(metricDefinition.SasToken))
            {
                errorMessages.Add("No Azure Queue SAS Token is configured");
            }

            return errorMessages;
        }
    }
}