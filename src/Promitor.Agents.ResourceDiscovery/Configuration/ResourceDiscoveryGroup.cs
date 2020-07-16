using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class ResourceDiscoveryGroup
    {
        public string Name { get; set; }
        public ResourceType Type { get; set; }
        public ResourceCriteria Criteria { get; set; } = new ResourceCriteria();
    }
}