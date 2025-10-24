using System;

namespace Promitor.Agents.Scraper.Runtime
{
    public interface ILastSuccessfulScrapeStore
    {
        /// <summary>
        /// Records the current UTC time as the most recent successful scrape completion time.
        /// </summary>
        void MarkNow();

        /// <summary>
        /// Retrieves the last successful scrape completion time in UTC, or null if none recorded.
        /// </summary>
        DateTimeOffset? GetLast();
    }
}


