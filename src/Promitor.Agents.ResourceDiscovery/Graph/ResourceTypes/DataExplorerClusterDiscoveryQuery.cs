using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class DataExplorerClusterDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.kusto/clusters" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var clusterName = resultRowEntry[2]?.ToString();
            var resource = new DataExplorerClusterResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), clusterName);
            return resource;
        }
    }
}
