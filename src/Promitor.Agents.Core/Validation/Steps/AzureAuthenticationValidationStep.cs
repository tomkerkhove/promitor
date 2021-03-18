using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Core;
using Promitor.Integrations.Azure.Authentication;
using Promitor.Integrations.Azure.Authentication.Configuration;

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
            var authenticationConfiguration = _configuration.GetSection("authentication").Get<AuthenticationConfiguration>();

            // To be still compatible with existing infrastructure using previous version of Promitor, we need to check if the authentication section exists.
            // If not, we should use a default value
            if (authenticationConfiguration == null)
            {
                Logger.LogWarning($"Promitor needs an authentication mode. You can choose ServicePrincipal or UserAssignedManagedIdentity or SystemAssignedManagedIdentity. Since no values has been specified, ServicePrincipal will be used by Promitor to authenticate to Azure");
                authenticationConfiguration = new AuthenticationConfiguration();
            }

            // TODO: Simplify
            switch (authenticationConfiguration.Mode)
            {
                case AuthenticationMode.ServicePrincipal:
                    var applicationId = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
                    if (string.IsNullOrWhiteSpace(applicationId))
                    {
                        return ValidationResult.Failure(ComponentName, "No service principal application id was specified for Azure authentication");
                    }

                    var applicationKey = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);
                    if (string.IsNullOrWhiteSpace(applicationKey))
                    { 
                        return ValidationResult.Failure(ComponentName, "No service principal application key was specified for Azure authentication");
                    }

                    Logger.LogInformation($"Promitor is configured to use a service principal (key:{applicationId})");

                    break;
                case AuthenticationMode.UserAssignedManagedIdentity:
                    var managedIdentityId = authenticationConfiguration.IdentityId;
                    if (string.IsNullOrWhiteSpace(managedIdentityId))
                    {
                        return ValidationResult.Failure(ComponentName, "No user-assigned managed identity id was specified for Azure authentication");
                    }

                    Logger.LogInformation($"Promitor is configured to use a user managed identity (key:{managedIdentityId})");
                    break;
                case AuthenticationMode.SystemAssignedManagedIdentity:
                    Logger.LogInformation("Promitor configured to use a system assigned identity");
                    break;
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}