using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.Scraper.Configuration;

namespace Promitor.Agents.Scraper.Validation.Steps.Sinks
{
    public class PrometheusScrapingEndpointMetricSinkValidationStep : ValidationStep, IValidationStep
    {
        private readonly IOptions<ScraperRuntimeConfiguration> _runtimeConfiguration;
        public PrometheusScrapingEndpointMetricSinkValidationStep(IOptions<ScraperRuntimeConfiguration> runtimeConfiguration, ILogger<PrometheusScrapingEndpointMetricSinkValidationStep> validationLogger)
            : base(validationLogger)
        {
            _runtimeConfiguration = runtimeConfiguration;
        }

        public string ComponentName => "Prometheus Scraping Endpoint Metric Sink";

        public ValidationResult Run()
        {
            var currentRuntimeConfiguration = _runtimeConfiguration.Value;
            var prometheusScrapingEndpointConfiguration = currentRuntimeConfiguration.MetricSinks?.PrometheusScrapingEndpoint;
            if (prometheusScrapingEndpointConfiguration == null)
            {
                return ValidationResult.Successful(ComponentName);
            }

            if (string.IsNullOrWhiteSpace(prometheusScrapingEndpointConfiguration.BaseUriPath))
            {
                var errorMessage = "No valid base URI was configured for Prometheus scraping endpoint in metric sink";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}
