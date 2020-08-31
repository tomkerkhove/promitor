using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Core.Scraping.Configuration.Runtime;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Misc
{
    [Category("Unit")]
    public class ConfigurationPathValidationStepTests
    {
        [Fact]
        public void ConfigurationPath_FileDoesNotExist_Fails()
        {
            // Arrange
            const string validConfigurationPath = "Invalid";
            var configOptions = Options.Create(new MetricsConfiguration {AbsolutePath = validConfigurationPath});

            // Act
            var scrapingScheduleValidationStep = new ConfigurationPathValidationStep(configOptions, NullLogger<ConfigurationPathValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ConfigurationPath_FileExists_Succeeds()
        {
            // Arrange
            const string invalidConfigurationPath = "Files/valid-sample.yaml";
            var configOptions = Options.Create(new MetricsConfiguration {AbsolutePath = invalidConfigurationPath});

            // Act
            var scrapingScheduleValidationStep = new ConfigurationPathValidationStep(configOptions, NullLogger<ConfigurationPathValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }
    }
}