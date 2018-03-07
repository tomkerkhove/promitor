using System;
using Shuttle.Core.Cron;

namespace Promitor.Scraper.Scheduling.Cron
{
    public static class CronSchedule
    {
        /// <summary>
        ///     Parses the Cron schedule into a Cron expression
        /// </summary>
        /// <param name="rawCronSchedule">Raw Cron schedule to use</param>
        public static CronExpression Parse(string rawCronSchedule)
        {
            if (string.IsNullOrWhiteSpace(rawCronSchedule))
            {
                throw new Exception("No Cron schedule was specified");
            }

            return new CronExpression(rawCronSchedule);
        }
    }
}