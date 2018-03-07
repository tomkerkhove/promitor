using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Promitor.Scraper.Validation.Interfaces;

namespace Promitor.Scraper.Validation
{
    public static class Validator
    {
        public static void Run(IEnumerable<IValidationStep> validationSteps)
        {
            if (validationSteps == null)
            {
                throw new Exception("No validation steps were specified.");
            }

            var validationResults = new List<ValidationResult>();

            foreach (var validationStep in validationSteps)
            {
                var validationResult = validationStep.Validate();
                validationResults.Add(validationResult);
            }

            if (validationResults.Any(result => result.IsSuccessful == false))
            {
                var validationOutputBuilder = new StringBuilder();
                validationOutputBuilder.AppendLine("Configuration validation failed.");
                validationOutputBuilder.AppendLine("Details:");

                foreach (var validationFailure in validationResults.Where(result => result.IsSuccessful == false))
                {
                    validationOutputBuilder.AppendLine($"- {validationFailure.Message}");
                }

                throw new Exception(validationOutputBuilder.ToString());
            }
        }
    }
}