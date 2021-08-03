﻿using System;
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
using Promitor.Integrations.Sinks.Prometheus.Labels;

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
                var defaultLabels = _metricsDeclarationProvider.GetDefaultLabels();
                
                var metricLabels = DetermineLabels(metricDefinition, scrapeResult, measuredMetric, defaultLabels);

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

            var orderedLabels = labels.OrderByDescending(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var gauge = CreateGauge(metricName, metricDescription, orderedLabels, enableMetricTimestamps);
            gauge.WithLabels(orderedLabels.Values.ToArray()).Set(metricValue);

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to StatsD server", metricName, metricValue);

            return Task.CompletedTask;
        }

        private IMetricFamily<IGauge> CreateGauge(string metricName, string metricDescription, Dictionary<string, string> labels, bool enableMetricTimestamps)
        {
            var gauge = _metricFactory.CreateGauge(metricName, metricDescription, includeTimestamp: enableMetricTimestamps, labelNames: labels.Keys.ToArray());
            return gauge;
        }

        private Dictionary<string, string> DetermineLabels(PrometheusMetricDefinition metricDefinition, ScrapeResult scrapeResult, MeasuredMetric measuredMetric, Dictionary<string, string> defaultLabels)
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
                        _logger.LogWarning("Custom label {CustomLabelName} was already specified with value '{LabelValue}' instead of '{CustomLabelValue}'. Ignoring...", customLabel.Key, labels[customLabelKey], customLabel.Value);
                        continue;
                    }

                    labels.Add(customLabelKey, customLabel.Value);
                }
            }

            foreach (var defaultLabel in defaultLabels)
            {
                var defaultLabelKey = defaultLabel.Key.SanitizeForPrometheusLabelKey();
                if (labels.ContainsKey(defaultLabelKey) == false)
                {
                    labels.Add(defaultLabelKey, defaultLabel.Value);
                }
            }

            // Add the tenant id
            var metricsDeclaration = _metricsDeclarationProvider.Get(applyDefaults: true);
            if (labels.ContainsKey("tenant_id") == false)
            {
                labels.Add("tenant_id", metricsDeclaration.AzureMetadata.TenantId);
            }

            // Transform labels, if need be
            if (_prometheusConfiguration.CurrentValue.Labels != null)
            {
                labels = LabelTransformer.TransformLabels(_prometheusConfiguration.CurrentValue.Labels.Transformation,labels);
            }

            var orderedLabels = labels.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return orderedLabels;
        }
    }
}
