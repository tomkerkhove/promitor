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
        private const string EntityNameDimension = "EntityName";

        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            var errorMessages = new List<string>();

            var configuredDimension = metricDefinition.AzureMetricConfiguration?.Dimension?.Name;
            var isEntityNameDimensionConfigured = string.IsNullOrWhiteSpace(configuredDimension) == false && configuredDimension.Equals(EntityNameDimension, StringComparison.InvariantCultureIgnoreCase);

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<ServiceBusQueueResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.Namespace))
                {
                    errorMessages.Add("No Service Bus Namespace is configured");
                }

                if (isEntityNameDimensionConfigured && string.IsNullOrWhiteSpace(resourceDefinition.QueueName) == false)
                {
                    errorMessages.Add($"Queue name is configured while '{EntityNameDimension}' dimension is configured as well. We only support one or the other.");
                }
            }

            return errorMessages;
        }
    }
}