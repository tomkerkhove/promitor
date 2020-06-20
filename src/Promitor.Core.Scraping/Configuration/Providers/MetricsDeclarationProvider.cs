using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Core.Scraping.Configuration.Serialization;

namespace Promitor.Core.Scraping.Configuration.Providers
{
    public class MetricsDeclarationProvider : IMetricsDeclarationProvider
    {
        private readonly ConfigurationSerializer _configurationSerializer;
        private readonly IConfiguration _configuration;

        public MetricsDeclarationProvider(IConfiguration configuration, ConfigurationSerializer configurationSerializer)
        {
            _configurationSerializer = configurationSerializer;
            _configuration = configuration;
        }

        public virtual PrometheusMetricDefinition GetPrometheusDefinition(string metricName, bool applyDefaults = false, IErrorReporter errorReporter = null)
        {
            var foundMetric = GetMetricDefinition(metricName,applyDefaults,errorReporter);
            return foundMetric?.PrometheusMetricDefinition;
        }

        public virtual MetricDefinition GetMetricDefinition(string metricName, bool applyDefaults = false, IErrorReporter errorReporter = null)
        {
            var metricsDeclaration = Get(applyDefaults, errorReporter);
            return metricsDeclaration.Metrics.FirstOrDefault(metric => metric.PrometheusMetricDefinition.Name.Equals(metricName, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual MetricsDeclaration Get(bool applyDefaults = false, IErrorReporter errorReporter = null)
        {
            var rawMetricsDeclaration = ReadRawDeclaration();
            errorReporter ??= new ErrorReporter();

            var config = _configurationSerializer.Deserialize(rawMetricsDeclaration, errorReporter);

            if (!errorReporter.HasErrors && applyDefaults)
            {
                foreach (var metric in config.Metrics)
                {
                    // Apply the default aggregation interval if none is specified
                    if (metric.AzureMetricConfiguration == null)
                    {
                        metric.AzureMetricConfiguration = new AzureMetricConfiguration();
                    }
                    if (metric.AzureMetricConfiguration?.Aggregation == null)
                    {
                        metric.AzureMetricConfiguration.Aggregation = new MetricAggregation();
                    }
                    if (metric.AzureMetricConfiguration?.Aggregation.Interval == null)
                    {
                        metric.AzureMetricConfiguration.Aggregation.Interval = config.MetricDefaults.Aggregation?.Interval;
                    }

                    // Apply the default scraping interval if none is specified
                    if (metric.Scraping == null)
                    {
                        metric.Scraping = config.MetricDefaults.Scraping;
                    }

                    // Apply the default scraping interval if none is specified
                    if (metric.Scraping.Schedule == null)
                    {
                        metric.Scraping.Schedule = config.MetricDefaults.Scraping.Schedule;
                    }
                }
            }
            return config;
        }

        public virtual string ReadRawDeclaration()
        {
            var metricConfiguration = _configuration.GetSection("metricsConfiguration").Get<MetricsConfiguration>();
            var scrapingConfigurationPath = metricConfiguration?.AbsolutePath;

            if (string.IsNullOrWhiteSpace(scrapingConfigurationPath))
            {
                throw new Exception("No scraping configuration path was specified.");
            }

            var rawMetricsDeclaration = File.ReadAllText(scrapingConfigurationPath);
            return rawMetricsDeclaration;
        }
    }
}