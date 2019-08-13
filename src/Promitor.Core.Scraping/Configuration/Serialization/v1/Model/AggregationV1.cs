﻿using System;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AggregationV1
    {
        /// <summary>
        /// The time period the metric should be aggregated over.
        /// </summary>
        public TimeSpan? Interval { get; set; }
    }
}