using System;
using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core;
using Promitor.Scraper.Host.Validation.Interfaces;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class ScrapingScheduleValidationStep : ValidationStep, IValidationStep
    {
        private const string DefaultCronSchedule = "*/5 * * * *";

        public ScrapingScheduleValidationStep(IConfiguration configuration) : base(configuration, NullLogger.Instance)
        {
        }

        public ScrapingScheduleValidationStep(IConfiguration configuration, ILogger logger) : base(configuration, logger)
        {
        }

        public string ComponentName { get; } = "Cron Schedule";

        public ValidationResult Run()
        {
            var scrapingCronSchedule = Configuration.GetValue<string>(EnvironmentVariables.Scraping.CronSchedule);
            if (string.IsNullOrWhiteSpace(scrapingCronSchedule))
            {
                Logger.LogWarning("No scraping schedule was specified, falling back to default '{ScrapeSchedule}' cron schedule...", DefaultCronSchedule);
                scrapingCronSchedule = DefaultCronSchedule;
                Configuration[EnvironmentVariables.Authentication.ApplicationId] = scrapingCronSchedule;
            }

            try
            {
                CronExpression.Parse(scrapingCronSchedule);
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