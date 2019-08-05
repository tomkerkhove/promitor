using System;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AggregationBuilder
    {
        /// <summary>
        /// The time period the metric should be aggregated over.
        /// </summary>
        public TimeSpan? Interval { get; set; }

        public Aggregation Build()
        {
            return new Aggregation
            {
                Interval = Interval
            };
        }
    }
}