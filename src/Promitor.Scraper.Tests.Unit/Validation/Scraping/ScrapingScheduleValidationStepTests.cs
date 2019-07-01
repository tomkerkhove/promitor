using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Promitor.Core;
using Promitor.Scraper.Host.Validation.Steps;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Scraping
{
    [Category("Unit")]
    public class ScrapingScheduleValidationStepTests
    {
        [Fact]
        public void ScrapingSchedule_InvalidCron_Fails()
        {
            // Arrange
            const string invalidCron = "Invalid * * * *";

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {EnvironmentVariables.Scraping.CronSchedule, invalidCron},
                })
                .Build();

            // Act
            var scrapingScheduleValidationStep = new ScrapingScheduleValidationStep(config);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ScrapingSchedule_ValidCron_Succeeds()
        {
            // Arrange
            const string validCron = "* * * * *";

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {EnvironmentVariables.Scraping.CronSchedule, validCron},
                })
                .Build();

            // Act
            var scrapingScheduleValidationStep = new ScrapingScheduleValidationStep(config);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }
    }
}