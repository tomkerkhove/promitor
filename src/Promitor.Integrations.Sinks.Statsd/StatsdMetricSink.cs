using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.Internal.Mappers;
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

            _reportMetricsActions = new Dictionary<string, Func<string, string, double, Dictionary<string, string>, Task>>
            {
                {  StatsdFormatterTypes.DEFAULT, new Func<string, string, double, Dictionary<string, string>, Task>(ReportMetricAsync) },
                {  StatsdFormatterTypes.CUSTOM, new Func<string, string, double, Dictionary<string, string>, Task>(ReportCustomFormattedMetricAsync) }
            };
        }

        public MetricSinkType Type => MetricSinkType.StatsD;

        public async Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapeResult)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));
            Guard.NotNull(scrapeResult, nameof(scrapeResult));
            Guard.NotNull(scrapeResult.MetricValues, nameof(scrapeResult.MetricValues));

            var reportMetricTasks = new List<Task>();
            var formatterType = _statsDConfiguration.CurrentValue?.FormatterType ?? StatsdFormatterTypes.DEFAULT;

            foreach (var measuredMetric in scrapeResult.MetricValues)
            {
                var metricValue = measuredMetric.Value ?? 0;

                var reportMetricTask = ReportMetric(metricName, metricDescription, metricValue, scrapeResult.Labels);
                reportMetricTasks.Add(reportMetricTask);
            }

            await Task.WhenAll(reportMetricTasks);
        }

        public Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            _statsDPublisher.Gauge(metricValue, metricName);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to StatsD server", metricName, metricValue);

            return Task.CompletedTask;
        }

        public Task ReportCustomFormattedMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));
            Guard.NotNull(_statsDConfiguration.CurrentValue, nameof(_statsDConfiguration.CurrentValue));            

            var bucket = JsonConvert.SerializeObject(new { 
                Account = _statsDConfiguration.CurrentValue.Account, 
                Namespace = _statsDConfiguration.CurrentValue.Namespace, 
                Metric = metricName, 
                Dims = labels 
            });

            _statsDPublisher.Gauge(metricValue, bucket);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to StatsD server", metricName, metricValue);

            return Task.CompletedTask;
        }
    }
}