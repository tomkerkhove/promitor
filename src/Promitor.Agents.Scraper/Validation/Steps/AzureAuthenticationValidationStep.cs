using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core;
using Promitor.Agents.Scraper.Validation.Interfaces;

namespace Promitor.Agents.Scraper.Validation.Steps
{
    public class AzureAuthenticationValidationStep : ValidationStep, IValidationStep
    {
        private readonly IConfiguration _configuration;

        public string ComponentName { get; } = "Azure Authentication";

        public AzureAuthenticationValidationStep(IConfiguration configuration) : this(configuration, NullLogger.Instance)
        {
        }

        public AzureAuthenticationValidationStep(IConfiguration configuration, ILogger logger) : base(logger)
        {
            _configuration = configuration;
        }

        public ValidationResult Run()
        {
            var applicationId = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                return ValidationResult.Failure(ComponentName, "No application id was specified for Azure authentication");
            }

            var applicationKey = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);
            if (string.IsNullOrWhiteSpace(applicationKey))
            {
                return ValidationResult.Failure(ComponentName, "No application key was specified for Azure authentication");
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}