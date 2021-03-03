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
            var useManagedIdentity = _configuration.GetValue<string>(EnvironmentVariables.Authentication.UseManagedIdentity, "0") == "1";

            if (useManagedIdentity)
            {
                var managedIdentityId = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ManagedIdentityId);
                this.Logger.LogInformation("Promitor configured to use a managed identity");

                if (!string.IsNullOrWhiteSpace(managedIdentityId))
                    this.Logger.LogInformation($"Promitor will use a user managed identity id:{managedIdentityId}");
                else
                    this.Logger.LogInformation($"Promitor will use the system assigned identity");
            }
            else
            {
                var applicationId = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
                this.Logger.LogInformation("Promitor configured to use a service principal");
                this.Logger.LogInformation($"Promitor service principal id:{applicationId}");

                if (string.IsNullOrWhiteSpace(applicationId))
                    return ValidationResult.Failure(ComponentName, "No application id was specified for Azure authentication");

                var applicationKey = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);
                if (string.IsNullOrWhiteSpace(applicationKey))
                    return ValidationResult.Failure(ComponentName, "No application key was specified for Azure authentication");
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}