using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core;
using Promitor.Scraper.Host.Validation.Interfaces;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class AzureAuthenticationValidationStep : ValidationStep, IValidationStep
    {
        public string ComponentName { get; } = "Azure Authentication";

        public AzureAuthenticationValidationStep(IConfiguration configuration) : base(configuration, NullLogger.Instance)
        {
        }

        public AzureAuthenticationValidationStep(IConfiguration configuration, ILogger logger) : base(configuration, logger)
        {
        }

        public ValidationResult Run()
        {
            
            var applicationId = Configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                return ValidationResult.Failure(ComponentName, "No application id was specified for Azure authentication");
            }

            var applicationKey = Configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);
            if (string.IsNullOrWhiteSpace(applicationKey))
            {
                return ValidationResult.Failure(ComponentName, "No application key was specified for Azure authentication");
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}