using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Core.Metrics.Interfaces;

namespace Promitor.Core.Metrics
{
    public class AggregatedSystemMetricsPublisher : ISystemMetricsPublisher
    {
        private readonly IEnumerable<ISystemMetricsSink> _metricSinks;

        public AggregatedSystemMetricsPublisher(IEnumerable<ISystemMetricsSink> metricSinks)
        {
            _metricSinks = metricSinks;
        }

        public async Task WriteGaugeMeasurementAsync (string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp)
        {
            if (_metricSinks == null)
            {
                return;
            }

            foreach (var metricCollector in _metricSinks)
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
