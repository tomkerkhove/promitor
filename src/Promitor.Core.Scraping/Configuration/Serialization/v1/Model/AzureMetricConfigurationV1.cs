using System;
using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetricConfigurationV1
    {
        /// <summary>
        ///     The name of the Azure metric to scrape.
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        ///     Amount of maximum resources to limit the results to
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        ///     Information about the dimensions of an Azure Monitor metric
        /// </summary>
        public IReadOnlyCollection<MetricDimensionV1> Dimensions { get; set; }

        /// <summary>
        ///     Information about the dimension of an Azure Monitor metric
        /// </summary>
        [Obsolete("Dimension is deprecated, please use Dimensions instead.")]
        public MetricDimensionV1 Dimension { get; set; }

        /// <summary>
        ///     The settings for how the metric should be aggregated before being returned from Azure.
        /// </summary>
        public MetricAggregationV1 Aggregation { get; set; }
    }
}