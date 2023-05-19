using System.IO;
using System.Security.Authentication;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using GuardNet;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Promitor.Core;
using Promitor.Integrations.Azure.Authentication.Configuration;

namespace Promitor.Integrations.Azure.Authentication
{
    public class AzureAuthenticationFactory
    {
        /// <summary>
        /// Get the configured authentication info for Microsoft Azure
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        public static AzureAuthenticationInfo GetConfiguredAzureAuthentication(IConfiguration configuration)
        {
            // To be still compatible with existing infrastructure using previous version of Promitor, we need to check if the authentication section exists.
            // If not, we should use a default value
            var authenticationConfiguration = configuration.GetSection("authentication").Get<AuthenticationConfiguration>() ?? new AuthenticationConfiguration();

            string applicationKey;

            if (!string.IsNullOrWhiteSpace(authenticationConfiguration.SecretFilePath) && !string.IsNullOrWhiteSpace(authenticationConfiguration.SecretFileName))
            {
                var filePath = Path.Combine(authenticationConfiguration.SecretFilePath, authenticationConfiguration.SecretFileName);

                if (!File.Exists(filePath))
                {
                    throw new AuthenticationException("Invalid secret file path was configured for service principle authentication because it does not exist");
                }
                
                applicationKey = File.ReadAllText(filePath);
            }
            else
            {
                applicationKey = configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);
            }

            string identityId = authenticationConfiguration.IdentityId;
            if (authenticationConfiguration.Mode == AuthenticationMode.ServicePrincipal)
            {
                if (string.IsNullOrWhiteSpace(identityId))
                {
                    // Use environment variable for backwards compatibility
                    identityId = configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
                }

                if (string.IsNullOrWhiteSpace(identityId))
                {
                    throw new AuthenticationException("No identity was configured for service principle authentication");
                }

                if (string.IsNullOrWhiteSpace(applicationKey))
                {
                    throw new AuthenticationException("No identity secret was configured for service principle authentication");
                }
            }
            
            return new AzureAuthenticationInfo
            {
                Mode = authenticationConfiguration.Mode,
                IdentityId = identityId,
                Secret = applicationKey
            };
        }

        public static TokenCredential GetTokenCredential(string resource, string tenantId, AzureAuthenticationInfo authenticationInfo, System.Uri azureAuthorityHost)
        {
            Guard.NotNullOrWhitespace(resource, nameof(resource));
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNull(authenticationInfo, nameof(authenticationInfo));

            TokenCredential tokenCredential;

            var tokenCredentialOptions = new TokenCredentialOptions { AuthorityHost = azureAuthorityHost };

            switch (authenticationInfo.Mode)
            {
                case AuthenticationMode.ServicePrincipal:
                    tokenCredential = new ClientSecretCredential(tenantId, authenticationInfo.IdentityId, authenticationInfo.Secret, tokenCredentialOptions);
                    break;
                case AuthenticationMode.UserAssignedManagedIdentity:
                    var clientId = authenticationInfo.GetIdentityIdOrDefault();
                    tokenCredential = new ManagedIdentityCredential(clientId, tokenCredentialOptions);
                    break;
                case AuthenticationMode.SystemAssignedManagedIdentity:
                    tokenCredential = new ManagedIdentityCredential(options:tokenCredentialOptions);
                    break;
                default:
                    tokenCredential = new DefaultAzureCredential();
                    break;
            }

            return tokenCredential;
        }

        /// <summary>
        ///     Gets a valid token using a Service Principal or a Managed Identity
        /// </summary>
        public static async Task<TokenCredentials> GetTokenCredentialsAsync(string resource, string tenantId, AzureAuthenticationInfo authenticationInfo, System.Uri azureAuthorityHost)
        {
            var tokenCredential = GetTokenCredential(resource, tenantId, authenticationInfo, azureAuthorityHost);

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

        public static AzureCredentials CreateAzureAuthentication(AzureEnvironment azureCloud, string tenantId, AzureAuthenticationInfo azureAuthenticationInfo, AzureCredentialsFactory azureCredentialsFactory)
        {
            AzureCredentials credentials;

            switch (azureAuthenticationInfo.Mode)
            {
                case AuthenticationMode.ServicePrincipal:
                    credentials = GetServicePrincipleCredentials(azureCloud, tenantId, azureAuthenticationInfo, azureCredentialsFactory);
                    break;
                case AuthenticationMode.UserAssignedManagedIdentity:
                    credentials = GetUserAssignedManagedIdentityCredentials(azureCloud, tenantId, azureAuthenticationInfo, azureCredentialsFactory);
                    break;
                default:
                    credentials = GetSystemAssignedManagedIdentityCredentials(azureCloud, tenantId, azureCredentialsFactory);
                    break;
            }

            return credentials;
        }

        private static AzureCredentials GetSystemAssignedManagedIdentityCredentials(AzureEnvironment azureCloud, string tenantId, AzureCredentialsFactory azureCredentialsFactory)
        {
            return azureCredentialsFactory.FromSystemAssignedManagedServiceIdentity(MSIResourceType.VirtualMachine, azureCloud, tenantId);
        }

        private static AzureCredentials GetServicePrincipleCredentials(AzureEnvironment azureCloud, string tenantId, AzureAuthenticationInfo azureAuthenticationInfo, AzureCredentialsFactory azureCredentialsFactory)
        {
            if (string.IsNullOrWhiteSpace(azureAuthenticationInfo.IdentityId))
            {
                throw new AuthenticationException("No identity was configured for service principle authentication");
            }

            if (string.IsNullOrWhiteSpace(azureAuthenticationInfo.Secret))
            {
                throw new AuthenticationException("No identity was configured for service principle authentication");
            }

            return azureCredentialsFactory.FromServicePrincipal(azureAuthenticationInfo.IdentityId, azureAuthenticationInfo.Secret, tenantId, azureCloud);
        }

        private static AzureCredentials GetUserAssignedManagedIdentityCredentials(AzureEnvironment azureCloud, string tenantId, AzureAuthenticationInfo azureAuthenticationInfo, AzureCredentialsFactory azureCredentialsFactory)
        {
            var clientId = azureAuthenticationInfo.GetIdentityIdOrDefault();
            return azureCredentialsFactory.FromUserAssigedManagedServiceIdentity(clientId, MSIResourceType.VirtualMachine, azureCloud, tenantId);
        }
    }
}
