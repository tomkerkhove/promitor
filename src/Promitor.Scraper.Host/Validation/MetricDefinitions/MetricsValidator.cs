using System;
using System.Collections.Generic;
using Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions
{
    public class MetricsValidator
    {
        public List<string> Validate(List<MetricDefinition> metrics)
        {
            Guard.NotNull(metrics, nameof(metrics));

            var errorMessages = new List<string>();

            foreach (var metric in metrics)
            {
                var metricErrorMessages = Validate(metric);
                errorMessages.AddRange(metricErrorMessages);
            }

            return errorMessages;
        }

        public List<string> Validate(MetricDefinition metric)
        {
            Guard.NotNull(metric, nameof(metric));

            var errorMessages = new List<string>();

            if (metric == null)
            {
                errorMessages.Add("Invalid metric is configured");
                return errorMessages;
            }

            if (metric.ResourceType == ResourceType.NotSpecified)
            {
                errorMessages.Add($"{metric.ResourceType} '{nameof(ResourceType.NotSpecified)}' is not supported");
            }

            if (string.IsNullOrWhiteSpace(metric.Name))
            {
                errorMessages.Add("No metric name is configured");
            }

            List<string> metricDefinitionValidationErrors;
            switch (metric.ResourceType)
            {
                case ResourceType.ServiceBusQueue:
                    var serviceBusQueueMetricValidator = new ServiceBusQueueMetricValidator();
                    metricDefinitionValidationErrors = serviceBusQueueMetricValidator.Validate(metric as ServiceBusQueueMetricDefinition);
                    break;
                case ResourceType.Generic:
                    var genericMetricDefinition = new GenericMetricValidator();
                    metricDefinitionValidationErrors = genericMetricDefinition.Validate(metric as GenericMetricDefinition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(metric), metric.ResourceType, $"No validation rules are defined for metric type '{metric.ResourceType}'");
            }

            errorMessages.AddRange(metricDefinitionValidationErrors);

            var metricsConfigurationErrorMessages = ValidateAzureMetricConfiguration(metric.AzureMetricConfiguration);
            errorMessages.AddRange(metricsConfigurationErrorMessages);

            return errorMessages;
        }

        private List<string> ValidateAzureMetricConfiguration(AzureMetricConfiguration azureMetricConfiguration)
        {
            var errorMessages = new List<string>();

            if (azureMetricConfiguration == null)
            {
                errorMessages.Add("Invalid azure metric configuration is configured");
                return errorMessages;
            }

            if (string.IsNullOrWhiteSpace(azureMetricConfiguration.MetricName))
            {
                errorMessages.Add("No metric name for Azure is configured");
            }

            return errorMessages;
        }
    }
}