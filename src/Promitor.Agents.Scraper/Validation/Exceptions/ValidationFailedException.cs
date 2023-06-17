using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Promitor.Agents.Core.Validation;

// ReSharper disable CollectionNeverQueried.Global
namespace Promitor.Agents.Scraper.Validation.Exceptions
{
    public class ValidationFailedException : Exception
    {
        public ValidationFailedException(List<ValidationResult> validationResults)
            : base($"Validation Failed. Errors:{ListErrors(validationResults)}")
        {
            ValidationResults.AddRange(validationResults);
        }

        public List<ValidationResult> ValidationResults { get; } = new();

        private static string ListErrors(List<ValidationResult> validationResults)
        {
            var errorBuilder = new StringBuilder();

            foreach (var validationResult in validationResults.Where(result => result.IsSuccessful == false))
            {
                errorBuilder.AppendLine($"- {validationResult.ComponentName}: {validationResult.Message}");
            }

            return errorBuilder.ToString();
        }
    }
}