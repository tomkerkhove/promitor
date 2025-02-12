using System.Collections.Generic;
using Promitor.Core.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class AzureLandscape : IAzureCloudEndpoints
    {
        public string TenantId { get; set; }
        public List<string> Subscriptions { get; set; }
        public AzureCloud Cloud { get; set; } = AzureCloud.Global;
        public AzureEndpoints Endpoints { get; set; }
    }
}
