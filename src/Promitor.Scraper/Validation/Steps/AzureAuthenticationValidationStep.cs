using System;
using Promitor.Scraper.Configuration;
using Promitor.Scraper.Validation.Interfaces;

namespace Promitor.Scraper.Validation.Steps
{
    public class AzureAuthenticationValidationStep : ValidationStep, IValidationStep
    {
        public string ComponentName { get; } = "Azure Authentication";

        public ValidationResult Run()
        {
            var applicationId = Environment.GetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationId);
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                var errorMessage = "No application id was specified for Azure authentication";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            var applicationKey = Environment.GetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationKey);
            if (string.IsNullOrWhiteSpace(applicationKey))
            {
                var errorMessage = "No application key was specified for Azure authentication";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}