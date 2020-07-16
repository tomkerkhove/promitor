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

            var resources = await _resourceDiscoveryClient.GetAsync(resourceDiscoveryGroupName);
            return resources;
        }
    }
}