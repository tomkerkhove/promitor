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

        /// <summary>
        /// How far behind the most recent time grain the reported value should be read from.
        /// </summary>
        public TimeSpan? Offset { get; set; }
    }
}
