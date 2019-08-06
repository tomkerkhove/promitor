using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Prometheus.Client;
using Promitor.Core.Configuration.FeatureFlags;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Prometheus.Interfaces;

namespace Promitor.Core.Scraping.Prometheus
{
    public class PrometheusMetricWriter : IPrometheusMetricWriter
    {
        private readonly FeatureToggleClient _featureToggleClient;
        private readonly ILogger<PrometheusMetricWriter> _logger;

        public PrometheusMetricWriter(FeatureToggleClient featureToggleClient, ILogger<PrometheusMetricWriter> logger)
        {
            _featureToggleClient = featureToggleClient;
            _logger = logger;
        }

        public void ReportMetric(MetricDefinition metricDefinition, ScrapeResult scrapedMetricResult)
        {
            var metricsTimestampFeatureFlag = _featureToggleClient.IsActive(ToggleNames.DisableMetricTimestamps, defaultFlagState: true);

            var labels = DetermineLabels(metricDefinition, scrapedMetricResult);

            var gauge = Metrics.CreateGauge(metricDefinition.Name, metricDefinition.Description, includeTimestamp: metricsTimestampFeatureFlag, labelNames: labels.Names);
            var metricValue = DetermineMetricMeasurement(scrapedMetricResult);
            gauge.WithLabels(labels.Values).Set(metricValue);
        }

        private static double DetermineMetricMeasurement(ScrapeResult scrapedMetricResult)
        {
            return scrapedMetricResult.MetricValue ?? double.NaN;
        }

        private (string[] Names, string[] Values) DetermineLabels(MetricDefinition metricDefinition, ScrapeResult scrapeResult)
        {
            var labels = new Dictionary<string, string>(scrapeResult.Labels);

            if (metricDefinition?.Labels?.Any() == true)
            {
                foreach (var customLabel in metricDefinition.Labels)
                {
                    if (labels.ContainsKey(customLabel.Key))
                    {
                        _logger.LogWarning("Custom label '{CustomLabelName}' was already specified with value 'LabelValue' instead of 'CustomLabelValue'. Ignoring...", customLabel.Key, labels[customLabel.Key], customLabel.Value);
                        continue;
                    }

                    labels.Add(customLabel.Key, customLabel.Value);
                }
            }

            return (labels.Keys.ToArray(), labels.Values.ToArray());
        }
    }
}
