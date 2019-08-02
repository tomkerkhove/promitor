using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class ServiceBusTopicMetricValidator : MetricValidator<ServiceBusTopicMetricDefinition>
    {
        protected override IEnumerable<string> Validate(ServiceBusTopicMetricDefinition serviceBusTopicMetricDefinition)
        {
            Guard.NotNull(serviceBusTopicMetricDefinition, nameof(serviceBusTopicMetricDefinition));

            if (string.IsNullOrWhiteSpace(serviceBusTopicMetricDefinition.Namespace))
            {
                yield return "No Service Bus Namespace is configured";
            }

            if (string.IsNullOrWhiteSpace(serviceBusTopicMetricDefinition.TopicName))
            {
                yield return "No queue name is configured";
            }

            if (string.IsNullOrWhiteSpace(serviceBusTopicMetricDefinition.SubscriptionName))
            {
                yield return "No subscription name is configured";
            }
        }
    }
}