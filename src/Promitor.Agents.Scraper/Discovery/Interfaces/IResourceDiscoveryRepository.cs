using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Agents.Core.Contracts;
using Promitor.Core.Contracts;

namespace Promitor.Agents.Scraper.Discovery.Interfaces
{
    public interface IResourceDiscoveryRepository
    {
        Task<List<AzureResourceDefinition>> GetResourceDiscoveryGroupAsync(string resourceDiscoveryGroupName);
        Task<AgentHealthReport> GetHealthAsync();
    }
}