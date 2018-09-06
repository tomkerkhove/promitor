using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Promitor.Scraper.Host.Scheduling.Cron;
using Promitor.Scraper.Host.Scheduling.Interfaces;

namespace Promitor.Scraper.Host.Scheduling.Infrastructure
{
    public class SchedulerHostedService : BackgroundService
    {
        private readonly List<SchedulerTaskWrapper> _scheduledTasks = new List<SchedulerTaskWrapper>();

        public SchedulerHostedService(IEnumerable<IScheduledTask> scheduledTasks)
        {
            var referenceTime = DateTime.UtcNow;

            foreach (var scheduledTask in scheduledTasks)
            {
                var scheduledWrapper = new SchedulerTaskWrapper
                {
                    Schedule = CronSchedule.Parse(scheduledTask.Schedule),
                    Task = scheduledTask,
                    NextRunTime = referenceTime
                };

                _scheduledTasks.Add(scheduledWrapper);
            }
        }

        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(cancellationToken);

                await Task.Delay(TimeSpan.FromMinutes(value: 1), cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            var referenceTime = DateTime.UtcNow;

            var tasksThatShouldRun = _scheduledTasks.Where(task => task.ShouldRun(referenceTime)).ToList();

            foreach (var taskThatShouldRun in tasksThatShouldRun)
            {
                taskThatShouldRun.Increment();

                await taskFactory.StartNew(
                    async () =>
                    {
                        try
                        {
                            await taskThatShouldRun.Task.ExecuteAsync(cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            UnobservedTaskException?.Invoke(this, args);

                            if (!args.Observed)
                            {
                                throw;
                            }
                        }
                    },
                    cancellationToken);
            }
        }
    }
}