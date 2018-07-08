using System;
using System.ComponentModel;
using Promitor.Scraper.Host;
using Promitor.Scraper.Host.Validation.Steps;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation
{
    [Category("Unit")]
    public class ConfigurationPathValidationStepTests
    {
        [Fact]
        public void ConfigurationPath_FileDoesNotExist_Succeeds()
        {
            // Arrange
            const string validConfigurationPath = "Invalid";
            Environment.SetEnvironmentVariable(EnvironmentVariables.Configuration.Path, validConfigurationPath);

            // Act
            var scrapingScheduleValidationStep = new ConfigurationPathValidationStep();
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ConfigurationPath_FileExists_Succeeds()
        {
            // Arrange
            const string invalidConfigurationPath = "Files/valid-sample.yaml";
            Environment.SetEnvironmentVariable(EnvironmentVariables.Configuration.Path, invalidConfigurationPath);

            // Act
            var scrapingScheduleValidationStep = new ConfigurationPathValidationStep();
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }
    }
}