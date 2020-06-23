using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Controllers;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Promitor.Core.Contracts;
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

            // 1. Create query per type
            var query = DefineQuery(resourceCollectionDefinition.Type, resourceCollectionDefinition.Criteria);

            // 2. Run Query
            var unparsedResults = await _azureResourceGraph.QueryAsync(query);

            // 3. Parse query results into resource
            var foundResources = ParseQueryResults(resourceCollectionDefinition.Type, unparsedResults);

            var contextualInformation = new Dictionary<string, object>
            {
                {"ResourceType",resourceCollectionDefinition.Type},
                {"CollectionName",resourceCollectionName}
            };
            _logger.LogMetric("Discovered Resources", foundResources.Count, contextualInformation);

            return foundResources;
        }

        private string DefineQuery(ResourceType resourceType, ResourceCriteria criteria)
        {
            switch (resourceType)
            {
                case ResourceType.ContainerRegistry:
                    return ContainerRegistryDiscovery.DefineQuery(criteria);
                case ResourceType.AppPlan:
                    return AppPlanDiscovery.DefineQuery(criteria);
                default:
                    throw new NotSupportedException();
            }
        }

        private List<object> ParseQueryResults(ResourceType resourceType, JObject unparsedResults)
        {
            var foundResources = new List<object>();
            var rows = unparsedResults["rows"];
            foreach (var row in rows)
            {
                object resource;
                switch (resourceType)
                {
                    case ResourceType.ContainerRegistry:
                        resource = ContainerRegistryDiscovery.ParseQueryResults(row);
                        break;
                    case ResourceType.AppPlan:
                        resource =AppPlanDiscovery.ParseQueryResults(row);
                        break;
                    default:
                        throw new NotSupportedException();
                }

                foundResources.Add(resource);
            }

            return foundResources;
        }
    }
}