using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public static class Authentication
    {
        public static async Task<ServiceClientCredentials> GetServiceClientCredentialsAsync(string resource, string clientId, string clientSecret, string tenantId)
        {
            AuthenticationContext authContext = new AuthenticationContext($"https://login.microsoftonline.com/{tenantId}");

            AuthenticationResult authResult = await authContext.AcquireTokenAsync(resource, new ClientCredential(clientId, clientSecret));

            ServiceClientCredentials serviceClientCredentials = new TokenCredentials(authResult.AccessToken);

            return serviceClientCredentials;
        }
    }
}
