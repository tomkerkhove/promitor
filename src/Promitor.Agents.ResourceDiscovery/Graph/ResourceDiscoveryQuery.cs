using System;
using System.Collections.Generic;
using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public abstract class ResourceDiscoveryQuery
    {
        public List<AzureResourceDefinition> ParseQueryResults(JObject unparsedResults)
        {
            Guard.NotNull(unparsedResults, nameof(unparsedResults));

            var foundResources = new List<AzureResourceDefinition>();
            var rows = unparsedResults["rows"];
            if (rows == null)
            {
                throw new Exception("No rows were found in the response");
            }

            foreach (var row in rows)
            {
                var resource = ParseResults(row);

                foundResources.Add(resource);
            }

            return foundResources;
        }

        public abstract string DefineQuery(ResourceCriteria criteria);
        public abstract AzureResourceDefinition ParseResults(JToken resultRowEntry);
    }
}
