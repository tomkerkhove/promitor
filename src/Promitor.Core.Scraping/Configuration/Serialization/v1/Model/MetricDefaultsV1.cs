namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    /// <summary>
    /// Contains default settings that apply to all metrics.
    /// </summary>
    public class MetricDefaultsV1
    {
        /// <summary>
        /// The default aggregation settings to use when querying metrics from Azure.
        /// </summary>
        public AggregationV1 Aggregation { get; set; }

        /// <summary>
        /// The default scraping settings.
        /// </summary>
        public ScrapingV1 Scraping { get; set; }
    }
}
