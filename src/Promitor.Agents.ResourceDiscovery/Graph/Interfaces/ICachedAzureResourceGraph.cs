using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Agents.ResourceDiscovery.Graph.Model;

namespace Promitor.Agents.ResourceDiscovery.Graph.Interfaces
{
    public interface ICachedAzureResourceGraph
    {
        Task<PagedQueryResult> QueryAsync(string queryName, string query, int pageSize, int currentPage, bool skipCache = false);
        Task<List<Resource>> QueryForResourcesAsync(string queryName, string query, List<string> targetSubscriptions, int pageSize, int currentPage, bool skipCache = false);
    }
}
