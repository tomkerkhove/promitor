using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Controllers;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Repositories
{
    public class CachedResourceRepository : ResourceRepository
    {
        private readonly IOptionsMonitor<CacheConfiguration> _cacheConfiguration;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DiscoveryController" /> class.
        /// </summary>
        public CachedResourceRepository(AzureResourceGraph azureResourceGraph, IMemoryCache memoryCache, IOptionsMonitor<CacheConfiguration> cacheConfiguration, IOptionsMonitor<ResourceDeclaration> resourceDeclarationMonitor, ILogger<CachedResourceRepository> logger)
            : base(azureResourceGraph, resourceDeclarationMonitor, logger)
        {
            Guard.NotNull(cacheConfiguration, nameof(cacheConfiguration));
            Guard.NotNull(memoryCache, nameof(memoryCache));

            _memoryCache = memoryCache;
            _cacheConfiguration = cacheConfiguration;
        }

        public override async Task<List<AzureResourceDefinition>> GetResourcesAsync(string resourceDiscoveryGroupName)
        {
            Guard.NotNullOrWhitespace(resourceDiscoveryGroupName, nameof(resourceDiscoveryGroupName));

            if (_memoryCache.TryGetValue(resourceDiscoveryGroupName, out List<AzureResourceDefinition> cachedDiscoveredResources))
            {
                return cachedDiscoveredResources;
            }

            var discoveredResources = await base.GetResourcesAsync(resourceDiscoveryGroupName);
            AddCacheEntry(resourceDiscoveryGroupName, discoveredResources);

            return discoveredResources;
        }

        private void AddCacheEntry(string resourceDiscoveryGroupName, List<AzureResourceDefinition> discoveredResources)
        {
            var durationInMinutes = _cacheConfiguration.CurrentValue.DurationInMinutes;
            var cacheExpiration = TimeSpan.FromMinutes(durationInMinutes);
            _memoryCache.Set(resourceDiscoveryGroupName, discoveredResources, cacheExpiration);
        }
    }
}
