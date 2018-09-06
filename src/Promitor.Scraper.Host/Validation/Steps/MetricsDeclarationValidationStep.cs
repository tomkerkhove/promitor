﻿using System;
using System.Collections.Generic;
using System.Linq;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Configuration.Providers.Interfaces;
using Promitor.Scraper.Host.Validation.Interfaces;
using Promitor.Scraper.Host.Validation.MetricDefinitions;

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

        private List<string> DetectDuplicateMetrics(List<MetricDefinition> metrics)
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

        private List<string> ValidateMetrics(List<MetricDefinition> metrics)
        {
            var errorMessages = new List<string>();

            if (metrics == null)
            {
                errorMessages.Add("No metrics are configured");
                return errorMessages;
            }

            var metricsValidator = new MetricsValidator();
            var metricErrorMessages = metricsValidator.Validate(metrics);
            errorMessages.AddRange(metricErrorMessages);

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