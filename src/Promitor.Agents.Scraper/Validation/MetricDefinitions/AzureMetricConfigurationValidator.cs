using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions
{
    public class AzureMetricConfigurationValidator
    {
        private readonly MetricDefaults _metricDefaults;

        public AzureMetricConfigurationValidator(MetricDefaults metricDefaults)
        {
            _metricDefaults = metricDefaults;
        }

        public IEnumerable<string> Validate(AzureMetricConfiguration azureMetricConfiguration)
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

            var metricAggregationValidator = new MetricAggregationValidator(_metricDefaults);
            var metricsAggregationErrorMessages = metricAggregationValidator.Validate(azureMetricConfiguration.Aggregation);
            errorMessages.AddRange(metricsAggregationErrorMessages);

            return errorMessages;
        }
    }
}