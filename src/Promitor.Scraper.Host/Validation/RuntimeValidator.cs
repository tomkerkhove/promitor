using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Scraper.Host.Validation.Exceptions;
using Promitor.Scraper.Host.Validation.Interfaces;
using Promitor.Scraper.Host.Validation.Steps;

namespace Promitor.Scraper.Host.Validation
{
    public class RuntimeValidator
    {
        private readonly ILogger _validationLogger;
        private readonly List<IValidationStep> _validationSteps;

        public RuntimeValidator()
        {
            _validationLogger = new ConsoleLogger("Validation", (message, logLevel) => true, includeScopes: true);

            var scrapeConfigurationProvider = new MetricsDeclarationProvider();
            _validationSteps = new List<IValidationStep>
            {
                new ConfigurationPathValidationStep(_validationLogger),
                new ScrapingScheduleValidationStep(_validationLogger),
                new AzureAuthenticationValidationStep(_validationLogger),
                new MetricsDeclarationValidationStep(scrapeConfigurationProvider,_validationLogger)
            };
        }

        public void Run()
        {
            var validationLogger = new ConsoleLogger("Validation", (message, logLevel) => true, includeScopes: true);
            validationLogger.LogInformation("Starting validation of Promitor setup");

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
            _validationLogger.LogInformation("Validation step {currentStep}/{totalSteps}: {validationStepName}", currentStep, totalSteps, validationStep.ComponentName);

            var validationResult = validationStep.Run();
            if (validationResult.IsSuccessful)
            {
                _validationLogger.LogInformation("Validation step succeeded");
            }
            else
            {
                _validationLogger.LogWarning("Validation step failed. Error(s): {validationMessage}", validationResult.Message);
            }

            return validationResult;
        }
    }
}