namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    /// <summary>
    /// Contains settings about how promitor scrapes Azure metrics.
    /// </summary>
    public class ScrapingV1
    {
        /// <summary>
        /// A cron expression describing how often scrapes should occur.
        /// </summary>
        public string Schedule { get; set; }
    }
}
