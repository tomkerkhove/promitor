using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using CronScheduler.Extensions.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Promitor.Tests.Unit.Agents.Core
{
    public class SchedulerStartupTests
    {
        [Fact]
        public async Task StartAsync_WithScheduledJob_ExecutesJob()
        {
            var schedulerCronosReference = typeof(IScheduledJob).Assembly.GetReferencedAssemblies()
                .Single(assembly => assembly.Name == "Cronos");
            Assert.Equal(schedulerCronosReference.GetPublicKeyToken(), typeof(CronExpression).Assembly.GetName().GetPublicKeyToken());

            var executed = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var job = new Mock<IScheduledJob>();
            job.SetupGet(scheduledJob => scheduledJob.Name).Returns("startup-test");
            job.Setup(scheduledJob => scheduledJob.ExecuteAsync(It.IsAny<CancellationToken>()))
                .Returns(() =>
                {
                    executed.TrySetResult(true);
                    return Task.CompletedTask;
                });

            using var host = new HostBuilder()
                .ConfigureServices(services => services.AddScheduler(builder =>
                    builder.AddJob(_ => job.Object, options =>
                    {
                        options.CronSchedule = "0 0 * * *";
                        options.RunImmediately = true;
                    }, jobName: "startup-test")))
                .Build();

            await host.StartAsync();
            try
            {
                Assert.True(await executed.Task.WaitAsync(TimeSpan.FromSeconds(10)));
            }
            finally
            {
                await host.StopAsync();
            }
        }
    }
}
