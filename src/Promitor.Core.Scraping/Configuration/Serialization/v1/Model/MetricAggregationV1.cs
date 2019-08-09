﻿using System;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricAggregationV1
    {
        /// <summary>
        /// The time period the metric should be aggregated over.
        /// </summary>
        public TimeSpan? Interval { get; set; }

        /// <summary>
        ///     Type of aggregation to query the Azure Monitor metric
        /// </summary>
        public AggregationType Type { get; set; }
    }
}