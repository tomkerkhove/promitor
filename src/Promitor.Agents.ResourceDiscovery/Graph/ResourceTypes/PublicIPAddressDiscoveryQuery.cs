using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class PublicIPAddressDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.network/publicipaddresses" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var publicIPAddressName = resultRowEntry[2]?.ToString();

            var resource = new PublicIPAddressResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), publicIPAddressName);
            return resource;
        }
    }
}
