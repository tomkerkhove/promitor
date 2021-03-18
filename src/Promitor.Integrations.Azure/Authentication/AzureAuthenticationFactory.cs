using System.Security.Authentication;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using GuardNet;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Promitor.Integrations.Azure.Authentication.Configuration;

namespace Promitor.Integrations.Azure.Authentication
{
    public class AzureAuthenticationFactory
    {
        public static AzureAuthenticationInfo GetConfiguredAzureAuthentication(IConfiguration configuration)
        {
            var authenticationConfiguration = configuration.GetSection("authentication").Get<AuthenticationConfiguration>();

            // To be still compatible with existing infrastructure using previous version of Promitor, we need to check if the authentication section exists.
            // If not, we should use a default value
            if (authenticationConfiguration == null)
            {
                authenticationConfiguration = new AuthenticationConfiguration();
            }

            var applicationId = configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
            var applicationKey = configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);
            var identityId = authenticationConfiguration.IdentityId;

            return new AzureAuthenticationInfo
            {
                Mode = authenticationConfiguration.Mode,
                IdentityId = managedIdentityId,
                IdentityId = applicationId,
                Secret = applicationKey
            };
        }

        public AzureCredentials CreateAzureAuthentication(AzureEnvironment azureCloud, string tenantId, AzureAuthenticationConfiguration azureCredentials)
        {
            AzureCredentials credentials;

            switch (azureCredentials.AuthenticationMode)
            {
                case AuthenticationMode.ServicePrincipal:
                    if (string.IsNullOrWhiteSpace(azureCredentials.IdentityId))
                    {
                        throw new AuthenticationException("No identity was configured for service principle authentication");
                    }
                    if (string.IsNullOrWhiteSpace(azureCredentials.Secret))
                    {
                        throw new AuthenticationException("No identity was configured for service principle authentication");
                    }
                    credentials = _azureCredentialsFactory.FromServicePrincipal(azureCredentials.IdentityId, azureCredentials.Secret, tenantId, azureCloud);
                    break;
                case AuthenticationMode.UserAssignedManagedIdentity:
                    Guard.NotNullOrWhitespace(managedIdentityId, nameof(managedIdentityId));
                    credentials = _azureCredentialsFactory.FromUserAssigedManagedServiceIdentity(managedIdentityId, MSIResourceType.VirtualMachine, azureCloud, tenantId);
                    break;
                default:
                    credentials = _azureCredentialsFactory.FromSystemAssignedManagedServiceIdentity(MSIResourceType.VirtualMachine, azureCloud, tenantId);
                    break;
            }

            return credentials;
        }

        /// <summary>
        ///     Gets a valid token using a Service Principal or a Managed Identity
        /// </summary>
        public async Task<TokenCredentials> GetTokenCredentialsAsync(string resource, string tenantId, AzureAuthenticationInfo authenticationInfo)
        {
            Guard.NotNullOrWhitespace(resource, nameof(resource));
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNull(authenticationInfo, nameof(authenticationInfo));

            TokenCredential tokenCredential;

            switch (authenticationInfo.Mode)
            {
                case AuthenticationMode.ServicePrincipal:
                    tokenCredential = new ClientSecretCredential(tenantId, authenticationInfo.IdentityId, authenticationInfo.Secret);
                    break;
                case AuthenticationMode.UserAssignedManagedIdentity:
                    tokenCredential = new ManagedIdentityCredential(authenticationInfo.IdentityId);
                    break;
                case AuthenticationMode.SystemAssignedManagedIdentity:
                    tokenCredential = new ManagedIdentityCredential();
                    break;
                default:
                    tokenCredential = new DefaultAzureCredential();
                    break;
            }

            // When you reaching an endpoint, using an impersonate identity, the only endpoint available is always '/.default'
            // MSAL add the './default' string to your resource request behind the scene.
            // We have to do it here, since we are at a lower level(and we are not using MSAL; by the way)
            if (!resource.ToLowerInvariant().EndsWith("/.default"))
            {
                if (!resource.EndsWith("/"))
                {
                    resource += "/";
                }

                resource += ".default";
            }

            var accessToken = await tokenCredential.GetTokenAsync(new TokenRequestContext(new[] { resource }), default);

            return new TokenCredentials(accessToken.Token);
        }
    }
}
