using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;

namespace Promitor.Core.Metrics
{
    public class AggregatedSystemMetricsCollector : ISystemMetricsCollector
    {
        private readonly IEnumerable<ISystemMetricsCollector> _metricCollectors;

        public AggregatedSystemMetricsCollector(IEnumerable<ISystemMetricsCollector> metricCollectors)
        {
            _metricCollectors = metricCollectors;
        }

        public async Task WriteGaugeMeasurementAsync (string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp)
        {
            if (_metricCollectors == null)
            {
                return;
            }

            foreach (var metricCollector in _metricCollectors)
            {
                if (metricCollector == null)
                {
                    continue;
                }

                await metricCollector.WriteGaugeMeasurementAsync(name, description, value, labels, includeTimestamp);
            }
        }
    }
}
