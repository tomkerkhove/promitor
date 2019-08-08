using System;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    /// <summary>
    /// Contains aggregation settings used when querying Azure metrics.
    /// </summary>
    public class AggregationV2
    {
        /// <summary>
        /// The aggregation interval to use when querying Azure metrics.
        /// </summary>
        public TimeSpan? Interval { get; set; }
    }
}
