using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class ServiceBusQueueMetricValidator : IMetricValidator<ServiceBusQueueMetricDefinition>
    {
        public List<string> Validate(ServiceBusQueueMetricDefinition serviceBusQueueMetricDefinition)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(serviceBusQueueMetricDefinition.Namespace))
            {
                errorMessages.Add("No Service Bus Namespace is configured");
            }

            if (string.IsNullOrWhiteSpace(serviceBusQueueMetricDefinition.QueueName))
            {
                errorMessages.Add("No queue name is configured");
            }

            return errorMessages;
        }
    }
}