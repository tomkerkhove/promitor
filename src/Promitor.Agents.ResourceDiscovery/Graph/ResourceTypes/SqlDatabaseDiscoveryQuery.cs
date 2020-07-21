using System;
using System.Linq;
using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class SqlDatabaseDiscoveryQuery : ResourceDiscoveryQuery
    {
        private const string ServerSectionInResourceUri = "servers/";

        public override string[] ResourceTypes => new[] { "microsoft.sql/servers/databases" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name", "id" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));
            
            var serverName = GetServerNameFromResourceUri(resultRowEntry[4]);

            if (string.IsNullOrWhiteSpace(serverName))
            {
                throw new Exception($"Unable to determine server name from resource URI '{resultRowEntry[4]}'");
            }

            var resource = new SqlDatabaseResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), serverName,resultRowEntry[3]?.ToString());
            return resource;
        }

        public virtual string GetServerNameFromResourceUri(JToken resourceUri)
        {
            Guard.NotNull(resourceUri, nameof(resourceUri));
            var rawResourceUri = resourceUri.ToString();
            Guard.For<ArgumentException>(()=>string.IsNullOrWhiteSpace(rawResourceUri), nameof(resourceUri));

            var positionOfServersSection = rawResourceUri.LastIndexOf(ServerSectionInResourceUri, StringComparison.InvariantCultureIgnoreCase) + ServerSectionInResourceUri.Length;
            var sqlResourceDetailsParts = rawResourceUri.Substring(positionOfServersSection).Split("/");
            return sqlResourceDetailsParts.FirstOrDefault();
        }
    }
}
