using System;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetricConfigurationV1
    {
        /// <summary>
        ///     The name of the Azure metric to scrape.
        /// </summary>
        [Obsolete("Use Metric.Name instead")]
        public string MetricName { get; set; }

        /// <summary>
        ///     Configuration on the Azure Monitor metric to query
        public MetricInformationV1 Metric { get; set; }

        /// <summary>
        ///     Amount of maximum resources to limit the results to
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        ///     Information about the dimension of an Azure Monitor metric
        /// </summary>
        public MetricDimensionV1 Dimension { get; set; }

        /// <summary>
        ///     The settings for how the metric should be aggregated before being returned from Azure.
        /// </summary>
        public MetricAggregationV1 Aggregation { get; set; }
    }
}