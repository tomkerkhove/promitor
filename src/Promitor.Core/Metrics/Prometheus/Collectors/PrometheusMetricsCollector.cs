using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Prometheus.Client;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;

namespace Promitor.Core.Metrics.Prometheus.Collectors
{
    public class PrometheusMetricsCollector : IPrometheusMetricsCollector
    {
        private readonly IMetricFactory _metricFactory;

        public PrometheusMetricsCollector(IMetricFactory metricFactory)
        {
            Guard.NotNull(metricFactory, nameof(metricFactory));

            _metricFactory = metricFactory;
        }

        /// <summary>
        ///     Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        /// <param name="includeTimestamp">Indication whether or not a timestamp should be reported</param>
        public void WriteGaugeMeasurement(string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp)
        {
            // Order labels alphabetically
            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var gauge = _metricFactory.CreateGauge(name, help: description, includeTimestamp: true, labelNames: orderedLabels.Keys.ToArray());
            gauge.WithLabels(orderedLabels.Values.ToArray()).Set(value);
        }
    }
}
