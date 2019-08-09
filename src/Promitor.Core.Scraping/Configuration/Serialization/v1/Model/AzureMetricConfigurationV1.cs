namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetricConfigurationV1
    {
        /// <summary>
        ///     Name of the Azure Monitor metric to query
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        ///     Configuration on how to aggregate the metric
        /// </summary>
        public MetricAggregationV1 Aggregation { get; set; }

        public Configuration.Model.AzureMetricConfiguration Build()
        {
            return new Configuration.Model.AzureMetricConfiguration
            {
                MetricName = MetricName,
                Aggregation = Aggregation.Build()
            };
        }
    }
}