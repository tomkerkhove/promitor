using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus.Client;
using Promitor.Core;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Integrations.Sinks.Prometheus.Configuration;

namespace Promitor.Integrations.Sinks.Prometheus
{
    public class PrometheusScrapingEndpointMetricSink : IMetricSink
    {
        private readonly ILogger<PrometheusScrapingEndpointMetricSink> _logger;
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> _prometheusConfiguration;

        public PrometheusScrapingEndpointMetricSink(IMetricsDeclarationProvider metricsDeclarationProvider, IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> prometheusConfiguration, ILogger<PrometheusScrapingEndpointMetricSink> logger)
        {
            Guard.NotNull(prometheusConfiguration, nameof(prometheusConfiguration));
            Guard.NotNull(logger, nameof(logger));

            _metricsDeclarationProvider = metricsDeclarationProvider;
            _prometheusConfiguration = prometheusConfiguration;
            _logger = logger;
        }

        public MetricSinkType Type => MetricSinkType.PrometheusScrapingEndpoint;

        public async Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapeResult)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));
            Guard.NotNull(scrapeResult, nameof(scrapeResult));
            Guard.NotNull(scrapeResult.MetricValues, nameof(scrapeResult.MetricValues));

            var reportMetricTasks = new List<Task>();

            foreach (var measuredMetric in scrapeResult.MetricValues)
            {
                var metricValue = measuredMetric.Value ?? 0;
                var metricDefinition = _metricsDeclarationProvider.GetPrometheusDefinition(metricName);

                var metricLabels = DetermineLabels(metricDefinition, scrapeResult, measuredMetric);

                var reportMetricTask = ReportMetricAsync(metricName, metricDescription, metricValue, metricLabels);
                reportMetricTasks.Add(reportMetricTask);
            }

            await Task.WhenAll(reportMetricTasks);
        }

        public Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            var enableMetricTimestamps = _prometheusConfiguration.CurrentValue.EnableMetricTimestamps;

            var gauge = Metrics.CreateGauge(metricName, metricDescription, includeTimestamp: enableMetricTimestamps, labelNames: labels.Keys.ToArray());
            gauge.WithLabels(labels.Values.ToArray()).Set(metricValue);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to StatsD server", metricName, metricValue);

            return Task.CompletedTask;
        }

        private Dictionary<string, string> DetermineLabels(PrometheusMetricDefinition metricDefinition, ScrapeResult scrapeResult, MeasuredMetric measuredMetric)
        {
            var labels = new Dictionary<string, string>(scrapeResult.Labels);

            if (measuredMetric.IsDimensional)
            {
                labels.Add(measuredMetric.DimensionName.ToLower(), measuredMetric.DimensionValue);
            }

            if (metricDefinition?.Labels?.Any() == true)
            {
                foreach (var customLabel in metricDefinition.Labels)
                {
                    if (labels.ContainsKey(customLabel.Key))
                    {
                        _logger.LogWarning("Custom label {CustomLabelName} was already specified with value 'LabelValue' instead of 'CustomLabelValue'. Ignoring...", customLabel.Key, labels[customLabel.Key], customLabel.Value);
                        continue;
                    }

                    labels.Add(customLabel.Key, customLabel.Value);
                }
            }

            return labels;
        }
    }
}