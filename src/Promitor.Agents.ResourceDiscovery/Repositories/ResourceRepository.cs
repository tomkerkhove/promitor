using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Controllers;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Repositories
{
    public class ResourceRepository
    {
        private readonly AzureResourceGraph _azureResourceGraph;
        private readonly ILogger<ResourceRepository> _logger;
        private readonly IOptionsMonitor<ResourceDeclaration> _resourceDeclarationMonitor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DiscoveryController" /> class.
        /// </summary>
        public ResourceRepository(AzureResourceGraph azureResourceGraph, IOptionsMonitor<ResourceDeclaration> resourceDeclarationMonitor, ILogger<ResourceRepository> logger)
        {
            Guard.NotNull(resourceDeclarationMonitor, nameof(resourceDeclarationMonitor));
            Guard.NotNull(azureResourceGraph, nameof(azureResourceGraph));
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
            _azureResourceGraph = azureResourceGraph;
            _resourceDeclarationMonitor = resourceDeclarationMonitor;
        }

        /// <summary>
        ///     Get resources that are part of a given resource collection
        /// </summary>
        /// <param name="resourceCollectionName">Name of the resource collection</param>
        public async Task<List<object>> GetResourcesAsync(string resourceCollectionName)
        {
            var resourceDeclaration = _resourceDeclarationMonitor.CurrentValue;
            var resourceCollectionDefinition = resourceDeclaration.ResourceCollections.SingleOrDefault(collection => collection.Name.Equals(resourceCollectionName, StringComparison.InvariantCultureIgnoreCase));
            if (resourceCollectionDefinition == null)
            {
                // No collection found so nothing to return
                return null;
            }

            //var foundResources = await _azureResourceGraph.QueryAsync(resourceCollectionDefinition.Type, resourceCollectionDefinition.Criteria);

            //var contextualInformation = new Dictionary<string, object>
            //{
            //    {"ResourceType",resourceCollectionDefinition.Type},
            //    {"CollectionName",resourceCollectionName}
            //};
            //_logger.LogMetric("Discovered Resources", foundResources.Count, contextualInformation);

            var query = DefineQuery(resourceCollectionDefinition.Type, resourceCollectionDefinition.Criteria);
            var unparsedResults = await _azureResourceGraph.QueryAsync(query);
            var foundResources = ParseQueryResults(resourceCollectionDefinition.Type, unparsedResults);

            return foundResources;
        }

        private string DefineQuery(string resourceType, ResourceCriteria criteria)
        {
            if (resourceType == "microsoft.containerregistry/registries")
            {
                return GraphQuery.ForResourceType(resourceType)
                    .WithSubscriptionsWithIds(criteria.Subscriptions) // Filter on queried subscriptions defined in landscape
                    .WithResourceGroupsWithName(criteria.ResourceGroups)
                    .WithinRegions(criteria.Regions)
                    .WithTags(criteria.Tags)
                    .Project("subscriptionId", "resourceGroup", "type", "name", "id")
                    .Build();
            }

            return GraphQuery.ForResourceType(resourceType)
                .WithSubscriptionsWithIds(criteria.Subscriptions) // Filter on queried subscriptions defined in landscape
                .WithResourceGroupsWithName(criteria.ResourceGroups)
                .WithinRegions(criteria.Regions)
                .WithTags(criteria.Tags)
                .Project("subscriptionId", "resourceGroup", "type", "name", "id")
                .Build();
        }

        private List<object> ParseQueryResults(string resourceType, JObject unparsedResults)
        {
            var foundResources = new List<object>();
            var rows = unparsedResults["rows"];
            foreach (var row in rows)
            {
                object resource;
                if (resourceType == "microsoft.containerregistry/registries")
                {
                    resource = new ContainerRegistryResourceDefinition(row[0].ToString(), row[1].ToString(), row[3].ToString());
                }
                else
                {
                    resource = new Resource(row[0].ToString(), row[1].ToString(), row[2].ToString(),
                        row[3].ToString());
                }

                foundResources.Add(resource);
            }

            return foundResources;
        }
    }
}