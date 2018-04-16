using System;
using System.Collections.Generic;
using System.Linq;
using Promitor.Scraper.Validation.Exceptions;
using Promitor.Scraper.Validation.Interfaces;

namespace Promitor.Scraper.Validation
{
    public static class Validator
    {
        private const string SeperationText = "-------------------------------------------------";
        private const string StartValidationText = "Starting validation of configuration...";
        private const string ValidationStepSuccessfulText = "\tValidation step succeeded.";

        private const string GeneralValidationFailed =
            "Configuration is invalid. Please fix them before running Promitor again";

        private const string GeneralValidationSucceeded = "Configuration is valid.";

        public static void Run(List<IValidationStep> validationSteps)
        {
            if (validationSteps == null)
            {
                throw new Exception("No validation steps were specified.");
            }

            OutputValidationIntroduction();

            var validationResults = RunValidationSteps(validationSteps);
            ProcessValidationResults(validationResults);
        }

        private static void ProcessValidationResults(List<ValidationResult> validationResults)
        {
            var failedValidationResults = validationResults.Where(result => result.IsSuccessful == false).ToList();

            var validationFailed = failedValidationResults.Any();
            OutputValidationResult(validationFailed);

            if (validationFailed)
            {
                throw new ValidationFailedException(failedValidationResults);
            }
        }

        private static void OutputValidationIntroduction()
        {
            Console.WriteLine(SeperationText);
            Console.WriteLine(StartValidationText);
        }

        private static void OutputValidationResult(bool isValidationSuccessful)
        {
            var generalValidationOutcome = isValidationSuccessful
                ? GeneralValidationFailed
                : GeneralValidationSucceeded;

            Console.WriteLine(generalValidationOutcome);
            Console.WriteLine(SeperationText);
        }

        private static List<ValidationResult> RunValidationSteps(List<IValidationStep> validationSteps)
        {
            if (validationSteps == null)
            {
                return Enumerable.Empty<ValidationResult>().ToList();
            }

            var totalValidationSteps = validationSteps.Count;
            var validationResults = new List<ValidationResult>();

            for (var currentValidationStep = 1; currentValidationStep <= totalValidationSteps; currentValidationStep++)
            {
                var validationStep = validationSteps[currentValidationStep - 1];
                var validationResult = ValidateStep(validationStep, currentValidationStep, totalValidationSteps);
                validationResults.Add(validationResult);
            }

            return validationResults;
        }

        private static ValidationResult ValidateStep(IValidationStep validationStep, int currentStep, int totalSteps)
        {
            Console.WriteLine($"\tValidation step {currentStep}/{totalSteps}: {validationStep.ComponentName}");

            var validationResult = validationStep.Validate();
            var validationOutcome = validationResult.IsSuccessful
                ? ValidationStepSuccessfulText
                : $"\tValidation step failed. Error(s): {validationResult.Message}";
            Console.WriteLine(validationOutcome);

            return validationResult;
        }
    }
}