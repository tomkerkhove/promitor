using System.Collections.Generic;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class ResourceDeclaration
    {
        public string Version { get; set; } = SpecVersion.v1.ToString();
        public AzureLandscape AzureLandscape { get; set; }
        public List<ResourceDiscoveryGroup> ResourceDiscoveryGroups { get; set; }
    }
}