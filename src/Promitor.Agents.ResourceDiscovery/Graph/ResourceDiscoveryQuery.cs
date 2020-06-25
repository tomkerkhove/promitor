using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public abstract class ResourceDiscoveryQuery
    {
        public List<object> ParseQueryResults(JObject unparsedResults)
        {
            var foundResources = new List<object>();
            var rows = unparsedResults["rows"];
            foreach (var row in rows)
            {
                object resource = ParseResults(row);

                foundResources.Add(resource);
            }

            return foundResources;
        }

        public abstract string DefineQuery(ResourceCriteria criteria);
        public abstract AzureResourceDefinition ParseResults(JToken resultRowEntry);
    }
}
