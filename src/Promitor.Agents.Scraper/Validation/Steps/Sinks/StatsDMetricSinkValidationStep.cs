using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Integrations.Sinks.Statsd.Configuration;

namespace Promitor.Agents.Scraper.Validation.Steps.Sinks
{
    public class StatsDMetricSinkValidationStep : ValidationStep,
        IValidationStep
    {
        private readonly IOptions<ScraperRuntimeConfiguration> _runtimeConfiguration;

        public StatsDMetricSinkValidationStep(IOptions<ScraperRuntimeConfiguration> runtimeConfiguration, ILogger<StatsDMetricSinkValidationStep> validationLogger)
            : base(validationLogger)
        {
            _runtimeConfiguration = runtimeConfiguration;
        }

        public string ComponentName => "StatsD Metric Sink";

        public ValidationResult Run()
        {
            var currentRuntimeConfiguration = _runtimeConfiguration.Value;
            var statsDConfiguration = currentRuntimeConfiguration.MetricSinks?.Statsd;
            if (statsDConfiguration == null)
            {
                return ValidationResult.Successful(ComponentName);
            }

            var errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(statsDConfiguration.Host))
            {
                errorMessages.Add("No host of StatsD server is configured");
            }

            if (statsDConfiguration.Port <= 0)
            {
                errorMessages.Add($"StatsD port {statsDConfiguration.Port} is not allowed");
            }

            if (statsDConfiguration.MetricFormat == StatsdFormatterTypesEnum.Geneva)
            {
                if (statsDConfiguration.Geneva is null)
                {
                    errorMessages.Add("StatsD Geneva formatter configuration is missing");
                }
                else 
                {
                    if (string.IsNullOrWhiteSpace(statsDConfiguration.Geneva.Account))
                    {
                        errorMessages.Add("Account of Geneva is missing for StatsD formatter");
                    }

                    if (string.IsNullOrWhiteSpace(statsDConfiguration.Geneva.Namespace))
                    {
                        errorMessages.Add("Namespace of Geneva is missing for StatsD formatter");
                    }
                }                
            }

            return errorMessages.Any() ? ValidationResult.Failure(ComponentName, errorMessages) : ValidationResult.Successful(ComponentName);
        }
    }
}