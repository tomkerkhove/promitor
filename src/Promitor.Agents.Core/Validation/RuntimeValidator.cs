using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Validation.Interfaces;

#pragma warning disable 618
namespace Promitor.Agents.Core.Validation
{
    public class RuntimeValidator
    {
        private readonly ILogger _validationLogger;
        private readonly List<IValidationStep> _validationSteps;

        public RuntimeValidator(IEnumerable<IValidationStep> validationSteps,
            ILogger<RuntimeValidator> validatorLogger)
        {
            _validationLogger = validatorLogger;
            _validationSteps = validationSteps.ToList();
        }

        /// <summary>
        /// Checks whether Promitor's configuration is valid so that the application
        /// can start running successfully.
        /// </summary>
        /// <returns>
        /// true if the configuration is valid, false otherwise.
        /// </returns>
        public bool Validate()
        {
            _validationLogger.LogInformation("Starting validation of Promitor setup");

            var validationResults = RunValidationSteps();

            return validationResults.All(result => result.IsSuccessful);
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