﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Controllers;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Core.Contracts;

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
        public async Task<List<AzureResourceDefinition>> GetResourcesAsync(string resourceCollectionName)
        {
            var resourceDeclaration = _resourceDeclarationMonitor.CurrentValue;
            var resourceCollectionDefinition = resourceDeclaration.ResourceCollections.SingleOrDefault(collection => collection.Name.Equals(resourceCollectionName, StringComparison.InvariantCultureIgnoreCase));
            if (resourceCollectionDefinition == null)
            {
                // No collection found so nothing to return
                return null;
            }

            var resourceDiscovery = ResourceDiscoveryFactory.UseResourceDiscoveryFor(resourceCollectionDefinition.Type);

            // 1. Create query per type
            var query = resourceDiscovery.DefineQuery(resourceCollectionDefinition.Criteria);

            // 2. Run Query
            var unparsedResults = await _azureResourceGraph.QueryAsync(query);

            // 3. Parse query results into resource
            var foundResources = resourceDiscovery.ParseQueryResults(unparsedResults);

            var contextualInformation = new Dictionary<string, object>
            {
                {"ResourceType",resourceCollectionDefinition.Type},
                {"CollectionName",resourceCollectionName}
            };
            _logger.LogMetric("Discovered Resources", foundResources.Count, contextualInformation);

            return foundResources;
        }
    }
}