using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Microsoft.Extensions.Options;
using Prometheus.Client.Abstractions;
using Promitor.Core.Metrics;
using Promitor.Integrations.Sinks.Prometheus.Configuration;

namespace Promitor.Integrations.Sinks.Prometheus
{
    public class RuntimeMetricsCollector : IRuntimeMetricsCollector
    {
        private readonly IMetricFactory _metricFactory;
        private readonly IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> _prometheusConfiguration;

        public RuntimeMetricsCollector(IMetricFactory metricFactory, IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> prometheusConfiguration)
        {
            Guard.NotNull(metricFactory, nameof(metricFactory));

            _metricFactory = metricFactory;
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

            var gauge = _metricFactory.CreateGauge(name, help: description, includeTimestamp: enableMetricTimestamps, labelNames: labels.Keys.ToArray());
            gauge.WithLabels(labels.Values.ToArray()).Set(value);
        }
    }
}
