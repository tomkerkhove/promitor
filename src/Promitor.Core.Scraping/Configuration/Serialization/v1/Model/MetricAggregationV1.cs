﻿using System;
using Promitor.Core.Metrics;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    /// <summary>
    /// Contains aggregation settings for a single metric (vs the global aggregation settings).
    /// </summary>
    public class MetricAggregationV1
    {
        /// <summary>
        /// The time period the metric should be aggregated over.
        /// </summary>
        public TimeSpan? Interval { get; set; }

        /// <summary>
        /// Type of aggregation to query the Azure Monitor metric.
        /// </summary>
        public PromitorMetricAggregationType? Type { get; set; }
    }
}
