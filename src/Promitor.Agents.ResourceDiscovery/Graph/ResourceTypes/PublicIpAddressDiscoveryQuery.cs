using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class PublicIpAddressDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.network/publicipaddresses" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var publicIpAddressName = resultRowEntry[2]?.ToString();

            var resource = new PublicIpAddressResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), publicIpAddressName);
            return resource;
        }
    }
}
