using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using JustEat.StatsD;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Sinks;

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

        public async Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapeResult, MeasuredMetric measuredMetric)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));
            Guard.NotNull(measuredMetric, nameof(measuredMetric));

            var metricValue = measuredMetric.Value ?? 0;

            await ReportMetricAsync(metricName, metricDescription, metricValue, new Dictionary<string, string>());
        }

        public Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            _statsDPublisher.Gauge(metricValue, metricName);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to StatsD server", metricName, metricValue);

            return Task.CompletedTask;
        }
    }
}
