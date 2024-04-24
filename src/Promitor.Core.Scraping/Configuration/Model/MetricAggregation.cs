
using Promitor.Core.Metrics;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class MetricAggregation : Aggregation
    {
        /// <summary>
        ///     Type of aggregation to query the Azure Monitor metric
        /// </summary>
        public PromitorMetricAggregationType Type { get; set; }
    }
}