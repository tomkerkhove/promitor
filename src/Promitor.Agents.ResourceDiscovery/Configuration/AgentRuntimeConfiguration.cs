using Promitor.Agents.Core.Configuration;

namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class AgentRuntimeConfiguration : RuntimeConfiguration
    {
        public CacheConfiguration Cache { get; set; } = new();
    }
}
