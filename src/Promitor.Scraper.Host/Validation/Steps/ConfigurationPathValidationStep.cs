using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core;
using Promitor.Core.Configuration.Metrics;
using Promitor.Scraper.Host.Validation.Interfaces;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class ConfigurationPathValidationStep : ValidationStep, IValidationStep
    {
        public string ComponentName { get; } = "Metrics Declaration Path";

        public ConfigurationPathValidationStep(IConfiguration configuration) : base(configuration, NullLogger.Instance)
        {
        }

        public ConfigurationPathValidationStep(IConfiguration configuration, ILogger logger) : base(configuration, logger)
        {
        }

        public ValidationResult Run()
        {
            var metricsConfiguration = Configuration.GetSection("metricsConfiguration").Get<MetricsConfiguration>();
            if (string.IsNullOrWhiteSpace(metricsConfiguration?.AbsolutePath))
            {
                var errorMessage = "No scrape configuration path configured";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            if (File.Exists(metricsConfiguration?.AbsolutePath) == false)
            {
                var errorMessage = $"Scrape configuration at '{metricsConfiguration}' does not exist";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            Logger.LogInformation("Scrape configuration found at '{configurationPath}'", metricsConfiguration?.AbsolutePath);
            return ValidationResult.Successful(ComponentName);
        }
    }
}