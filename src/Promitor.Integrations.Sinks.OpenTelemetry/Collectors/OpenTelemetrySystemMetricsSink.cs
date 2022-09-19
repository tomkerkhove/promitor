using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Promitor.Core.Metrics.Interfaces;

namespace Promitor.Integrations.Sinks.OpenTelemetry.Collectors
{
    public class OpenTelemetrySystemMetricsSink : ISystemMetricsSink
    {
        private readonly OpenTelemetryCollectorMetricSink _metricSink;

        public OpenTelemetrySystemMetricsSink(OpenTelemetryCollectorMetricSink metricSink)
        {
            Guard.NotNull(metricSink, nameof(metricSink));

            _metricSink = metricSink;
        }

        public async Task WriteGaugeMeasurementAsync(string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp)
        {
            await _metricSink.ReportMetricAsync(name, description, value, labels);
        }
    }
}
