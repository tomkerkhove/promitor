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

        public async Task<HttpResponseMessage> GetDiscoveredResourcesWithResponseAsync(string resourceDiscoveryGroupName, int currentPage = 1, int pageSize = 1000)
        {
            return await GetAsync($"/api/v2/resources/groups/{resourceDiscoveryGroupName}/discover?currentPage={currentPage}&pageSize={pageSize}");
        }

        public async Task<List<AzureResourceDefinition>> GetAllDiscoveredResourcesAsync(string resourceDiscoveryGroupName)
        {
            PagedResult<List<AzureResourceDefinition>> pagedResult = null;
            var results = new List<AzureResourceDefinition>();
            var currentPage = 1;

            do
            {
                var response = await GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName, currentPage);
                var rawResponse = await response.Content.ReadAsStringAsync();

                pagedResult = GetDeserializedResponse<PagedResult<List<AzureResourceDefinition>>>(rawResponse);
                results.AddRange(pagedResult.Result);
                currentPage++;
            }
            while (pagedResult.HasMore);

            return results;
        }
    }
}