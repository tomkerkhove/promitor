using System;
using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class SqlElasticPoolDiscoveryQuery : ResourceDiscoveryQuery
    {
        public const string ServerSectionInResourceUri = "servers/";
        public override string[] ResourceTypes => new[] { "microsoft.sql/servers/elasticpools" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name", "id" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var serverName = GetParentResourceNameFromResourceUri(ServerSectionInResourceUri, resultRowEntry[4]);
            if (string.IsNullOrWhiteSpace(serverName))
            {
                throw new Exception($"Unable to determine server name from resource URI '{resultRowEntry[4]}'");
            }

            var resource = new SqlElasticPoolResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), serverName, resultRowEntry[3]?.ToString());
            return resource;
        }
    }
}
