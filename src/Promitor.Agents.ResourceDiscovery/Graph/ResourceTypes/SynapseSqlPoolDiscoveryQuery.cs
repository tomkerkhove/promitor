using System;
using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class SynapseSqlPoolDiscoveryQuery : ResourceDiscoveryQuery
    {
        public const string WorkspaceSectionInResourceUri = "workspaces/";
        public override string[] ResourceTypes => new[] { "microsoft.synapse/workspaces/sqlpools" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name", "id" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            // Get workspace name
            var workspaceName = GetParentResourceNameFromResourceUri(WorkspaceSectionInResourceUri, resultRowEntry[4]);
            if (string.IsNullOrWhiteSpace(workspaceName))
            {
                throw new Exception($"Unable to determine workspace name from resource URI '{resultRowEntry[4]}'");
            }

            // Create resource definition
            var resource = new SynapseSqlPoolResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), workspaceName, resultRowEntry[3]?.ToString());
            return resource;
        }
    }
}
