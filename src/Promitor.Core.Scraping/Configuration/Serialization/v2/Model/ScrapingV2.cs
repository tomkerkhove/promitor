namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    /// <summary>
    /// Contains settings about how promitor scrapes Azure metrics.
    /// </summary>
    public class ScrapingV2
    {
        /// <summary>
        /// A cron expression describing how often scrapes should occur.
        /// </summary>
        public string Schedule { get; set; }
    }
}
