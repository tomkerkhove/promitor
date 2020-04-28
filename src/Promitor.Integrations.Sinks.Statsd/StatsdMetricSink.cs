using GuardNet;
using JustEat.StatsD;
using Microsoft.Extensions.Logging;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.Sinks.Core;

namespace Promitor.Integrations.Sinks.Statsd
{
    public class StatsdMetricSink : MetricSink, IMetricSink
    {
        private readonly IStatsDPublisher _statsDPublisher;

        public StatsdMetricSink(IStatsDPublisher statsDPublisher, ILogger<StatsdMetricSink> logger)
        : base(logger)
        {
            Guard.NotNull(statsDPublisher, nameof(statsDPublisher));

            _statsDPublisher = statsDPublisher;
        }

        public override MetricSinkType SinkType => MetricSinkType.StatsD;

        public override void ReportMetric(string metricName, string metricDescription, MeasuredMetric measuredMetric)
        {
            var metricValue = measuredMetric.Value ?? 0;
            _statsDPublisher.Gauge(metricValue, metricName);
        }
    }
}
