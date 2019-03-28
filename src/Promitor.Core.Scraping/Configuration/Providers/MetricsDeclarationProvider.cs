using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Configuration.Serialization.Core;

namespace Promitor.Core.Scraping.Configuration.Providers
{
    public class MetricsDeclarationProvider : IMetricsDeclarationProvider
    {
        private readonly ConfigurationSerializer _configurationSerializer;

        public MetricsDeclarationProvider(ILogger logger)
        {
            _configurationSerializer = new ConfigurationSerializer(logger);
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
            var scrapingConfigurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.Configuration.Path);
            if (string.IsNullOrWhiteSpace(scrapingConfigurationPath))
            {
                Console.WriteLine($"No scraping configuration path was specified, falling back to default '{Constants.Defaults.MetricsDeclarationPath}'...");
                scrapingConfigurationPath = Constants.Defaults.MetricsDeclarationPath;
            }

            var rawMetricsDeclaration = File.ReadAllText(scrapingConfigurationPath);
            return rawMetricsDeclaration;
        }
    }
}