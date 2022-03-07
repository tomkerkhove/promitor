using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces
{
    public interface IAzureResourceRepository
    {
        Task<PagedResult<List<AzureResourceDefinition>>> GetResourcesAsync(string resourceDiscoveryGroupName, int pageSize, int currentPage);
        Task<PagedResult<List<AzureSubscriptionInformation>>> DiscoverAzureSubscriptionsAsync(int pageSize, int currentPage);
        Task<PagedResult<List<AzureResourceGroupInformation>>> DiscoverAzureResourceGroupsAsync(int pageSize, int currentPage);
    }
}
