using System.Collections.Generic;

namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class AzureLandscape
    {
        public string TenantId { get; set; }
        public List<string> Subscriptions { get; set; }
    }
}
