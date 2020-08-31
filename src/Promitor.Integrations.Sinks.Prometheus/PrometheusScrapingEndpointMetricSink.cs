using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus.Client.Abstractions;
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
        private readonly IMetricFactory _metricFactory;
        private readonly ILogger<PrometheusScrapingEndpointMetricSink> _logger;
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> _prometheusConfiguration;

        public PrometheusScrapingEndpointMetricSink(IMetricFactory metricFactory, IMetricsDeclarationProvider metricsDeclarationProvider, IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> prometheusConfiguration, ILogger<PrometheusScrapingEndpointMetricSink> logger)
        {
            Guard.NotNull(metricFactory, nameof(metricFactory));
            Guard.NotNull(prometheusConfiguration, nameof(prometheusConfiguration));
            Guard.NotNull(logger, nameof(logger));

            _metricFactory = metricFactory;
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
                var metricValue = DetermineMetricMeasurement(measuredMetric);
                var metricDefinition = _metricsDeclarationProvider.GetPrometheusDefinition(metricName);

                var metricLabels = DetermineLabels(metricDefinition, scrapeResult, measuredMetric);

                var reportMetricTask = ReportMetricAsync(metricName, metricDescription, metricValue, metricLabels);
                reportMetricTasks.Add(reportMetricTask);
            }

            await Task.WhenAll(reportMetricTasks);
        }

        private double DetermineMetricMeasurement(MeasuredMetric scrapedMetricResult)
        {
            var metricUnavailableValue = _prometheusConfiguration.CurrentValue?.MetricUnavailableValue ?? Defaults.Prometheus.MetricUnavailableValue;
            return scrapedMetricResult.Value ?? metricUnavailableValue;
        }

        public Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            var enableMetricTimestamps = _prometheusConfiguration.CurrentValue.EnableMetricTimestamps;
            
            var gauge = CreateGauge(metricName, metricDescription, labels, enableMetricTimestamps);
            gauge.WithLabels(labels.Values.ToArray()).Set(metricValue);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to StatsD server", metricName, metricValue);

            return Task.CompletedTask;
        }

        private IMetricFamily<IGauge> CreateGauge(string metricName, string metricDescription, Dictionary<string, string> labels, bool enableMetricTimestamps)
        {
            var gauge = _metricFactory.CreateGauge(metricName, metricDescription, includeTimestamp: enableMetricTimestamps, labelNames: labels.Keys.ToArray());
            return gauge;
        }

        private Dictionary<string, string> DetermineLabels(PrometheusMetricDefinition metricDefinition, ScrapeResult scrapeResult, MeasuredMetric measuredMetric)
        {
            var labels = new Dictionary<string, string>(scrapeResult.Labels.Select(label => new KeyValuePair<string, string>(label.Key.SanitizeForPrometheusLabelKey(), label.Value)));

            if (measuredMetric.IsDimensional)
            {
                labels.Add(measuredMetric.DimensionName.SanitizeForPrometheusLabelKey(), measuredMetric.DimensionValue);
            }

            if (metricDefinition?.Labels?.Any() == true)
            {
                foreach (var customLabel in metricDefinition.Labels)
                {
                    var customLabelKey = customLabel.Key.SanitizeForPrometheusLabelKey();
                    if (labels.ContainsKey(customLabelKey))
                    {
                        _logger.LogWarning("Custom label {CustomLabelName} was already specified with value 'LabelValue' instead of 'CustomLabelValue'. Ignoring...", customLabel.Key, labels[customLabelKey], customLabel.Value);
                        continue;
                    }

                    labels.Add(customLabelKey, customLabel.Value);
                }
            }

            return labels;
        }
    }
}