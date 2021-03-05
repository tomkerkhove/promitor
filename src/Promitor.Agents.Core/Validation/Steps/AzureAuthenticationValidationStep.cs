using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Server;
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
            var serverConfiguration = _configuration.GetSection("server").Get<ServerConfiguration>();

            Guard.NotNull(serverConfiguration, nameof(serverConfiguration));

            if (serverConfiguration.Authentication == AuthenticationMode.ManagedIdentity)
            {
                var managedIdentityId = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ManagedIdentityId);
                this.Logger.LogInformation("Promitor configured to use a managed identity");

                if (!string.IsNullOrWhiteSpace(managedIdentityId))
                    this.Logger.LogInformation($"Promitor will use a user managed identity id:{managedIdentityId}");
                else
                    this.Logger.LogInformation($"Promitor will use the system assigned identity");
            }
            else if (serverConfiguration.Authentication == AuthenticationMode.ServicePrincipal)
            {
                var applicationId = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
                this.Logger.LogInformation("Promitor configured to use a service principal");
                this.Logger.LogInformation($"Promitor will use the service principal id:{applicationId}");

                if (string.IsNullOrWhiteSpace(applicationId))
                    return ValidationResult.Failure(ComponentName, "No service principal application id was specified for Azure authentication");

                var applicationKey = _configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);
                if (string.IsNullOrWhiteSpace(applicationKey))
                    return ValidationResult.Failure(ComponentName, "No service principal application key was specified for Azure authentication");
            }
            else
            {
                return ValidationResult.Failure(ComponentName, "Mode used for authentication in server configuration is not valid. Valid values are: 'ServicePrincipal' or 'ManagedIdentity'");
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}