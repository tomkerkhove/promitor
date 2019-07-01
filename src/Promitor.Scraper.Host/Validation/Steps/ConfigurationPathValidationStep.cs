using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core;
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
            var configurationPath = Configuration.GetValue<string>(EnvironmentVariables.Configuration.Path);
            if (string.IsNullOrWhiteSpace(configurationPath))
            {
                Logger.LogWarning("No scrape configuration path configured, falling back to default one on '{configurationPath}'.", Core.Scraping.Constants.Defaults.MetricsDeclarationPath);
                configurationPath = Core.Scraping.Constants.Defaults.MetricsDeclarationPath;
                Configuration[EnvironmentVariables.Configuration.Path] = configurationPath;
            }

            if (File.Exists(configurationPath) == false)
            {
                var errorMessage = $"Scrape configuration at '{configurationPath}' does not exist";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            Logger.LogInformation("Scrape configuration found at '{configurationPath}'", Core.Scraping.Constants.Defaults.MetricsDeclarationPath);
            return ValidationResult.Successful(ComponentName);
        }
    }
}