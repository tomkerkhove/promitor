using System;
using System.Linq;
using GuardNet;
using Newtonsoft.Json.Linq;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public abstract class SqlDiscoveryQuery : ResourceDiscoveryQuery
    {
        private const string ServerSectionInResourceUri = "servers/";

        protected virtual string GetServerNameFromResourceUri(JToken resourceUri)
        {
            Guard.NotNull(resourceUri, nameof(resourceUri));
            var rawResourceUri = resourceUri.ToString();
            Guard.For<ArgumentException>(() => string.IsNullOrWhiteSpace(rawResourceUri), nameof(resourceUri));

            var positionOfServersSection = rawResourceUri.LastIndexOf(ServerSectionInResourceUri, StringComparison.InvariantCultureIgnoreCase) + ServerSectionInResourceUri.Length;
            var sqlResourceDetailsParts = rawResourceUri.Substring(positionOfServersSection).Split("/");
            return sqlResourceDetailsParts.FirstOrDefault();
        }
    }
}
