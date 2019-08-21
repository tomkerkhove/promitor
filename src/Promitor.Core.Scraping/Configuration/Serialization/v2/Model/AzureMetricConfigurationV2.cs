namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    public class AzureMetricConfigurationV2
    {
        /// <summary>
        /// The name of the Azure metric to scrape.
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// The settings for how the metric should be aggregated before being returned from Azure.
        /// </summary>
        public MetricAggregationV2 Aggregation { get; set; } = new MetricAggregationV2();
    }
}
