﻿using System;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    /// <summary>
    /// Contains aggregation settings for a single metric (vs the global aggregation settings).
    /// </summary>
    public class MetricAggregationV2
    {
        public AggregationType? Type { get; set; }

        /// <summary>
        /// The aggregation interval to use when querying Azure metrics.
        /// </summary>
        public TimeSpan? Interval { get; set; }
    }
}
