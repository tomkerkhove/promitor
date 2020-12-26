using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Graph.Model;

namespace Promitor.Agents.ResourceDiscovery.Graph.Interfaces
{
    public interface ICachedAzureResourceGraph
    {
        Task<JObject> QueryAsync(string queryName, string query, bool skipCache = false);
        Task<List<Resource>> QueryForResourcesAsync(string queryName, string query, List<string> targetSubscriptions, bool skipCache = false);
    }
}
