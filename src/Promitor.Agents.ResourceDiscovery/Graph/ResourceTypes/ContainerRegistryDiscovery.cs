﻿using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class ContainerRegistryDiscovery
    {
        public static string ResourceType = "microsoft.containerregistry/registries";

        public static string DefineQuery(ResourceCriteria criteria)
        {
            return GraphQuery.ForResourceType(ResourceType)
                .WithSubscriptionsWithIds(criteria.Subscriptions) // Filter on queried subscriptions defined in landscape
                .WithResourceGroupsWithName(criteria.ResourceGroups)
                .WithinRegions(criteria.Regions)
                .WithTags(criteria.Tags)
                .Project("subscriptionId", "resourceGroup", "type", "name", "id")
                .Build();
        }

        public static AzureResourceDefinition ParseQueryResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));
            
            var resource = new ContainerRegistryResourceDefinition(resultRowEntry[0].ToString(), resultRowEntry[1].ToString(), resultRowEntry[3].ToString());
            return resource;
        }
    }
}
