using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Promitor.Agents.Core.Configuration.Server;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public static class Authentication
    {
        //public static async Task<ServiceClientCredentials> GetServiceClientCredentialsAsync(string resource, string clientId, string clientSecret, string tenantId)
        //{
        //    AuthenticationContext authContext = new AuthenticationContext($"https://login.microsoftonline.com/{tenantId}");

        //    AuthenticationResult authResult = await authContext.AcquireTokenAsync(resource, new ClientCredential(clientId, clientSecret));

        //    ServiceClientCredentials serviceClientCredentials = new TokenCredentials(authResult.AccessToken);

        //    return serviceClientCredentials;
        //}

        /// <summary>
        /// Gets a valid token using a Service Principal or a Managed Identity
        /// </summary>
        public static async Task<TokenCredentials> GetTokenCredentialsAsync(string resource, string tenantId, AuthenticationMode authenticationMode, string managedIdentityId = null, string clientId = null, string clientSecret = null)
        {
            TokenCredential tokenCredential;

            if (authenticationMode == AuthenticationMode.ServicePrincipal)
            {
                tokenCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            }
            else if (!string.IsNullOrEmpty(managedIdentityId))
            {
                tokenCredential = new ManagedIdentityCredential(managedIdentityId);
            }
            else
            {
                tokenCredential = new DefaultAzureCredential();
            }

            if (!resource.ToLowerInvariant().EndsWith("/.default"))
            {
                if (!resource.EndsWith("/"))
                    resource += "/";

                resource += ".default";
            }

            var accessToken = await tokenCredential.GetTokenAsync(new TokenRequestContext(scopes: new string[] { resource }), default);

            return new TokenCredentials(accessToken.Token);
        }
    }
}
