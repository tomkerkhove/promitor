namespace Promitor.Core.Scraping.Configuration.Model
{
    public class AzureMetricConfiguration
    {
        /// <summary>
        ///     Name of the Azure Monitor metric to query
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        ///     Information about the dimension of an Azure Monitor metric
        /// </summary>
        public MetricDimension Dimension { get; set; }

        /// <summary>
        ///     Configuration on how to aggregate the metric
        /// </summary>
        public MetricAggregation Aggregation { get; set; }
    }
}