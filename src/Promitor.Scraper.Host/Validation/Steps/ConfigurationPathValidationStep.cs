using System;
using System.IO;
using Promitor.Core;
using Promitor.Scraper.Host.Validation.Interfaces;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class ConfigurationPathValidationStep : ValidationStep, IValidationStep
    {
        public string ComponentName { get; } = "Metrics Declaration Path";

        public ValidationResult Run()
        {
            var configurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.Configuration.Path);
            if (string.IsNullOrWhiteSpace(configurationPath))
            {
                LogMessage("No scrape configuration configured, falling back to default one...");
                configurationPath = Promitor.Core.Scraping.Constants.Defaults.MetricsDeclarationPath;
                Environment.SetEnvironmentVariable(EnvironmentVariables.Configuration.Path, configurationPath);
            }

            if (File.Exists(configurationPath) == false)
            {
                var errorMessage = $"Scrape configuration at '{configurationPath}' does not exist";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            LogMessage($"Scrape configuration found at '{configurationPath}'");
            return ValidationResult.Successful(ComponentName);
        }
    }
}