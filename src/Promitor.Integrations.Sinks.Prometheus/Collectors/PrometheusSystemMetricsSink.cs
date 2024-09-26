using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Prometheus.Client;
using Promitor.Core.Metrics.Interfaces;

namespace Promitor.Integrations.Sinks.Prometheus.Collectors
{
    public class PrometheusSystemMetricsSink : ISystemMetricsSink
    {
        private readonly IMetricFactory _metricFactory;

        public PrometheusSystemMetricsSink(IMetricFactory metricFactory)
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
        public Task WriteGaugeMeasurementAsync(string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp)
        {
            // Order labels alphabetically
            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var gauge = _metricFactory.CreateGauge(name, help: description, includeTimestamp: includeTimestamp, labelNames: orderedLabels.Keys.ToArray());
            gauge.WithLabels(orderedLabels.Values.ToArray()).Set(value);

            return Task.CompletedTask;
        }
        
        /// <summary>
        ///     Records measurement for a histogram instrument 
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        /// <param name="includeTimestamp">Indication whether or not a timestamp should be reported</param>
        public Task WriteHistogramMeasurementAsync(string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp)
        {
            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var histogram = _metricFactory.CreateHistogram(name, help: description, includeTimestamp: includeTimestamp, labelNames: orderedLabels.Keys.ToArray(), buckets: [1, 2, 4, 8, 16, 32, 64]);
            histogram.WithLabels(orderedLabels.Values.ToArray()).Observe(value);
            return Task.CompletedTask;
        }
    }
}
