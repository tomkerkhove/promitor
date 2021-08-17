﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Promitor.Agents.Core.Observability
{
    /// <summary>
    /// Provides observability functionality for background jobs
    /// </summary>
    public class BackgroundJobMonitor
    {
        /// <summary>
        /// Provide capability to handle and log exceptions
        /// </summary>
        public static void HandleException(object jobName, UnobservedTaskExceptionEventArgs exceptionEventArgs, IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetService<ILogger<BackgroundJobMonitor>>();
            if (logger == null)
            {
                return;
            }

            logger.LogCritical(exceptionEventArgs.Exception, "Unhandled exception in job {JobName}", jobName);

            exceptionEventArgs.SetObserved();
        }
    }
}
