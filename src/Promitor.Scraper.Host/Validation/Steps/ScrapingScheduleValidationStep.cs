using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core;
using Promitor.Scraper.Host.Scheduling.Cron;
using Promitor.Scraper.Host.Validation.Interfaces;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class ScrapingScheduleValidationStep : ValidationStep, IValidationStep
    {
        private const string DefaultCronSchedule = "*/5 * * * *";

        public ScrapingScheduleValidationStep() : base(NullLogger.Instance)
        {
        }

        public ScrapingScheduleValidationStep(ILogger logger) : base(logger)
        {
        }

        public string ComponentName { get; } = "Cron Schedule";

        public ValidationResult Run()
        {
            var scrapingCronSchedule = Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);
            if (string.IsNullOrWhiteSpace(scrapingCronSchedule))
            {
                Logger.LogWarning("No scraping schedule was specified, falling back to default '{ScrapeSchedule}' cron schedule...", DefaultCronSchedule);
                scrapingCronSchedule = DefaultCronSchedule;
                Environment.SetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule, scrapingCronSchedule);
            }

            try
            {
                CronSchedule.Parse(scrapingCronSchedule);
                return ValidationResult.Successful(ComponentName);
            }
            catch (Exception exception)
            {
                Logger.LogError("No valid scraping schedule was specified ('{ScrapeSchedule}' - Exception: {Exception})", DefaultCronSchedule, exception);
                return ValidationResult.Failure(ComponentName, $"No valid scraping schedule was specified - '{scrapingCronSchedule}'.");
            }
        }
    }
}