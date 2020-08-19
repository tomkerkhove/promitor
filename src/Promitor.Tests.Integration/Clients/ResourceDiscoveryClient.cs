using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Promitor.Tests.Integration.Clients
{
    public class ResourceDiscoveryClient:AgentClient
    {
        public ResourceDiscoveryClient(IConfiguration configuration, ILogger logger)
            : base("Resource Discovery", "Agents:ResourceDiscovery:BaseUrl", configuration, logger)
        {
        }

        public async Task<HttpResponseMessage> GetResourceDiscoveryGroupsAsync()
        {
            return await GetAsync("/api/v1/resources/groups");
        }

        public async Task<HttpResponseMessage> GetDiscoveredResourcesAsync(string resourceDiscoveryGroupName)
        {
            return await GetAsync($"/api/v1/resources/groups/{resourceDiscoveryGroupName}/discover");
        }
    }
}