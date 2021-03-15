using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Rest;
using Promitor.Agents.Core.Configuration.Authentication;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public static class Authentication
    {
        /// <summary>
        /// Gets a valid token using a Service Principal or a Managed Identity
        /// </summary>
        public static async Task<TokenCredentials> GetTokenCredentialsAsync(string resource, string tenantId, AuthenticationMode authenticationMode, string managedIdentityId = null, string clientId = null, string clientSecret = null)
        {
            TokenCredential tokenCredential;

            switch (authenticationMode)
            {
                case AuthenticationMode.ServicePrincipal:
                    tokenCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                    break;
                case AuthenticationMode.UserAssignedManagedIdentity:
                    tokenCredential = new ManagedIdentityCredential(managedIdentityId);
                    break;
                case AuthenticationMode.SystemAssignedManagedIdentity:
                default:
                    tokenCredential = new DefaultAzureCredential();
                    break;
            }

            //When you reaching an endpoint, using an impersonate identity, the only endpoint available is always '/.default'
            //MSAL add the './default' string to your resource request behind the scene.
            //We have to do it here, since we are at a lower level(and we are not using MSAL; by the way)
            if (!resource.ToLowerInvariant().EndsWith("/.default"))
            {
                if (!resource.EndsWith("/"))
                    resource += "/";

                resource += ".default";
            }

            var accessToken = await tokenCredential.GetTokenAsync(new TokenRequestContext(scopes: new [] { resource }), default);

            return new TokenCredentials(accessToken.Token);
        }
    }
}
