using System;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
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
        ///     Initializes a new instance of the <see cref="CachedAzureResourceRepository" /> class.
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

        private const string GetResourceCacheKey = "GetResources-{0}-{1}-{2}";
        private const string AzureSubscriptionsCacheKey = "AzureSubscriptions-{0}-{1}";
        private const string AzureResourceGroupsCacheKey = "AzureResourceGroups-{0}-{1}";

        public async Task<PagedPayload<AzureResourceDefinition>> GetResourcesAsync(string resourceDiscoveryGroupName, int pageSize, int currentPage)
        {
            Guard.NotNullOrWhitespace(resourceDiscoveryGroupName, nameof(resourceDiscoveryGroupName));

            var cacheKey = string.Format(GetResourceCacheKey, resourceDiscoveryGroupName, pageSize, currentPage);

            if (_memoryCache.TryGetValue(cacheKey, out PagedPayload<AzureResourceDefinition> cachedDiscoveredResources))
            {
                return cachedDiscoveredResources;
            }

            var discoveredResources = await _azureResourceRepository.GetResourcesAsync(resourceDiscoveryGroupName, pageSize, currentPage);
            AddCacheEntry(cacheKey, discoveredResources);

            return discoveredResources;
        }

        public async Task<PagedPayload<AzureSubscriptionInformation>> DiscoverAzureSubscriptionsAsync(int pageSize, int currentPage)
        {
            var cacheKey = string.Format(AzureSubscriptionsCacheKey, pageSize, currentPage);
            if (_memoryCache.TryGetValue(cacheKey, out PagedPayload<AzureSubscriptionInformation> cachedAzureSubscriptions))
            {
                return cachedAzureSubscriptions;
            }

            var discoveredAzureSubscriptions = await _azureResourceRepository.DiscoverAzureSubscriptionsAsync(pageSize, currentPage);
            AddCacheEntry(cacheKey, discoveredAzureSubscriptions);

            return discoveredAzureSubscriptions;
        }

        public async Task<PagedPayload<AzureResourceGroupInformation>> DiscoverAzureResourceGroupsAsync(int pageSize, int currentPage)
        {
            var cacheKey = string.Format(AzureResourceGroupsCacheKey, pageSize, currentPage);
            if (_memoryCache.TryGetValue(cacheKey, out PagedPayload<AzureResourceGroupInformation> cachedAzureResourceGroups))
            {
                return cachedAzureResourceGroups;
            }

            var discoveredAzureResourceGroups = await _azureResourceRepository.DiscoverAzureResourceGroupsAsync(pageSize, currentPage);
            AddCacheEntry(cacheKey, discoveredAzureResourceGroups);

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
