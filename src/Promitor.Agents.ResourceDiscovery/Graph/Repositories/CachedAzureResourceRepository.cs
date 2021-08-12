using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Controllers;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Repositories.Interfaces;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Repositories
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

        public async Task<List<AzureResourceDefinition>> GetResourcesAsync(string resourceDiscoveryGroupName)
        {
            Guard.NotNullOrWhitespace(resourceDiscoveryGroupName, nameof(resourceDiscoveryGroupName));

            if (_memoryCache.TryGetValue(resourceDiscoveryGroupName, out List<AzureResourceDefinition> cachedDiscoveredResources))
            {
                return cachedDiscoveredResources;
            }

            var discoveredResources = await _azureResourceRepository.GetResourcesAsync(resourceDiscoveryGroupName);
            AddCacheEntry(resourceDiscoveryGroupName, discoveredResources);

            return discoveredResources;
        }

        public async Task<List<AzureSubscriptionInformation>> DiscoverAzureSubscriptionsAsync()
        {
            // TODO: Cache
            return await _azureResourceRepository.DiscoverAzureSubscriptionsAsync();
        }

        public async Task<List<AzureResourceGroupInformation>> DiscoverAzureResourceGroupsAsync()
        {
            // TODO: Cache
            return await _azureResourceRepository.DiscoverAzureResourceGroupsAsync();
        }

        private void AddCacheEntry(string resourceDiscoveryGroupName, List<AzureResourceDefinition> discoveredResources)
        {
            var durationInMinutes = _cacheConfiguration.CurrentValue.DurationInMinutes;
            var cacheExpiration = TimeSpan.FromMinutes(durationInMinutes);
            _memoryCache.Set(resourceDiscoveryGroupName, discoveredResources, cacheExpiration);
        }
    }
}
