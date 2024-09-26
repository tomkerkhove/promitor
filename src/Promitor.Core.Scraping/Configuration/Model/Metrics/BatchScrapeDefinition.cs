using System;
using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Contracts;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    /// Defines a batch of ScrapeDefinitions to be executed in a single request 
    /// Scrape definitions within a batch should share
    /// 1. The same resource type 
    /// 2. The same Azure metric scrape target with identical dimensions
    /// 3. The same time granularity 
    /// 4. The same filters
    /// </summary>
    public class BatchScrapeDefinition<TResourceDefinition> where TResourceDefinition : class, IAzureResourceDefinition
    {
        /// <summary>
        /// Creates a new instance of the <see cref="BatchScrapeDefinition{TResourceDefinition}"/> class.
        /// </summary>
        /// <param name="scrapeDefinitionBatchProperties">Shared Properties Among ScrapeDefinition's in the batch</param>
        /// <param name="groupedScrapeDefinitions">Scape definitions in the batch</param>
        public BatchScrapeDefinition(ScrapeDefinitionBatchProperties scrapeDefinitionBatchProperties, List<ScrapeDefinition<TResourceDefinition>> groupedScrapeDefinitions)
        {
            Guard.NotNull(groupedScrapeDefinitions, nameof(groupedScrapeDefinitions));
            Guard.NotLessThan(groupedScrapeDefinitions.Count, 1, nameof(groupedScrapeDefinitions));
            Guard.NotNull(scrapeDefinitionBatchProperties, nameof(scrapeDefinitionBatchProperties));
                        
            ScrapeDefinitionBatchProperties = scrapeDefinitionBatchProperties;
            ScrapeDefinitions = groupedScrapeDefinitions;
        }

        /// <summary>
        /// A batch of scrape job definitions to be executed as a single request 
        /// </summary>
        public List<ScrapeDefinition<TResourceDefinition>> ScrapeDefinitions { get; set; } = new List<ScrapeDefinition<TResourceDefinition>>();

        public ScrapeDefinitionBatchProperties ScrapeDefinitionBatchProperties { get; set; }
    }
}