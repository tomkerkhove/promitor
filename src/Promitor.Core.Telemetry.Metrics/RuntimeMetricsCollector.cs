using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Telemetry.Metrics.Interfaces;

namespace Promitor.Core.Telemetry.Metrics
{
    public class RuntimeMetricsCollector : IRuntimeMetricsCollector
    {
        private readonly IOptionsMonitor<PrometheusConfiguration> _prometheusConfiguration;

        public RuntimeMetricsCollector(IOptionsMonitor<PrometheusConfiguration> prometheusConfiguration)
        {
            _prometheusConfiguration = prometheusConfiguration;
        }

        /// <summary>
        /// Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        public void SetGaugeMeasurement(string name, string description, double value, Dictionary<string, string> labels)
        {
            var enableMetricTimestamps = _prometheusConfiguration.CurrentValue.EnableMetricTimestamps;

            var gauge = Prometheus.Client.Metrics.CreateGauge($"promitor_{name}", help: description, includeTimestamp: enableMetricTimestamps, labelNames: labels.Keys.ToArray());
            gauge.WithLabels(labels.Values.ToArray()).Set(value);
        }
    }
}
