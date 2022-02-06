using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Controllers;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph.Repositories
{
    public class CachedAzureResourceRepository : IAzureResourceRepository
    {
        private readonly IOptionsMonitor<CacheConfiguration> _cacheConfiguration;
        private readonly AzureResourceRepository _azureResourceRepository;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DiscoveryController" /> class.
        /// </summary>
        public CachedAzureResourceRepository(AzureResourceRepository azureResourceRepository, IMemoryCache memoryCache, IOptionsMonitor<CacheConfiguration> cacheConfiguration)
        {
            Guard.NotNull(cacheConfiguration, nameof(cacheConfiguration));
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));
            Guard.NotNull(memoryCache, nameof(memoryCache));

            _memoryCache = memoryCache;
            _azureResourceRepository = azureResourceRepository;
            _cacheConfiguration = cacheConfiguration;
        }

        private const string GetResourceCacheKey = "GetResources-{0}";
        private const string AzureSubscriptionsCacheKey = "AzureSubscriptions";
        private const string AzureResourceGroupsCacheKey = "AzureResourceGroups";

        public async Task<PagedResult<List<AzureResourceDefinition>>> GetResourcesAsync(string resourceDiscoveryGroupName, int pageSize, int currentPage)
        {
            Guard.NotNullOrWhitespace(resourceDiscoveryGroupName, nameof(resourceDiscoveryGroupName));

            var cacheKey = string.Format(GetResourceCacheKey, resourceDiscoveryGroupName);

            if (_memoryCache.TryGetValue(cacheKey, out PagedResult<List<AzureResourceDefinition>> cachedDiscoveredResources))
            {
                return cachedDiscoveredResources;
            }

            var discoveredResources = await _azureResourceRepository.GetResourcesAsync(resourceDiscoveryGroupName, pageSize, currentPage);
            AddCacheEntry(cacheKey, discoveredResources);

            return discoveredResources;
        }

        public async Task<List<AzureSubscriptionInformation>> DiscoverAzureSubscriptionsAsync(int pageSize, int currentPage)
        {
            if (_memoryCache.TryGetValue(AzureSubscriptionsCacheKey, out List<AzureSubscriptionInformation> cachedAzureSubscriptions))
            {
                return cachedAzureSubscriptions;
            }

            var discoveredAzureSubscriptions = await _azureResourceRepository.DiscoverAzureSubscriptionsAsync(pageSize, currentPage);
            AddCacheEntry(AzureSubscriptionsCacheKey, discoveredAzureSubscriptions);

            return discoveredAzureSubscriptions;
        }

        public async Task<List<AzureResourceGroupInformation>> DiscoverAzureResourceGroupsAsync(int pageSize, int currentPage)
        {
            if (_memoryCache.TryGetValue(AzureResourceGroupsCacheKey, out List<AzureResourceGroupInformation> cachedAzureResourceGroups))
            {
                return cachedAzureResourceGroups;
            }

            var discoveredAzureResourceGroups = await _azureResourceRepository.DiscoverAzureResourceGroupsAsync(pageSize, currentPage);
            AddCacheEntry(AzureResourceGroupsCacheKey, discoveredAzureResourceGroups);

            return discoveredAzureResourceGroups;
        }

        private void AddCacheEntry<TEntry>(string resourceDiscoveryGroupName, TEntry discoveredResources)
        {
            var durationInMinutes = _cacheConfiguration.CurrentValue.DurationInMinutes;
            var cacheExpiration = TimeSpan.FromMinutes(durationInMinutes);
            _memoryCache.Set(resourceDiscoveryGroupName, discoveredResources, cacheExpiration);
        }
    }
}
