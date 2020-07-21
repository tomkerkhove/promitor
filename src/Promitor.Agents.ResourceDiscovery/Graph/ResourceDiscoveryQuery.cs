using System;
using System.Collections.Generic;
using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Graph.Query;
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

        public virtual GraphQueryBuilder DefineQuery(ResourceCriteriaDefinition criteriaDefinition)
        {
            Guard.NotNull(criteriaDefinition, nameof(criteriaDefinition));
            Guard.NotNull(criteriaDefinition.Include, nameof(criteriaDefinition.Include));

            var graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceTypes)
                .WithSubscriptionsWithIds(criteriaDefinition.Include.Subscriptions) // Filter on queried subscriptions defined in landscape
                .WithResourceGroupsWithName(criteriaDefinition.Include.ResourceGroups)
                .WithinRegions(criteriaDefinition.Include.Regions)
                .WithTags(criteriaDefinition.Include.Tags)
                .Project(ProjectedFieldNames);

            return graphQueryBuilder;
        }

        public abstract string[] ResourceTypes { get; }
        public abstract string[] ProjectedFieldNames { get; }

        public abstract AzureResourceDefinition ParseResults(JToken resultRowEntry);
    }
}
