using System;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Integrations.Sinks.Core
{
    public abstract class MetricSink : IMetricSink
    {
        private ILogger Logger { get; }

        protected MetricSink(ILogger<MetricSink> logger)
        {
            Guard.NotNull(logger,nameof(logger));

            Logger = logger;
        }

        public void ReportMetric(string metricName, string metricDescription, ScrapeResult scrapedMetricResult)
        {
            foreach (var measuredMetric in scrapedMetricResult.MetricValues)
            {
                try
                {
                    ReportMetric(metricName, metricDescription, measuredMetric);
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, "Failed to write {MetricName} metric for sink {SinkType}", metricName, SinkType);
                }
            }
        }

        public abstract MetricSinkType SinkType { get; }
        public abstract void ReportMetric(string metricName, string metricDescription, MeasuredMetric measuredMetric);
    }
}