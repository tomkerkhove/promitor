using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Controllers;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Agents.ResourceDiscovery.Graph.Model;

namespace Promitor.Agents.ResourceDiscovery.Repositories
{
    public class ResourceRepository
    {
        private readonly AzureResourceGraph _azureResourceGraph;
        private readonly IOptionsMonitor<ResourceDeclaration> _resourceDeclarationMonitor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DiscoveryController" /> class.
        /// </summary>
        public ResourceRepository(AzureResourceGraph azureResourceGraph, IOptionsMonitor<ResourceDeclaration> resourceDeclarationMonitor)
        {
            Guard.NotNull(resourceDeclarationMonitor, nameof(resourceDeclarationMonitor));
            Guard.NotNull(azureResourceGraph, nameof(azureResourceGraph));

            _azureResourceGraph = azureResourceGraph;
            _resourceDeclarationMonitor = resourceDeclarationMonitor;
        }

        /// <summary>
        ///     Get resources that are part of a given resource collection
        /// </summary>
        /// <param name="resourceCollectionName">Name of the resource collection</param>
        public async Task<List<Resource>> GetResourcesAsync(string resourceCollectionName)
        {
            var resourceDeclaration = _resourceDeclarationMonitor.CurrentValue;
            var resourceCollectionDefinition = resourceDeclaration.ResourceCollections.SingleOrDefault(collection => collection.Name.Equals(resourceCollectionName, StringComparison.InvariantCultureIgnoreCase));
            if (resourceCollectionDefinition == null)
            {
                // No collection found so nothing to return
                return null;
            }

            var foundResources = await _azureResourceGraph.QueryAsync(resourceCollectionDefinition.Type, resourceCollectionDefinition.Criteria);
            return foundResources;
        }
    }
}