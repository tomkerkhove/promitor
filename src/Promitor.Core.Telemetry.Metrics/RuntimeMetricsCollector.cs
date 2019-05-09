using System.Collections.Generic;
using System.Linq;
using Promitor.Core.Infrastructure;
using Promitor.Core.Telemetry.Metrics.Interfaces;

namespace Promitor.Core.Telemetry.Metrics
{
    public class RuntimeMetricsCollector : IRuntimeMetricsCollector
    {
        /// <summary>
        /// Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        public void SetGaugeMeasurement(string name, string description, double value, Dictionary<string, string> labels)
        {
            var metricsTimestampFeatureFlag = FeatureFlag.IsActive(FeatureFlag.Names.MetricsTimestamp);

            var gauge = Prometheus.Client.Metrics.CreateGauge($"promitor_{name}", help: description, includeTimestamp: metricsTimestampFeatureFlag, labelNames: labels.Keys.ToArray());
            gauge.WithLabels(labels.Values.ToArray()).Set(value);
        }
    }
}
