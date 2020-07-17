using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class CosmosDbDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.documentdb/databaseaccounts" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));
            
            var resource = new CosmosDbResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), resultRowEntry[3]?.ToString());
            return resource;
        }
    }
}
