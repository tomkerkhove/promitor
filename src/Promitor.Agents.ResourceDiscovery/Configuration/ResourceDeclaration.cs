using System.Collections.Generic;

namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class ResourceDeclaration
    {
        public AzureLandscape AzureLandscape { get; set; }
        public List<ResourceCollection> ResourceCollections { get; set; }
    }
}