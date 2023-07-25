using System.Collections.Generic;
using Kusto.Language;
using System.Linq;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using ResourceType = Promitor.Core.Contracts.ResourceType;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions
{
    public class AzureMetricConfigurationValidator
    {
        private readonly MetricDefaults _metricDefaults;

        public AzureMetricConfigurationValidator(MetricDefaults metricDefaults)
        {
            _metricDefaults = metricDefaults;
        }

        public IEnumerable<string> Validate(MetricDefinition metrics)
        {
            if (metrics.ResourceType == ResourceType.LogAnalytics)
            {
                return ValidateLogAnalyticsConfiguration(metrics.LogAnalyticsConfiguration);
            }

            return ValidateAzureMetricConfiguration(metrics.AzureMetricConfiguration);
        }

        private IEnumerable<string> ValidateAzureMetricConfiguration(AzureMetricConfiguration azureMetricConfiguration)
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

            // Validate limit, if configured
            if (azureMetricConfiguration.Limit != null)
            {
                if (azureMetricConfiguration.Limit > Promitor.Core.Defaults.MetricDefaults.Limit)
                {
                    errorMessages.Add($"Limit cannot be higher than {Promitor.Core.Defaults.MetricDefaults.Limit}");
                }

                if (azureMetricConfiguration.Limit <= 0)
                {
                    errorMessages.Add("Limit has to be at least 1");
                }
            }

            if (azureMetricConfiguration.Dimension != null && azureMetricConfiguration.Dimensions.Any())
            {
                errorMessages.Add("Only one of 'dimensions' and 'dimension' is allowed. Please use 'dimensions'.");
            }

            var metricAggregationValidator = new MetricAggregationValidator(_metricDefaults);
            var metricsAggregationErrorMessages = metricAggregationValidator.Validate(azureMetricConfiguration.Aggregation);
            errorMessages.AddRange(metricsAggregationErrorMessages);

            return errorMessages;
        }

        private IEnumerable<string> ValidateLogAnalyticsConfiguration(LogAnalyticsConfiguration logAnalyticsConfiguration)
        {
            var resultString = "project result";
            var errorMessages = new List<string>();

            if (logAnalyticsConfiguration == null)
            {
                errorMessages.Add("Invalid Azure Log Analytics is configured");
                return errorMessages;
            }

            if (logAnalyticsConfiguration.Aggregation?.Interval == null)
            {
                errorMessages.Add("No Azure Log Analytics Interval is configured");
            }

            if (string.IsNullOrWhiteSpace(logAnalyticsConfiguration.Query))
            {
                errorMessages.Add("No Query for Azure Log Analytics is configured");
            }
            else
            {
                var code = KustoCode.Parse(logAnalyticsConfiguration.Query);
                var diagnostics = code.GetDiagnostics();
                if (diagnostics.Count > 0)
                {
                    errorMessages.Add("Syntax error with the query");
                }

                if (!logAnalyticsConfiguration.Query.Contains(resultString))
                {
                    errorMessages.Add("The Query need to return only 1 column name result only (use \"project result\")");
                }
            }
            return errorMessages;
        }
    }
}