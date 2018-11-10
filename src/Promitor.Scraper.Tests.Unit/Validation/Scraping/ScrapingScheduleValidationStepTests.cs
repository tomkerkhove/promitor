using System;
using System.ComponentModel;
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
            Environment.SetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule, invalidCron);

            // Act
            var scrapingScheduleValidationStep = new ScrapingScheduleValidationStep();
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ScrapingSchedule_ValidCron_Succeeds()
        {
            // Arrange
            const string validCron = "* * * * *";
            Environment.SetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule, validCron);

            // Act
            var scrapingScheduleValidationStep = new ScrapingScheduleValidationStep();
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }
    }
}