﻿namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetricConfigurationV1
    {
        /// <summary>
        /// The name of the Azure metric to scrape.
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// Name of the dimension to use for a given Azure Monitor metric.
        /// </summary>
        public string DimensionName { get; set; }

        /// <summary>
        /// The settings for how the metric should be aggregated before being returned from Azure.
        /// </summary>
        public MetricAggregationV1 Aggregation { get; set; }
    }
}
