using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class AzureFirewallDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.network/azureFirewalls" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var azureFirewallName = resultRowEntry[3]?.ToString();

            var resource = new AzureFirewallResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), azureFirewallName);
            return resource;
        }
    }
}
