using System;
using System.IO;
using Promitor.Scraper.Configuration;
using Promitor.Scraper.Validation.Interfaces;

namespace Promitor.Scraper.Validation.Steps
{
    public class ConfigurationPathValidationStep : IValidationStep
    {
        public ValidationResult Validate()
        {
            var configurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.ConfigurationPath);
            if (string.IsNullOrWhiteSpace(configurationPath))
            {
                Console.WriteLine("No scrape configuration configured, falling back to default one...");
                configurationPath = "default-scrape-configuration.yaml";
                Environment.SetEnvironmentVariable(EnvironmentVariables.ConfigurationPath, configurationPath);
            }

            if (File.Exists(configurationPath) == false)
            {
                var errorMessage = $"Scrape configuration at '{configurationPath}' does not exist";
                Console.WriteLine(errorMessage);

                return ValidationResult.Fail(errorMessage);
            }

            Console.WriteLine($"Scrape configuration found at '{configurationPath}'");
            return ValidationResult.Successful;
        }
    }
}