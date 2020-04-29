using System.Threading.Tasks;
using GuardNet;
using JustEat.StatsD;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Sinks;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Integrations.Sinks.Statsd
{
    public class StatsdMetricSink : IMetricSink
    {
        private readonly ILogger<StatsdMetricSink> _logger;
        private readonly IStatsDPublisher _statsDPublisher;

        public StatsdMetricSink(IStatsDPublisher statsDPublisher, ILogger<StatsdMetricSink> logger)
        {
            Guard.NotNull(statsDPublisher, nameof(statsDPublisher));
            Guard.NotNull(logger, nameof(logger));

            _statsDPublisher = statsDPublisher;
            _logger = logger;
        }

        public MetricSinkType Type => MetricSinkType.StatsD;

        public Task ReportMetricAsync(string metricName, string metricDescription, MeasuredMetric measuredMetric)
        {
            var metricValue = measuredMetric.Value ?? 0;
            _statsDPublisher.Gauge(metricValue, metricName);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to StatsD server", metricName, metricValue);

            return Task.CompletedTask;
        }
    }
}
