using System;
using Promitor.Scraper.Configuration;
using Promitor.Scraper.Scheduling.Cron;
using Promitor.Scraper.Validation.Interfaces;

namespace Promitor.Scraper.Validation.Steps
{
    public class ScrapingScheduleValidationStep : IValidationStep
    {
        private const string DefaultCronSchedule = "*/5 * * * *";

        public ValidationResult Validate()
        {
            var scrapingCronSchedule = Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);
            if (string.IsNullOrWhiteSpace(scrapingCronSchedule))
            {
                Console.WriteLine($"No scraping schedule was specified, falling back to default '{DefaultCronSchedule}' cron schedule...");
                Environment.SetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule, DefaultCronSchedule);
                scrapingCronSchedule = DefaultCronSchedule;
            }

            try
            {
                CronSchedule.Parse(scrapingCronSchedule);
                return ValidationResult.Successful;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"No valid scraping schedule was specified - '{scrapingCronSchedule}'. Details: {exception.Message}");
                return ValidationResult.Fail($"No valid scraping schedule was specified - '{scrapingCronSchedule}'.");
            }
        }
    }
}