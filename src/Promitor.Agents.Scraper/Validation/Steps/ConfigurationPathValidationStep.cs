using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Core.Scraping.Configuration.Runtime;

namespace Promitor.Agents.Scraper.Validation.Steps
{
    public class ConfigurationPathValidationStep : ValidationStep, IValidationStep
    {
        private readonly IOptions<MetricsConfiguration> _metricsConfiguration;
        public string ComponentName => "Metrics Declaration Path";

        public ConfigurationPathValidationStep(IOptions<MetricsConfiguration> metricsConfiguration, ILogger<ConfigurationPathValidationStep> logger) : base(logger)
        {
            _metricsConfiguration = metricsConfiguration;
        }

        public ValidationResult Run()
        {
            var absolutePath = _metricsConfiguration.Value.AbsolutePath;
            if (string.IsNullOrWhiteSpace(absolutePath))
            {
                var errorMessage = "No scrape configuration path configured";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            if (File.Exists(absolutePath) == false)
            {
                var errorMessage = $"Scrape configuration at '{absolutePath}' does not exist";
                return ValidationResult.Failure(ComponentName, errorMessage);
            }

            Logger.LogInformation("Scrape configuration found at '{configurationPath}'", absolutePath);
            return ValidationResult.Successful(ComponentName);
        }
    }
}