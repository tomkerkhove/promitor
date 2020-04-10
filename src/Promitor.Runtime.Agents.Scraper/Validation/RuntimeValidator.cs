using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Runtime.Agents.Scraper.Validation.Exceptions;
using Promitor.Runtime.Agents.Scraper.Validation.Interfaces;
using Promitor.Runtime.Agents.Scraper.Validation.Steps;

#pragma warning disable 618

namespace Promitor.Runtime.Agents.Scraper.Validation
{
    public class RuntimeValidator
    {
        private readonly ILogger _validationLogger;
        private readonly List<IValidationStep> _validationSteps;

        public RuntimeValidator(
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
                new MetricsDeclarationValidationStep(scrapeConfigurationProvider,  _validationLogger)
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