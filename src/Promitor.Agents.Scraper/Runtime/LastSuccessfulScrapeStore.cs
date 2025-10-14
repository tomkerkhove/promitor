using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Promitor.Agents.Scraper.Runtime
{
    /// <summary>
    /// Thread-safe in-memory store for the last successful scrape completion timestamp.
    /// </summary>
    public class LastSuccessfulScrapeStore : ILastSuccessfulScrapeStore
    {
        private const string CacheKey = "LastSuccessfulScrapeUtc";
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<LastSuccessfulScrapeStore> _logger;

        public LastSuccessfulScrapeStore(IMemoryCache memoryCache, ILogger<LastSuccessfulScrapeStore> logger)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void MarkNow()
        {
            var timestamp = DateTimeOffset.UtcNow;
            _memoryCache.Set(CacheKey, timestamp);
            _logger.LogDebug("Set last successful scrape timestamp to {Timestamp:o}.", timestamp);
        }

        public DateTimeOffset? GetLast()
        {
            if (_memoryCache.TryGetValue(CacheKey, out DateTimeOffset last))
            {
                _logger.LogDebug("Retrieved last successful scrape timestamp {Timestamp:o}.", last);
                return last;
            }
            _logger.LogDebug("No last successful scrape timestamp found.");
            return null;
        }
    }
}


