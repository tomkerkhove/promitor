using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Agents.ResourceDiscovery.Graph.Model;

namespace Promitor.Agents.ResourceDiscovery.Graph.Interfaces
{
    public interface IAzureResourceGraph
    {
        Task<PagedQueryResult> QueryAzureLandscapeAsync(string queryName, string query, int pageSize, int currentPage);
        Task<PagedQueryResult> QueryTargetSubscriptionsAsync(string queryName, string query, int pageSize, int currentPage);
        Task<List<Resource>> QueryForResourcesAsync(string queryName, string query, List<string> targetSubscriptions, int pageSize, int currentPage);
    }
}
