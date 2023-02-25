using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;

namespace Promitor.Core.Metrics.Sinks
{
    public class MetricSinkWriter
    {
        private readonly List<IMetricSink> _configuredSinks;
        private ILogger Logger { get; }

        public List<MetricSinkType> EnabledMetricSinks { get; }

        public MetricSinkWriter(IEnumerable<IMetricSink> configuredSinks, ILogger<MetricSinkWriter> logger)
        {
            var metricSinks = configuredSinks?.ToList();
            Guard.NotNull(metricSinks, nameof(configuredSinks));
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
            _configuredSinks = metricSinks;
            // ReSharper disable once AssignNullToNotNullAttribute (see guard above)
            this.EnabledMetricSinks = metricSinks.Select(x => x.Type).Distinct().ToList();
        }

        public async Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapedMetricResult)
        {
            Guard.NotNullOrWhitespace(metricName, nameof(metricName));
            Guard.NotNull(scrapedMetricResult, nameof(scrapedMetricResult));

            var reportTasks = new List<Task>();
            foreach (var sink in _configuredSinks)
            {
                var reportTask = ReportMetricAsync(sink, metricName, metricDescription, scrapedMetricResult);
                reportTasks.Add(reportTask);
            }

            await Task.WhenAll(reportTasks);
        }

        private async Task ReportMetricAsync(IMetricSink sink, string metricName, string metricDescription, ScrapeResult scrapedMetricResult)
        {
            Guard.NotNull(sink, nameof(sink));
            Guard.NotNullOrWhitespace(metricName, nameof(metricName));
            Guard.NotNull(scrapedMetricResult, nameof(scrapedMetricResult));
            Guard.NotNull(scrapedMetricResult.MetricValues, nameof(scrapedMetricResult.MetricValues));

            try
            {
                await sink.ReportMetricAsync(metricName, metricDescription, scrapedMetricResult);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Failed to write {MetricName} metric for sink {SinkType}", metricName, sink.Type);
            }
        }

        public async Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> metricLabels)
        {
            Guard.NotNullOrWhitespace(metricName, nameof(metricName));
            Guard.NotNull(metricLabels, nameof(metricLabels));

            var reportTasks = new List<Task>();
            foreach (var sink in _configuredSinks)
            {
                var reportTask = sink.ReportMetricAsync(metricName, metricDescription, metricValue, metricLabels);
                reportTasks.Add(reportTask);
            }

            await Task.WhenAll(reportTasks);
        }
    }
}