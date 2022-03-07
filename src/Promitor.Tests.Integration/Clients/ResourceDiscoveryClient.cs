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

        public async Task<HttpResponseMessage> GetDiscoveredResourcesWithResponseAsync(string apiVersion, string resourceDiscoveryGroupName, int currentPage = 1, int pageSize = 1000)
        {
            return await GetAsync($"/api/{apiVersion}/resources/groups/{resourceDiscoveryGroupName}/discover?currentPage={currentPage}&pageSize={pageSize}");
        }

        public async Task<HttpResponseMessage> GetDiscoveredResourcesV1WithResponseAsync(string resourceDiscoveryGroupName, int currentPage = 1, int pageSize = 1000)
        {
            return await GetDiscoveredResourcesWithResponseAsync("v1", resourceDiscoveryGroupName, currentPage, pageSize);
        }

        public async Task<HttpResponseMessage> GetDiscoveredResourcesV2WithResponseAsync(string resourceDiscoveryGroupName, int currentPage = 1, int pageSize = 1000)
        {
            return await GetDiscoveredResourcesWithResponseAsync("v2", resourceDiscoveryGroupName, currentPage, pageSize);
        }

        public async Task<List<AzureResourceDefinition>> GetAllDiscoveredResourcesAsync(string resourceDiscoveryGroupName)
        {
            PagedPayload<AzureResourceDefinition> pagedPayload;
            var results = new List<AzureResourceDefinition>();
            var currentPage = 1;

            do
            {
                var response = await GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName, currentPage);
                var rawResponse = await response.Content.ReadAsStringAsync();

                pagedPayload = GetDeserializedResponse<PagedPayload<AzureResourceDefinition>>(rawResponse);
                results.AddRange(pagedPayload.Result);
                currentPage++;
            }
            while (pagedPayload.HasMore);

            return results;
        }
    }
}