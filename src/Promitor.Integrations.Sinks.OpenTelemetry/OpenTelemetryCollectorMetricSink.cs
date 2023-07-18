#nullable enable
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Integrations.Sinks.Core;

namespace Promitor.Integrations.Sinks.OpenTelemetry
{
    public class OpenTelemetryCollectorMetricSink : MetricSink, IMetricSink
    {
        private readonly ILogger<OpenTelemetryCollectorMetricSink> _logger;
        private static readonly Meter azureMonitorMeter = new("Promitor.Scraper.Metrics.AzureMonitor", "1.0");

        public MetricSinkType Type => MetricSinkType.OpenTelemetryCollector;

        public OpenTelemetryCollectorMetricSink(IMetricsDeclarationProvider metricsDeclarationProvider, ILogger<OpenTelemetryCollectorMetricSink> logger)
            : base(metricsDeclarationProvider, logger)
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
                
                var metricLabels = DetermineLabels(metricName, scrapeResult, measuredMetric);

                var reportMetricTask = ReportMetricAsync(metricName, metricDescription, metricValue, metricLabels);
                reportMetricTasks.Add(reportMetricTask);
            }

            await Task.WhenAll(reportMetricTasks);
        }

        private readonly ConcurrentDictionary<string, ObservableGauge<double>> _gauges = new();
        private readonly ConcurrentDictionary<string, Channel<Measurement<double>>> _measurements = new();

        public async Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            // TODO: Move to factory instead? 
            if (_gauges.ContainsKey(metricName) == false)
            {
                InitializeNewMetric(metricName, metricDescription);
            }

            var composedTags = labels.Select(kvp => new KeyValuePair<string, object?>(kvp.Key, kvp.Value)).ToArray();
            var newMeasurement = new Measurement<double>(metricValue, composedTags);
            var channelWriter = _measurements[metricName].Writer;
            await channelWriter.WriteAsync(newMeasurement);
            
            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was pushed to OpenTelemetry Collector", metricName, metricValue);
        }

        private void InitializeNewMetric(string metricName, string metricDescription)
        {
            var gauge = azureMonitorMeter.CreateObservableGauge(metricName, description: metricDescription, observeValues: () => ReportMeasurementsForMetricAsync(metricName).Result);
            _gauges.TryAdd(metricName, gauge);

            _measurements.TryAdd(metricName, CreateNewMeasurementChannel());
        }

        private async Task<IEnumerable<Measurement<double>>> ReportMeasurementsForMetricAsync(string metricName)
        {
            List<Measurement<double>> measurementsToReport = new List<Measurement<double>>();
            var channel = _measurements[metricName];
            
            var totalCount = channel.Reader.Count;
            var readItems = 0;
            do
            {
                var item = await channel.Reader.ReadAsync();
                measurementsToReport.Add(item);
                readItems++;
            }
            while (readItems < totalCount);

            return measurementsToReport;
        }

        static Channel<Measurement<double>> CreateNewMeasurementChannel()
        {
            return Channel.CreateUnbounded<Measurement<double>>();
        }
    }
}
