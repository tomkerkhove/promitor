using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Validation.Interfaces;
using Spectre.Console;

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
            
            // Create a table
            var asciiTable = CreateAsciiTable();

            for (var currentValidationStep = 1; currentValidationStep <= totalValidationSteps; currentValidationStep++)
            {
                var validationStep = _validationSteps[currentValidationStep - 1];
                var validationResult = RunValidationStep(validationStep, asciiTable);
                validationResults.Add(validationResult);
            }

            AnsiConsole.Render(asciiTable);

            return validationResults;
        }

        private static Table CreateAsciiTable()
        {
            var asciiTable = new Table
            {
                Border = TableBorder.HeavyEdge
            };

            // Add some columns
            asciiTable.AddColumn("Name");
            asciiTable.AddColumn("Outcome");
            asciiTable.AddColumn("Details");
            asciiTable.Caption("Validation");

            return asciiTable;
        }

        private ValidationResult RunValidationStep(IValidationStep validationStep, Table asciiTable)
        {
            var validationResult = validationStep.Run();
            if (validationResult.IsSuccessful)
            {
                asciiTable.AddRow(validationStep.ComponentName, "Success", "Everything is well-configured.");
            }
            else
            {
                asciiTable.AddRow(validationStep.ComponentName, "Failed", $"Validation failed:\r\n{validationResult.Message}");
            }

            return validationResult;
        }
    }
}