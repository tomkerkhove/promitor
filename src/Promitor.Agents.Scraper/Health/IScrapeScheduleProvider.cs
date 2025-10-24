using System;

namespace Promitor.Agents.Scraper.Health
{
    /// <summary>
    /// Provides information about the configured scrape schedules
    /// </summary>
    public interface IScrapeScheduleProvider
    {
        /// <summary>
        /// Gets the minimum scrape interval across all configured metrics
        /// </summary>
        TimeSpan GetMinimumScrapeInterval();
    }
}

