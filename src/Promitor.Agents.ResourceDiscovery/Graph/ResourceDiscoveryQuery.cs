using System;
using System.Collections.Generic;
using System.Linq;
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
                .Project(ProjectedFieldNames)
                .OrderByAsc(ProjectedFieldNames);

            return graphQueryBuilder;
        }
        
        /// <summary>
        /// Gets the name of the parent resource from a resource URI
        /// </summary>
        /// <param name="resourceIdentifier">Identifier to split on, ie servers/ for Azure SQL DB to get the server name</param>
        /// <param name="resourceUri">Uri of the child resource</param>
        /// <returns>Name of the parent resource</returns>
        public virtual string GetParentResourceNameFromResourceUri(string resourceIdentifier, JToken resourceUri)
        {
            Guard.NotNull(resourceUri, nameof(resourceUri));
            var rawResourceUri = resourceUri.ToString();
            Guard.For<ArgumentException>(() => string.IsNullOrWhiteSpace(rawResourceUri), nameof(resourceUri));

            var positionOfServersSection = rawResourceUri.LastIndexOf(resourceIdentifier, StringComparison.InvariantCultureIgnoreCase) + resourceIdentifier.Length;
            var sqlResourceDetailsParts = rawResourceUri.Substring(positionOfServersSection).Split("/");
            return sqlResourceDetailsParts.FirstOrDefault();
        }

        public abstract string[] ResourceTypes { get; }
        public abstract string[] ProjectedFieldNames { get; }

        public abstract AzureResourceDefinition ParseResults(JToken resultRowEntry);
    }
}
