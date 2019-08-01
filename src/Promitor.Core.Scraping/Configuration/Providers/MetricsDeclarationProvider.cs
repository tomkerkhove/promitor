using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Configuration.Serialization;

namespace Promitor.Core.Scraping.Configuration.Providers
{
    public class MetricsDeclarationProvider : IMetricsDeclarationProvider
    {
        private readonly ConfigurationSerializer _configurationSerializer;
        private readonly IConfiguration _configuration;

        public MetricsDeclarationProvider(IConfiguration configuration, ILogger logger)
        {
            _configurationSerializer = new ConfigurationSerializer(logger);
            _configuration = configuration;
        }

        public virtual MetricsDeclaration Get(bool applyDefaults = false)
        {
            var rawMetricsDeclaration = ReadRawDeclaration();

            var config = _configurationSerializer.Deserialize(rawMetricsDeclaration);

            if (applyDefaults)
            {
                foreach (var metric in config.Metrics)
                {
                    // Apply AzureMetadata.ResourceGroupName to metrics with no other RG specified
                    if (string.IsNullOrWhiteSpace(metric.ResourceGroupName))
                    {
                        metric.ResourceGroupName = config.AzureMetadata.ResourceGroupName;
                    }

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
                        metric.AzureMetricConfiguration.Aggregation.Interval = config.MetricDefaults.Aggregation.Interval;
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