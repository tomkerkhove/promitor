﻿using System.Collections.Generic;

namespace Promitor.Core.Metrics.Prometheus.Collectors.Interfaces
{
    public interface IPrometheusMetricsCollector
    {
        /// <summary>
        ///     Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        /// <param name="includeTimestamp">Indication whether or not a timestamp should be reported</param>
        void WriteGaugeMeasurement(string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp);
    }
}
