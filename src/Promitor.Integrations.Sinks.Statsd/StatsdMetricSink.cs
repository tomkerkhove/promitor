using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using JustEat.StatsD;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Promitor.Core;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.Sinks.Statsd.Configuration;

namespace Promitor.Integrations.Sinks.Statsd
{
    public class StatsdMetricSink : IMetricSink
    {
        private readonly ILogger<StatsdMetricSink> _logger;
        private readonly IStatsDPublisher _statsDPublisher;
        private readonly IOptionsMonitor<StatsdSinkConfiguration> _statsDConfiguration;
        private readonly Dictionary<string, Func<string, string, double, Dictionary<string, string>, Task>> _reportMetricsActions;

        public StatsdMetricSink(IStatsDPublisher statsDPublisher, IOptionsMonitor<StatsdSinkConfiguration> configuration, ILogger<StatsdMetricSink> logger)
        {
            Guard.NotNull(statsDPublisher, nameof(statsDPublisher));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(configuration, nameof(configuration));

            _statsDPublisher = statsDPublisher;
            _statsDConfiguration = configuration;
            _logger = logger;
        }

        public MetricSinkType Type => MetricSinkType.StatsD;

        public async Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapeResult)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));
            Guard.NotNull(scrapeResult, nameof(scrapeResult));
            Guard.NotNull(scrapeResult.MetricValues, nameof(scrapeResult.MetricValues));

            var reportMetricTasks = new List<Task>();
            var formatterType = _statsDConfiguration.CurrentValue?.MetricFormat ?? StatsdFormatterTypesEnum.Default;

            foreach (var measuredMetric in scrapeResult.MetricValues)
            {
                var metricValue = measuredMetric.Value ?? 0;

                switch (formatterType)
                {
                    case StatsdFormatterTypesEnum.Default:
                        reportMetricTasks.Add(ReportMetricAsync(metricName, metricDescription, metricValue, scrapeResult.Labels));
                        break;
                    case StatsdFormatterTypesEnum.Geneva:
                        reportMetricTasks.Add(ReportMetricWithGenevaFormattingAsync(metricName, metricDescription, metricValue, scrapeResult.Labels));
                        break;
default:
  throw new ArgumentOutOfRangeException(nameof(formatterType), $"{formatterType} is not supported as formatting type.");
                }

                await Task.WhenAll(reportMetricTasks);
            }
        }

        public Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            _statsDPublisher.Gauge(metricValue, metricName);

            LogMetricWritten(metricName, metricValue);

            return Task.CompletedTask;
        }

        private Task ReportMetricWithGenevaFormattingAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));
            Guard.NotNull(_statsDConfiguration.CurrentValue, nameof(_statsDConfiguration.CurrentValue));
            Guard.NotNull(_statsDConfiguration.CurrentValue.Geneva, nameof(_statsDConfiguration.CurrentValue.Geneva));

            var bucket = JsonConvert.SerializeObject(new
            {
                Account = _statsDConfiguration.CurrentValue.Geneva.Account,
                Namespace = _statsDConfiguration.CurrentValue.Geneva.Namespace,
                Metric = metricName,
                Dims = labels
            });

            _statsDPublisher.Gauge(metricValue, bucket);

            LogMetricWritten(metricName, metricValue);

            return Task.CompletedTask;
        }

        private void LogMetricWritten(string metricName, double metricValue)
        {
            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to StatsD server", metricName, metricValue);
        }
    }
}