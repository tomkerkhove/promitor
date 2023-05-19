using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.Factories;
using Promitor.Core.Contracts;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions
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
                .SelectMany(Validate)
                .AsParallel();

            return errorMessages.ToList();
        }

        private IEnumerable<string> Validate(MetricDefinition metric)
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

            if (string.IsNullOrWhiteSpace(metric.PrometheusMetricDefinition?.Name))
            {
                errorMessages.Add("No metric name is configured");
            }

            if (metric.Resources?.Any() == false && metric.ResourceDiscoveryGroups?.Any() == false)
            {
                errorMessages.Add("No resource or resource collection is configured to be scraped");
            }
            else
            {
                var metricDefinitionValidationErrors = MetricValidatorFactory
                    .GetValidatorFor(metric.ResourceType)
                    .Validate(metric);

                errorMessages.AddRange(metricDefinitionValidationErrors);
            }

            var azureMetricConfigurationValidator = new AzureMetricConfigurationValidator(_metricDefaults);
            var azureMetricConfigurationErrorMessages = azureMetricConfigurationValidator.Validate(metric);
            errorMessages.AddRange(azureMetricConfigurationErrorMessages);

            var metricScrapingScheduleValidator = new MetricScrapingValidator(_metricDefaults);
            var metricScrapingErrorMessages = metricScrapingScheduleValidator.Validate(metric.Scraping);
            errorMessages.AddRange(metricScrapingErrorMessages);

            return errorMessages;
        }
    }
}