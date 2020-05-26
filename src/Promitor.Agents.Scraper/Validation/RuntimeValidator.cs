using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Agents.Scraper.Validation.Exceptions;
using Promitor.Agents.Scraper.Validation.Interfaces;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Agents.Scraper.Validation.Steps.Sinks;
using Promitor.Core.Scraping.Configuration.Runtime;

#pragma warning disable 618
namespace Promitor.Agents.Scraper.Validation
{
    public class RuntimeValidator
    {
        private readonly ILogger _validationLogger;
        private readonly List<IValidationStep> _validationSteps;

        public RuntimeValidator(
            IOptions<ScraperRuntimeConfiguration> runtimeConfiguration,
            IOptions<MetricsConfiguration> metricsConfiguration,
            ILogger<RuntimeValidator> validatorLogger,
            IMetricsDeclarationProvider scrapeConfigurationProvider,
            IConfiguration configuration)
        {
            _validationLogger = validatorLogger;

            _validationSteps = new List<IValidationStep>
            {
                new ConfigurationPathValidationStep(metricsConfiguration, _validationLogger),
                new AzureAuthenticationValidationStep(configuration, _validationLogger),
                new MetricsDeclarationValidationStep(scrapeConfigurationProvider,  _validationLogger),
                new ResourceDiscoveryValidationStep(runtimeConfiguration.Value.ResourceDiscovery,  _validationLogger),
                new StatsDMetricSinkValidationStep(runtimeConfiguration,  _validationLogger),
                new PrometheusScrapingEndpointMetricSinkValidationStep(runtimeConfiguration,  _validationLogger)
            };
        }

        public void Run()
        {
            _validationLogger.LogInformation("Starting validation of Promitor setup");

            var validationResults = RunValidationSteps();
            ProcessValidationResults(validationResults);
        }

        private void ProcessValidationResults(List<ValidationResult> validationResults)
        {
            var failedValidationResults = validationResults.Where(result => result.IsSuccessful == false).ToList();

            var validationFailed = failedValidationResults.Any();
            if (validationFailed)
            {
                _validationLogger.LogCritical("Promitor is not configured correctly. Please fix validation issues and re-run.");
                throw new ValidationFailedException(failedValidationResults);
            }

            _validationLogger.LogInformation("Promitor configuration is valid, we are good to go.");
        }

        private List<ValidationResult> RunValidationSteps()
        {
            if (_validationSteps == null)
            {
                return Enumerable.Empty<ValidationResult>().ToList();
            }

            var totalValidationSteps = _validationSteps.Count;
            var validationResults = new List<ValidationResult>();

            for (var currentValidationStep = 1; currentValidationStep <= totalValidationSteps; currentValidationStep++)
            {
                var validationStep = _validationSteps[currentValidationStep - 1];
                var validationResult = RunValidationStep(validationStep, currentValidationStep, totalValidationSteps);
                validationResults.Add(validationResult);
            }

            return validationResults;
        }

        private ValidationResult RunValidationStep(IValidationStep validationStep, int currentStep, int totalSteps)
        {
            _validationLogger.LogInformation("Start Validation step {currentStep}/{totalSteps}: {validationStepName}", currentStep, totalSteps, validationStep.ComponentName);

            var validationResult = validationStep.Run();
            if (validationResult.IsSuccessful)
            {
                _validationLogger.LogInformation("Validation step {currentStep}/{totalSteps} succeeded", currentStep, totalSteps);
            }
            else
            {
                _validationLogger.LogWarning("Validation step {currentStep}/{totalSteps} failed. Error(s): {validationMessage}", currentStep, totalSteps, validationResult.Message);
            }

            return validationResult;
        }
    }
}