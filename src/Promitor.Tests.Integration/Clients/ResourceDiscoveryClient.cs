using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;

namespace Promitor.Tests.Integration.Clients
{
    public class ResourceDiscoveryClient : AgentClient
    {
        public ResourceDiscoveryClient(IConfiguration configuration, ILogger logger)
            : base("Resource Discovery", "Agents:ResourceDiscovery:BaseUrl", configuration, logger)
        {
        }

        public async Task<HttpResponseMessage> GetResourceDiscoveryGroupsWithResponseAsync()
        {
            return await GetAsync("/api/v1/resources/groups");
        }

        public async Task<HttpResponseMessage> GetDiscoveredResourcesWithResponseAsync(string resourceDiscoveryGroupName)
        {
            return await GetAsync($"/api/v1/resources/groups/{resourceDiscoveryGroupName}/discover");
        }

        public async Task<List<AzureResourceDefinition>> GetDiscoveredResourcesAsync(string resourceDiscoveryGroupName)
        {
            var response = await GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);
            var rawResponse = await response.Content.ReadAsStringAsync();

            return GetDeserializedResponse<List<AzureResourceDefinition>>(rawResponse);
        }
    }
}