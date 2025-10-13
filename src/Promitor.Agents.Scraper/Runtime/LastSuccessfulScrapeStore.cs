using System;
using Microsoft.Extensions.Caching.Memory;

namespace Promitor.Agents.Scraper.Runtime
{
    /// <summary>
    /// Thread-safe in-memory store for the last successful scrape completion timestamp.
    /// </summary>
    public class LastSuccessfulScrapeStore : ILastSuccessfulScrapeStore
    {
        private const string CacheKey = "LastSuccessfulScrapeUtc";
        private readonly IMemoryCache _memoryCache;

        public LastSuccessfulScrapeStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public void MarkNow()
        {
            _memoryCache.Set(CacheKey, DateTimeOffset.UtcNow);
        }

        public DateTimeOffset? GetLast()
        {
            if (_memoryCache.TryGetValue(CacheKey, out DateTimeOffset last))
            {
                return last;
            }
            return null;
        }
    }
}


