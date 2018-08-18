using System.Collections.Generic;
using System.Linq;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Providers.Interfaces;
using Promitor.Scraper.Host.Model.Configuration;
using Promitor.Scraper.Host.Validation.Interfaces;
using Promitor.Scraper.Model.Configuration.Metrics.ResouceTypes;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class MetricsDeclarationValidationStep : ValidationStep, IValidationStep
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;

        public MetricsDeclarationValidationStep(IMetricsDeclarationProvider metricsDeclarationProvider)
        {
            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        public string ComponentName { get; } = "Metrics Declaration";

        public ValidationResult Run()
        {
            var rawMetricsConfiguration = _metricsDeclarationProvider.ReadRawDeclaration();
            LogMessage("Following metrics configuration was configured:");
            LogMessage(rawMetricsConfiguration);

            var metricsDeclaration = _metricsDeclarationProvider.Get();
            if (metricsDeclaration == null)
            {
                return ValidationResult.Failure(ComponentName, "Unable to deserialize configured metrics declaration");
            }

            var validationErrors = new List<string>();
            var azureMetadataErrorMessages = ValidateAzureMetadata(metricsDeclaration.AzureMetadata);
            validationErrors.AddRange(azureMetadataErrorMessages);

            var metricsErrorMessages = ValidateMetrics(metricsDeclaration.Metrics);
            validationErrors.AddRange(metricsErrorMessages);

            return validationErrors.Any() ? ValidationResult.Failure(ComponentName, validationErrors) : ValidationResult.Successful(ComponentName);
        }

        private List<string> DetectDuplicateMetrics(List<ServiceBusQueueMetricDefinition> metrics)
        {
            var duplicateMetricNames = metrics.GroupBy(metric => metric.Name)
                .Where(groupedMetrics => groupedMetrics.Count() > 1)
                .Select(groupedMetrics => groupedMetrics.Key)
                .ToList();

            return duplicateMetricNames;
        }

        private List<string> ValidateAzureMetadata(AzureMetadata azureMetadata)
        {
            var errorMessages = new List<string>();

            if (azureMetadata == null)
            {
                errorMessages.Add("No azure metadata is configured");
                return errorMessages;
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.TenantId))
            {
                errorMessages.Add($"{azureMetadata.TenantId} is not configured");
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.SubscriptionId))
            {
                errorMessages.Add($"{azureMetadata.SubscriptionId} is not configured");
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.ResourceGroupName))
            {
                errorMessages.Add($"{azureMetadata.ResourceGroupName} is not configured");
            }

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
                errorMessages.Add($"No {azureMetricConfiguration.MetricName} is configured");
            }

            return errorMessages;
        }

        private List<string> ValidateMetric(ServiceBusQueueMetricDefinition metric)
        {
            var errorMessages = new List<string>();

            if (metric == null)
            {
                errorMessages.Add("Invalid metric is configured");
                return errorMessages;
            }

            if (string.IsNullOrWhiteSpace(metric.Namespace))
            {
                errorMessages.Add($"{metric.Namespace} is not configured");
            }

            if (string.IsNullOrWhiteSpace(metric.QueueName))
            {
                errorMessages.Add($"{metric.QueueName} is not configured");
            }

            if (string.IsNullOrWhiteSpace(metric.Name))
            {
                errorMessages.Add($"{metric.QueueName} is not configured");
            }

            if (metric.ResourceType == ResourceType.NotSpecified)
            {
                errorMessages.Add($"{metric.ResourceType} '{nameof(ResourceType.NotSpecified)}' is not supported");
            }

            var metricsConfigurationErrorMessages = ValidateAzureMetricConfiguration(metric.AzureMetricConfiguration);
            errorMessages.AddRange(metricsConfigurationErrorMessages);

            return errorMessages;
        }

        private List<string> ValidateMetrics(List<ServiceBusQueueMetricDefinition> metrics)
        {
            var errorMessages = new List<string>();

            if (metrics == null)
            {
                errorMessages.Add("No metrics are configured");
                return errorMessages;
            }

            foreach (var metric in metrics)
            {
                var metricErrorMessages = ValidateMetric(metric);
                errorMessages.AddRange(metricErrorMessages);
            }

            // Detect duplicate metric names
            var duplicateMetricNames = DetectDuplicateMetrics(metrics);
            foreach (var duplicateMetricName in duplicateMetricNames)
            {
                errorMessages.Add($"Metric name '{duplicateMetricName}' is declared multiple times");
            }

            return errorMessages;
        }
    }
}