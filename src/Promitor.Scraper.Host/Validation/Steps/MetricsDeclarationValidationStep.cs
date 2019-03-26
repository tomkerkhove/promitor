﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Scraper.Host.Validation.Interfaces;
using Promitor.Scraper.Host.Validation.MetricDefinitions;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class MetricsDeclarationValidationStep : ValidationStep, IValidationStep
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;

        public MetricsDeclarationValidationStep(IMetricsDeclarationProvider metricsDeclarationProvider) : this(metricsDeclarationProvider, NullLogger.Instance)
        {
        }

        public MetricsDeclarationValidationStep(IMetricsDeclarationProvider metricsDeclarationProvider, ILogger logger) : base(logger)
        {
            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        public string ComponentName { get; } = "Metrics Declaration";

        public ValidationResult Run()
        {
            var rawMetricsConfiguration = _metricsDeclarationProvider.ReadRawDeclaration();
            Logger.LogInformation("Following metrics configuration was configured:\n{Configuration}", rawMetricsConfiguration);

            var metricsDeclaration = _metricsDeclarationProvider.Get();
            if (metricsDeclaration == null)
            {
                return ValidationResult.Failure(ComponentName, "Unable to deserialize configured metrics declaration");
            }

            var validationErrors = new List<string>();
            var azureMetadataErrorMessages = ValidateAzureMetadata(metricsDeclaration.AzureMetadata);
            validationErrors.AddRange(azureMetadataErrorMessages);

            var metricsErrorMessages = ValidateMetrics(metricsDeclaration.Metrics, metricsDeclaration.MetricDefaults);
            validationErrors.AddRange(metricsErrorMessages);

            return validationErrors.Any() ? ValidationResult.Failure(ComponentName, validationErrors) : ValidationResult.Successful(ComponentName);
        }

        private static IEnumerable<string> DetectDuplicateMetrics(List<MetricDefinition> metrics)
        {
            var duplicateMetricNames = metrics.GroupBy(metric => metric.Name)
                .Where(groupedMetrics => groupedMetrics.Count() > 1)
                .Select(groupedMetrics => groupedMetrics.Key);

            return duplicateMetricNames;
        }

        private static IEnumerable<string> ValidateAzureMetadata(AzureMetadata azureMetadata)
        {
            var errorMessages = new List<string>();

            if (azureMetadata == null)
            {
                errorMessages.Add("No azure metadata is configured");
                return errorMessages;
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.TenantId))
            {
                errorMessages.Add("No tenant id is configured");
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.SubscriptionId))
            {
                errorMessages.Add("No subscription id is configured");
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.ResourceGroupName))
            {
                errorMessages.Add("No resource group name is not configured");
            }

            return errorMessages;
        }

        private static IEnumerable<string> ValidateMetrics(List<MetricDefinition> metrics, MetricDefaults metricDefaults)
        {
            var errorMessages = new List<string>();

            if (metrics == null)
            {
                errorMessages.Add("No metrics are configured");
                return errorMessages;
            }

            var metricsValidator = new MetricsValidator(metricDefaults);
            var metricErrorMessages = metricsValidator.Validate(metrics);
            errorMessages.AddRange(metricErrorMessages);

            // Detect duplicate metric names
            var duplicateMetrics = DetectDuplicateMetrics(metrics);
            errorMessages.AddRange(duplicateMetrics.Select(duplicateMetricName => $"Metric name '{duplicateMetricName}' is declared multiple times"));

            return errorMessages;
        }
    }
}