using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Graph.Interfaces;
using Promitor.Agents.ResourceDiscovery.Graph.Model;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public class CachedAzureResourceGraph : ICachedAzureResourceGraph
    {
        private readonly IOptionsMonitor<CacheConfiguration> _cacheConfiguration;
        private readonly IAzureResourceGraph _azureResourceGraph;
        private readonly IMemoryCache _memoryCache;

        public CachedAzureResourceGraph(IAzureResourceGraph azureResourceGraph, IMemoryCache memoryCache, IOptionsMonitor<CacheConfiguration> cacheConfiguration)
        {
            Guard.NotNull(cacheConfiguration, nameof(cacheConfiguration));
            Guard.NotNull(azureResourceGraph, nameof(azureResourceGraph));
            Guard.NotNull(memoryCache, nameof(memoryCache));

            _memoryCache = memoryCache;
            _azureResourceGraph = azureResourceGraph;
            _cacheConfiguration = cacheConfiguration;
        }

        public async Task<JObject> QueryAsync(string queryName, string query, bool skipCache = false)
        {
            if (skipCache == false && _memoryCache.TryGetValue(queryName, out JObject cachedQueryResult))
            {
                return cachedQueryResult;
            }

            var queryResult = await _azureResourceGraph.QueryAsync(queryName, query);
            AddCacheEntry(queryName, queryResult);

            return queryResult;
        }

        public async Task<List<Resource>> QueryForResourcesAsync(string queryName, string query, List<string> targetSubscriptions, bool skipCache = false)
        {
            if (skipCache == false && _memoryCache.TryGetValue(queryName, out List<Resource> cachedResources))
            {
                return cachedResources;
            }

            var queryResult = await _azureResourceGraph.QueryForResourcesAsync(queryName, query, targetSubscriptions);
            AddCacheEntry(queryName, queryResult);

            return queryResult;
        }

        private void AddCacheEntry(string cacheKey, object cacheEntry)
        {
            var durationInMinutes = _cacheConfiguration.CurrentValue.DurationInMinutes;
            var cacheExpiration = TimeSpan.FromMinutes(durationInMinutes);
            _memoryCache.Set(cacheKey, cacheEntry, cacheExpiration);
        }
    }
}
