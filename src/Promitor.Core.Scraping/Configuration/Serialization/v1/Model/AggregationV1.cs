using System;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    /// <summary>
    /// Contains aggregation settings used when querying Azure metrics.
    /// </summary>
    public class AggregationV1
    {
        /// <summary>
        /// The interval to use for aggregating the metric data when querying Azure metrics.
        /// </summary>
        public TimeSpan? Interval { get; set; }
    }
}
