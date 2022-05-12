using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class MariaDbDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.dbformariadb/servers" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var serverName = resultRowEntry[2]?.ToString();
            var resource = new MariaDbResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), serverName);
            return resource;
        }
    }
}
