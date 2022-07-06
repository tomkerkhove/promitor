using System.Threading;

namespace Promitor.Agents.Scraper.Scheduling
{
    /// <inheritdoc cref="IScrapingMutex"/>
    public class ScrapingMutex : SemaphoreSlim, IScrapingMutex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapingMutex"/> class, specifying
        /// the number scraping tasks that can be processed concurrently.
        /// </summary>
        /// <param name="concurrency">The number scraping tasks that can be processed concurrently.</param>
        public ScrapingMutex(int concurrency) 
            : base(concurrency, concurrency)
        {
        }
    }
}