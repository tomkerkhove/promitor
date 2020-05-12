using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Validation.Interfaces;

namespace Promitor.Agents.Scraper.Validation.Steps.Sinks
{
    public class PrometheusScrapingEndpointMetricSinkValidationStep : ValidationStep, IValidationStep
    {
        private readonly IOptions<ScraperRuntimeConfiguration> _runtimeConfiguration;

        public PrometheusScrapingEndpointMetricSinkValidationStep(IOptions<ScraperRuntimeConfiguration> runtimeConfiguration)
            : this(runtimeConfiguration, NullLogger.Instance)
        {
        }

        public PrometheusScrapingEndpointMetricSinkValidationStep(IOptions<ScraperRuntimeConfiguration> runtimeConfiguration, ILogger validationLogger)
            : base(validationLogger)
        {
            _runtimeConfiguration = runtimeConfiguration;
        }

        public string ComponentName { get; } = "Prometheus Scraping Endpoint Metric Sink";

        public ValidationResult Run()
        {
            var currentRuntimeConfiguration = _runtimeConfiguration.Value;
            var legacyPrometheusConfiguration = currentRuntimeConfiguration?.Prometheus;
            var prometheusScrapingEndpointConfiguration = currentRuntimeConfiguration?.MetricSinks?.PrometheusScrapingEndpoint;
            if (prometheusScrapingEndpointConfiguration == null && legacyPrometheusConfiguration == null)
            {
                return ValidationResult.Successful(ComponentName);
            }

            if (string.IsNullOrWhiteSpace(prometheusScrapingEndpointConfiguration?.BaseUriPath) == false &&
                string.IsNullOrWhiteSpace(legacyPrometheusConfiguration?.ScrapeEndpoint?.BaseUriPath) == false &&
                legacyPrometheusConfiguration.ScrapeEndpoint.BaseUriPath.Equals(prometheusScrapingEndpointConfiguration.BaseUriPath, StringComparison.InvariantCultureIgnoreCase))
            {
                var errorMessage = "Duplicate Prometheus scraping endpoint was configured in the metric sink & legacy configuration";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }
            else
            {
                if (prometheusScrapingEndpointConfiguration != null && string.IsNullOrWhiteSpace(prometheusScrapingEndpointConfiguration.BaseUriPath))
                {
                    var errorMessage = "No valid base URI was configured for Prometheus scraping endpoint in metric sink";
                    return ValidationResult.Failure(ComponentName, errorMessage);
                }

                if (legacyPrometheusConfiguration?.ScrapeEndpoint != null && string.IsNullOrWhiteSpace(legacyPrometheusConfiguration.ScrapeEndpoint.BaseUriPath))
                {
                    var errorMessage = "No valid base URI was configured for Prometheus scraping endpoint in legacy configuration";
                    return ValidationResult.Failure(ComponentName, errorMessage);
                }
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}
