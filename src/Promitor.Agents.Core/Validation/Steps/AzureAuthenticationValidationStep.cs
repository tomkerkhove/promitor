using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Core;

namespace Promitor.Agents.Core.Validation.Steps
{
    public class AzureAuthenticationValidationStep : ValidationStep, IValidationStep
    {
        private readonly IConfiguration _configuration;

        public AzureAuthenticationValidationStep(IConfiguration configuration, ILogger<AzureAuthenticationValidationStep> logger) : base(logger)
        {
            Guard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        public string ComponentName { get; } = "Azure Authentication";

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