using System.Collections.Generic;

namespace Promitor.Core.Metrics
{
    public interface IRuntimeMetricsCollector
    {
        /// <summary>
        ///     Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        void SetGaugeMeasurement(string name, string description, double value, Dictionary<string, string> labels);
    }
}