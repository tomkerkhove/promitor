﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;

namespace Promitor.Core.Scraping.Sinks
{
    public class MetricSinkWriter
    {
        private readonly List<IMetricSink> _configuredSinks;
        private ILogger Logger { get; }

        public MetricSinkWriter(IEnumerable<IMetricSink> configuredSinks, ILogger<MetricSinkWriter> logger)
        {
            var metricSinks = configuredSinks?.ToList();
            Guard.NotNull(metricSinks, nameof(configuredSinks));
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
            _configuredSinks = metricSinks;
        }

        public async Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapedMetricResult)
        {
            var reportTasks = new List<Task>();
            foreach (var sink in _configuredSinks)
            {
                var reportTask = ReportMetricAsync(sink, metricName, metricDescription, scrapedMetricResult);
                reportTasks.Add(reportTask);
            }

            await Task.WhenAll(reportTasks);
        }

        public async Task ReportMetricAsync(IMetricSink sink, string metricName, string metricDescription, ScrapeResult scrapedMetricResult)
        {
            foreach (var measuredMetric in scrapedMetricResult.MetricValues)
            {
                try
                {
                    await sink.ReportMetricAsync(metricName, metricDescription, measuredMetric);
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, "Failed to write {MetricName} metric for sink {SinkType}", metricName, sink.Type);
                }
            }
        }
    }
}