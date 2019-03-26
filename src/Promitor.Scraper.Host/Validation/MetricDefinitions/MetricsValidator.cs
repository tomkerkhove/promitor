﻿using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Validation.Factories;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions
{
    public class MetricsValidator
    {
        private readonly MetricDefaults _metricDefaults;

        public MetricsValidator(MetricDefaults metricDefaults)
        {
            _metricDefaults = metricDefaults;
        }

        public IList<string> Validate(List<MetricDefinition> metrics)
        {
            Guard.NotNull(metrics, nameof(metrics));

            var errorMessages = metrics
                .SelectMany(metric => Validate(metric))
                .AsParallel();

            return errorMessages.ToList();
        }

        private IList<string> Validate(MetricDefinition metric)
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

            var metricDefinitionValidationErrors = MetricValidatorFactory
                .GetValidatorFor(metric.ResourceType)
                .Validate(metric);

            errorMessages.AddRange(metricDefinitionValidationErrors);

            var metricAggregationValidator = new AzureMetricConfigurationValidator(_metricDefaults);
            var metricsConfigurationErrorMessages = metricAggregationValidator.Validate(metric.AzureMetricConfiguration);
            errorMessages.AddRange(metricsConfigurationErrorMessages);

            return errorMessages;
        }
    }
}