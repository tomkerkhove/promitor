using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
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
        ///     Initializes a new instance of the <see cref="AzureResourceRepository" /> class.
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
        /// <param name="pageSize">The amount of results that are allowed per page</param>
        /// <param name="currentPage">Current page that is being queried</param>
        public virtual async Task<PagedPayload<AzureResourceDefinition>> GetResourcesAsync(string resourceDiscoveryGroupName, int pageSize, int currentPage)
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
            var unparsedResults = await _azureResourceGraph.QueryTargetSubscriptionsAsync(resourceDiscoveryGroupName, query, pageSize, currentPage);

            // 3. Parse query results into resource
            var foundResources = resourceDiscovery.ParseQueryResults(unparsedResults.Result);

            var contextualInformation = new Dictionary<string, object>
            {
                {"ResourceType",resourceDiscoveryGroupDefinition.Type},
                {"CollectionName",resourceDiscoveryGroupName}
            };
            
            // Log a metric with amount of discovered resources
            // But only if the current page is 1, or we'll emit it too much
            if(currentPage == 1)
            {
                _logger.LogCustomMetric("Discovered Resources", unparsedResults.TotalRecords, contextualInformation);
            }

            return new PagedPayload<AzureResourceDefinition>(foundResources, unparsedResults.TotalRecords, unparsedResults.CurrentPage, unparsedResults.PageSize);
        }

        public async Task<PagedPayload<AzureSubscriptionInformation>> DiscoverAzureSubscriptionsAsync(int pageSize, int currentPage)
        {
            var query = @"ResourceContainers
| where type == ""microsoft.resources/subscriptions""
| project tenantId, subscriptionId, name, state=properties[""state""], spendingLimit=properties[""subscriptionPolicies""][""spendingLimit""], quotaId=properties[""subscriptionPolicies""][""quotaId""], authorizationSource=properties[""authorizationSource""]
| order by tenantId asc, subscriptionId asc, name asc";

            var unparsedResults = await _azureResourceGraph.QueryAzureLandscapeAsync("Discover Azure Subscriptions", query, pageSize, currentPage);
            var foundSubscriptionInformation = ParseQueryResults(unparsedResults.Result, row => new AzureSubscriptionInformation
            {
                TenantId = row[0]?.ToString(),
                Name = row[2]?.ToString(),
                Id = row[1]?.ToString(),
                State = row[3]?.ToString(),
                SpendingLimit = row[4]?.ToString(),
                QuotaId = row[5]?.ToString(),
                AuthorizationSource = row[6]?.ToString()
            });

            return new PagedPayload<AzureSubscriptionInformation>(foundSubscriptionInformation, unparsedResults.TotalRecords, unparsedResults.CurrentPage, unparsedResults.PageSize);
        }

        public async Task<PagedPayload<AzureResourceGroupInformation>> DiscoverAzureResourceGroupsAsync(int pageSize, int currentPage)
        {
            var query = @"ResourceContainers
| where type == ""microsoft.resources/subscriptions/resourcegroups""
| project tenantId, subscriptionId, name, location, provisioningState=properties[""provisioningState""], managedBy
| order by tenantId asc, subscriptionId asc, name asc";

            var unparsedResults = await _azureResourceGraph.QueryAzureLandscapeAsync("Discover Azure Resource Groups", query, pageSize, currentPage);
            var foundResourceGroupInformation = ParseQueryResults(unparsedResults.Result, row => new AzureResourceGroupInformation
            {
                TenantId = row[0]?.ToString(),
                SubscriptionId = row[1]?.ToString(),
                Name = row[2]?.ToString(),
                Region = row[3]?.ToString(),
                ProvisioningState = row[4]?.ToString(),
                ManagedBy = row[5]?.ToString()
            });

            return new PagedPayload<AzureResourceGroupInformation>(foundResourceGroupInformation, unparsedResults.TotalRecords, unparsedResults.CurrentPage, unparsedResults.PageSize);
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