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

            if (string.IsNullOrWhiteSpace(serviceBusQueueMetricDefinition.Namespace))
            {
                yield return "No Service Bus Namespace is configured";
            }

            if (string.IsNullOrWhiteSpace(serviceBusQueueMetricDefinition.QueueName))
            {
                yield return "No queue name is configured";
            }
        }
    }
}