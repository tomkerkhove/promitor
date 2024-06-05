using System;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    /// <summary>
    /// Contains settings to scrape metrics in batched API calls
    /// </summary>
    public class MetricBatchScrapeConfigV1
    {
        /// <summary>
        /// Enable batched scraping mode for all metrics in the scraper
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Maximum number of resources in a batch. Azure Monitor API specifies a max limit of 50 as of March 2024
        /// </summary>
        public int MaxBatchSize { get; set; }
    }
}
