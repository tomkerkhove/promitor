using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Metrics.Sinks;

namespace Promitor.Integrations.Sinks.OpenTelemetry
{
    public class OpenTelemetryCollectorMetricSink : IMetricSink
    {
        private readonly ILogger<OpenTelemetryCollectorMetricSink> _logger;
        private static readonly Meter AzureMonitorMeter = new Meter("Promitor.Scraper.Metrics.AzureMonitor", "1.0");

        public MetricSinkType Type { get; } = MetricSinkType.OpenTelemetryCollector;

        public OpenTelemetryCollectorMetricSink(ILogger<OpenTelemetryCollectorMetricSink> logger)
        {
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
        }

        public async Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapeResult)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));
            Guard.NotNull(scrapeResult, nameof(scrapeResult));
            Guard.NotNull(scrapeResult.MetricValues, nameof(scrapeResult.MetricValues));

            var reportMetricTasks = new List<Task>();

            foreach (var measuredMetric in scrapeResult.MetricValues)
            {
                var metricValue = measuredMetric.Value ?? 0;

                var reportMetricTask = ReportMetricAsync(metricName, metricDescription, metricValue, new Dictionary<string, string>());
                reportMetricTasks.Add(reportMetricTask);
            }

            await Task.WhenAll(reportMetricTasks);
        }

        public async Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            // TODO: Switch to gauge
            var counter = AzureMonitorMeter.CreateCounter<double>(metricName, description: metricDescription);
            counter.Add(metricValue);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was pushed to OpenTelemetry Collector", metricName, metricValue);
        }
    }
}
