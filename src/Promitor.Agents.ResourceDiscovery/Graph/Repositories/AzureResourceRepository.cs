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
using Promitor.Agents.ResourceDiscovery.Graph.Interfaces;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph.Repositories
{
    public class AzureResourceRepository : IAzureResourceRepository
    {
        private readonly IAzureResourceGraph _azureResourceGraph;
        private readonly ILogger<AzureResourceRepository> _logger;
        private readonly IOptionsMonitor<ResourceDeclaration> _resourceDeclarationMonitor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DiscoveryController" /> class.
        /// </summary>
        public AzureResourceRepository(IAzureResourceGraph azureResourceGraph, IOptionsMonitor<ResourceDeclaration> resourceDeclarationMonitor, ILogger<AzureResourceRepository> logger)
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
        /// <param name="resourceDiscoveryGroupName">Name of the resource collection</param>
        public virtual async Task<List<AzureResourceDefinition>> GetResourcesAsync(string resourceDiscoveryGroupName)
        {
            var resourceDeclaration = _resourceDeclarationMonitor.CurrentValue;
            var resourceDiscoveryGroupDefinition = resourceDeclaration.ResourceDiscoveryGroups.SingleOrDefault(collection => collection.Name.Equals(resourceDiscoveryGroupName, StringComparison.InvariantCultureIgnoreCase));
            if (resourceDiscoveryGroupDefinition == null)
            {
                // No collection found so nothing to return
                return null;
            }

            var resourceDiscovery = ResourceDiscoveryFactory.UseResourceDiscoveryFor(resourceDiscoveryGroupDefinition.Type);

            // 1. Create query per type
            var query = resourceDiscovery.DefineQuery(resourceDiscoveryGroupDefinition.Criteria).Build();

            // 2. Run Query
            var unparsedResults = await _azureResourceGraph.QueryTargetSubscriptionsAsync(resourceDiscoveryGroupName, query);

            // 3. Parse query results into resource
            var foundResources = resourceDiscovery.ParseQueryResults(unparsedResults);

            var contextualInformation = new Dictionary<string, object>
            {
                {"ResourceType",resourceDiscoveryGroupDefinition.Type},
                {"CollectionName",resourceDiscoveryGroupName}
            };
            _logger.LogMetric("Discovered Resources", foundResources.Count, contextualInformation);

            return foundResources;
        }

        public async Task<List<AzureSubscriptionInformation>> DiscoverAzureSubscriptionsAsync()
        {
            var query = @"ResourceContainers
| where type == ""microsoft.resources/subscriptions""
| project tenantId, subscriptionId, name, state=properties[""state""], spendingLimit=properties[""subscriptionPolicies""][""spendingLimit""], quotaId=properties[""subscriptionPolicies""][""quotaId""], authorizationSource=properties[""authorizationSource""]";

            var unparsedResults = await _azureResourceGraph.QueryAzureLandscapeAsync("Discover Azure Subscriptions", query);
            return ParseQueryResults(unparsedResults, row => new AzureSubscriptionInformation
            {
                TenantId = row[0]?.ToString(),
                Name = row[2]?.ToString(),
                Id = row[1]?.ToString(),
                State = row[3]?.ToString(),
                SpendingLimit = row[4]?.ToString(),
                QuotaId = row[5]?.ToString(),
                AuthorizationSource = row[6]?.ToString()
            });
        }

        public async Task<List<AzureResourceGroupInformation>> DiscoverAzureResourceGroupsAsync()
        {
            var query = @"ResourceContainers
| where type == ""microsoft.resources/subscriptions/resourcegroups""
| project tenantId, subscriptionId, name, location, provisioningState=properties[""provisioningState""], managedBy";

            var unparsedResults = await _azureResourceGraph.QueryAzureLandscapeAsync("Discover Azure Resource Groups", query);
            return ParseQueryResults(unparsedResults, row => new AzureResourceGroupInformation
            {
                TenantId = row[0]?.ToString(),
                SubscriptionId = row[1]?.ToString(),
                Name = row[2]?.ToString(),
                Region = row[3]?.ToString(),
                ProvisioningState = row[4]?.ToString(),
                ManagedBy = row[5]?.ToString()
            });
        }

        private List<TInfo> ParseQueryResults<TInfo>(JObject unparsedResults, Func<JToken, TInfo> parseResult)
        {
            Guard.NotNull(unparsedResults, nameof(unparsedResults));

            var foundResources = new List<TInfo>();
            var rows = unparsedResults["rows"];
            if (rows == null)
            {
                throw new Exception("No rows were found in the response");
            }

            foreach (var row in rows)
            {
                var resource = parseResult(row);

                foundResources.Add(resource);
            }

            return foundResources;
        }
    }
}