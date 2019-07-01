using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Promitor.Core;
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
            Environment.SetEnvironmentVariable(EnvironmentVariables.Configuration.Path, validConfigurationPath);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {EnvironmentVariables.Configuration.Path, validConfigurationPath}
                })
                .Build();

            // Act
            var scrapingScheduleValidationStep = new ConfigurationPathValidationStep(config);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ConfigurationPath_FileExists_Succeeds()
        {
            // Arrange
            const string invalidConfigurationPath = "Files/valid-sample.yaml";

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {EnvironmentVariables.Configuration.Path, invalidConfigurationPath}
                })
                .Build();

            // Act
            var scrapingScheduleValidationStep = new ConfigurationPathValidationStep(config);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }
    }
}