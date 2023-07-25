using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class ServiceBusNamespaceMetricValidator : IMetricValidator
    {
        private const string EntityNameDimension = "EntityName";

        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            var errorMessages = new List<string>();

            if (metricDefinition.AzureMetricConfiguration?.Dimensions?.Count > 1)
            {
                errorMessages.Add("At least one Dimension other than EntityName is defined.");
            }

            var isEntityNameDimensionConfigured = metricDefinition.AzureMetricConfiguration?.HasDimension(EntityNameDimension) ?? false;

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<ServiceBusNamespaceResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.Namespace))
                {
                    errorMessages.Add("No Service Bus Namespace is configured");
                }

                if (isEntityNameDimensionConfigured && string.IsNullOrWhiteSpace(resourceDefinition.QueueName) == false)
                {
                    errorMessages.Add($"Queue name is configured while '{EntityNameDimension}' dimension is configured as well. We only support one or the other.");
                }

                if (isEntityNameDimensionConfigured && string.IsNullOrWhiteSpace(resourceDefinition.TopicName) == false)
                {
                    errorMessages.Add($"Topic name is configured while '{EntityNameDimension}' dimension is configured as well. We only support one or the other.");
                }

                if (string.IsNullOrWhiteSpace(resourceDefinition.QueueName) == false && string.IsNullOrWhiteSpace(resourceDefinition.TopicName) == false)
                {
                    errorMessages.Add("Queue & topic name are both configured while we only support one or the other.");
                }
            }

            return errorMessages;
        }
    }
}