using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class ServiceBusQueueMetricValidator : MetricValidator<ServiceBusQueueMetricDefinition>
    {
        protected override IEnumerable<string> Validate(ServiceBusQueueMetricDefinition serviceBusQueueMetricDefinition)
        {
            Guard.NotNull(serviceBusQueueMetricDefinition, nameof(serviceBusQueueMetricDefinition));

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