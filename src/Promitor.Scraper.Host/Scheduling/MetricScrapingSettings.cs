using CronScheduler.AspNetCore;
using GuardNet;

namespace Promitor.Scraper.Host.Scheduling
{
    public class MetricScrapingSettings : SchedulerOptions
    {
        public MetricScrapingSettings(string scrapingCronSchedule)
        {
            Guard.NotNullOrWhitespace(scrapingCronSchedule, nameof(scrapingCronSchedule));

            CronSchedule = scrapingCronSchedule;
        }
    }
}