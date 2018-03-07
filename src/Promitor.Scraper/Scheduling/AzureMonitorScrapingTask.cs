using System;
using System.Threading;
using System.Threading.Tasks;
using Promitor.Scraper.Configuration;
using Promitor.Scraper.Scheduling.Interfaces;

namespace Promitor.Scraper.Scheduling
{
    public class AzureMonitorScrapingTask : IScheduledTask
    {
        public string Schedule => Environment.GetEnvironmentVariable(EnvironmentVariables.ScrapeCronSchedule);

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Scraping Azure Monitor - {DateTimeOffset.Now}");

            return Task.CompletedTask;
        }
    }
}