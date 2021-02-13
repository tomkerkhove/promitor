using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class SynapseApacheSparkPoolDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.synapse/workspaces/bigdatapools" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name", "id" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            // TODO: Parse id (4) to get workspace name
            var workspaceName = string.Empty;
            var resource = new SynapseApacheSparkPoolResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), workspaceName, resultRowEntry[3]?.ToString());
            return resource;
        }
    }
}
