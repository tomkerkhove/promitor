using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces
{
    public interface IAzureResourceRepository
    {
        Task<List<AzureResourceDefinition>> GetResourcesAsync(string resourceDiscoveryGroupName);
        Task<List<AzureSubscriptionInformation>> DiscoverAzureSubscriptionsAsync();
        Task<List<AzureResourceGroupInformation>> DiscoverAzureResourceGroupsAsync();
    }
}
