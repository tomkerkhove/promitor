namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    /// <summary>
    /// Contains default settings that apply to all metrics.
    /// </summary>
    public class MetricDefaultsV2
    {
        /// <summary>
        /// The default aggregation settings to use when querying metrics from Azure.
        /// </summary>
        public AggregationV2 Aggregation { get; set; }

        /// <summary>
        /// The default scraping settings.
        /// </summary>
        public ScrapingV2 Scraping { get; set; }
    }
}
