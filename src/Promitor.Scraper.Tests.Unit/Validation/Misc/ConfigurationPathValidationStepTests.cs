using System.ComponentModel;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Metrics;
using Promitor.Scraper.Host.Validation.Steps;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Misc
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
            var scrapingScheduleValidationStep = new ConfigurationPathValidationStep(configOptions);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ConfigurationPath_FileExists_Succeeds()
        {
            // Arrange
            const string invalidConfigurationPath = "Files/valid-sample.yaml";
            var configOptions = Options.Create(new MetricsConfiguration {AbsolutePath = invalidConfigurationPath});

            // Act
            var scrapingScheduleValidationStep = new ConfigurationPathValidationStep(configOptions);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }
    }
}