using System;
using System.Collections.Generic;
using System.Linq;
using Promitor.Scraper.Host.Validation.Exceptions;
using Promitor.Scraper.Host.Validation.Interfaces;

namespace Promitor.Scraper.Host.Validation
{
    public static class Validator
    {
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
            Console.WriteLine(Constants.Texts.Validation.SeperationText);
            Console.WriteLine(Constants.Texts.Validation.StartValidationText);
        }

        private static void OutputValidationResult(bool isValidationSuccessful)
        {
            var generalValidationOutcome = isValidationSuccessful
                ? Constants.Texts.Validation.GeneralValidationFailed
                : Constants.Texts.Validation.GeneralValidationSucceeded;

            Console.WriteLine(generalValidationOutcome);
            Console.WriteLine(Constants.Texts.Validation.SeperationText);
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
                var validationResult = RunValidationStep(validationStep, currentValidationStep, totalValidationSteps);
                validationResults.Add(validationResult);
            }

            return validationResults;
        }

        private static ValidationResult RunValidationStep(IValidationStep validationStep, int currentStep,
            int totalSteps)
        {
            Console.WriteLine($"\tValidation step {currentStep}/{totalSteps}: {validationStep.ComponentName}");

            var validationResult = validationStep.Run();
            var validationOutcome = validationResult.IsSuccessful
                ? Constants.Texts.Validation.ValidationStepSuccessfulText
                : $"\tValidation step failed. Error(s): {validationResult.Message}";
            Console.WriteLine(validationOutcome);

            return validationResult;
        }
    }
}