using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Promitor.Core.Contracts;

namespace Promitor.Agents.Scraper.Discovery
{
    public class ResourceDiscoveryRepository
    {
        private readonly ResourceDiscoveryClient _resourceDiscoveryClient;

        public ResourceDiscoveryRepository(ResourceDiscoveryClient resourceDiscoveryClient)
        {
            Guard.NotNull(resourceDiscoveryClient, nameof(resourceDiscoveryClient));

            _resourceDiscoveryClient = resourceDiscoveryClient;
        }

        public async Task<List<AzureResourceDefinition>> GetResourceDiscoveryGroupAsync(string resourceDiscoveryGroupName)
        {
            Guard.NotNullOrWhitespace(resourceDiscoveryGroupName,nameof(resourceDiscoveryGroupName));

            PagedPayload<AzureResourceDefinition> pagedPayload;
            var results = new List<AzureResourceDefinition>();
            var currentPage = 1;

            do
            {
                pagedPayload = await _resourceDiscoveryClient.GetAsync(resourceDiscoveryGroupName, currentPage);
                results.AddRange(pagedPayload.Result);
                currentPage++;
            }
            while (pagedPayload.HasMore);

            return results;
        }
    }
}