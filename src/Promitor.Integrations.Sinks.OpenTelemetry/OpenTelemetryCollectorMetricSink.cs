#nullable enable
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;
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
        private static readonly Meter azureMonitorMeter = new Meter("Promitor.Scraper.Metrics.AzureMonitor", "1.0");

        public MetricSinkType Type => MetricSinkType.OpenTelemetryCollector;

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
                
                var reportMetricTask = ReportMetricAsync(metricName, metricDescription, metricValue, scrapeResult.Labels);
                reportMetricTasks.Add(reportMetricTask);
            }

            await Task.WhenAll(reportMetricTasks);
        }

        private readonly ConcurrentDictionary<string, ObservableGauge<double>> _gauges = new ConcurrentDictionary<string, ObservableGauge<double>>();
        private readonly ConcurrentDictionary<string, HashSet<Measurement<double>>> _measurements = new ConcurrentDictionary<string, HashSet<Measurement<double>>>();

        public Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            // TODO: Move to factory instead?
            if (_gauges.ContainsKey(metricName) == false)
            {
                InitializeNewMetric(metricName, metricDescription);
            }

            var composedTags = labels.Select(kvp => new KeyValuePair<string, object?>(kvp.Key, kvp.Value)).ToArray();
            var newMeasurement = new Measurement<double>(metricValue, composedTags);
            _measurements[metricName].Add(newMeasurement);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was pushed to OpenTelemetry Collector", metricName, metricValue);

            return Task.CompletedTask;
        }

        private void InitializeNewMetric(string metricName, string metricDescription)
        {
            var gauge = azureMonitorMeter.CreateObservableGauge<double>(metricName, description: metricDescription, observeValues: () => ReportMeasurementsForMetric(metricName));
            _gauges.TryAdd(metricName, gauge);

            _measurements.TryAdd(metricName, new HashSet<Measurement<double>>());
        }

        private IEnumerable<Measurement<double>> ReportMeasurementsForMetric(string metricName)
        {
            var recordedMeasurements = _measurements[metricName];

            var measurementsToReport = Interlocked.Exchange(ref recordedMeasurements, new HashSet<Measurement<double>>());

            return measurementsToReport;
        }
    }
}
