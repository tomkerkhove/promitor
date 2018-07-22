using System;
using Promitor.Scraper.Scheduling.Interfaces;
using Shuttle.Core.Cron;

/*
 * Based on example by Maarten Balliauw - https://blog.maartenballiauw.be/post/2017/08/01/building-a-scheduled-cache-updater-in-aspnet-core-2.html
 * Thank you!
 */
namespace Promitor.Scraper.Host.Scheduling.Infrastructure
{
    internal class SchedulerTaskWrapper
    {
        public DateTime LastRunTime { get; set; }
        public DateTime NextRunTime { get; set; }
        public CronExpression Schedule { get; set; }
        public IScheduledTask Task { get; set; }

        public void Increment()
        {
            LastRunTime = NextRunTime;
            NextRunTime = Schedule.NextOccurrence(NextRunTime);
        }

        public bool ShouldRun(DateTime currentTime)
        {
            return NextRunTime < currentTime && LastRunTime != NextRunTime;
        }
    }
}