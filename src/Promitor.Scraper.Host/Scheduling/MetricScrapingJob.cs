using System;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.AspNetCore;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Promitor.Core;

namespace Promitor.Scraper.Host.Scheduling
{
    public class MetricScrapingJob : IScheduledJob
    {
        public MetricScrapingJob()
        {
            CronSchedule = Environment.GetEnvironmentVariable(EnvironmentVariables.Scraping.CronSchedule);
            RunImmediately = false;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
        }

        public string CronSchedule { get; }
        public string CronTimeZone { get; }
        public bool RunImmediately { get; }
    }
}
