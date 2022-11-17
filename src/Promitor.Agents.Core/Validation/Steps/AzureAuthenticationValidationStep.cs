using System.Security.Authentication;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Integrations.Azure.Authentication;

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
            try
            {
                AzureAuthenticationFactory.GetConfiguredAzureAuthentication(_configuration);
                
                return ValidationResult.Successful(ComponentName);
            }
            catch (AuthenticationException authenticationException)
            {
                return ValidationResult.Failure(ComponentName, $"Azure authentication is not configured correctly - {authenticationException.Message}");
            }
        }
    }
}