using System.Threading.Tasks;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces
{
    public interface IAzureResourceRepository
    {
        Task<PagedPayload<AzureResourceDefinition>> GetResourcesAsync(string resourceDiscoveryGroupName, int pageSize, int currentPage);
        Task<PagedPayload<AzureSubscriptionInformation>> DiscoverAzureSubscriptionsAsync(int pageSize, int currentPage);
        Task<PagedPayload<AzureResourceGroupInformation>> DiscoverAzureResourceGroupsAsync(int pageSize, int currentPage);
    }
}
