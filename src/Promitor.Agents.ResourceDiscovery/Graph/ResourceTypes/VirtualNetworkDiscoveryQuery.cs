using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class VirtualNetworkDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.network/virtualnetworks" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var virtualNetworkName = resultRowEntry[2]?.ToString();
            var resource = new VirtualNetworkResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), virtualNetworkName);
            return resource;
        }
    }
}
