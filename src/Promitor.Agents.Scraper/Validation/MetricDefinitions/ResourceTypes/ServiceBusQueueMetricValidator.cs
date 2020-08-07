using System;
using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class ServiceBusQueueMetricValidator : IMetricValidator
    {
        private const string UnsupportedEntityDimension = "EntityName";

        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            var errorMessages = new List<string>();

            var configuredDimension = metricDefinition.AzureMetricConfiguration?.Dimension?.Name;
            if (string.IsNullOrWhiteSpace(configuredDimension) == false
                && configuredDimension.Equals(UnsupportedEntityDimension, StringComparison.InvariantCultureIgnoreCase))
            {
                errorMessages.Add($"Dimension '{UnsupportedEntityDimension}' is not supported for now");
            }

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<ServiceBusQueueResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.Namespace))
                {
                    errorMessages.Add("No Service Bus Namespace is configured");
                }
            }

            return errorMessages;
        }
    }
}