using System;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace Promitor.Agents.Core.Observability
{
    public class SerilogFactory
    {
        /// <summary>
        ///     Transforms log level to Serilog format
        /// </summary>
        /// <param name="logLevel">Specified log level</param>
        public static LogEventLevel DetermineSinkLogLevel(LogLevel? logLevel)
        {
            if (logLevel == null)
            {
                return LogEventLevel.Verbose;
            }

            switch (logLevel)
            {
                case LogLevel.Critical:
                    return LogEventLevel.Fatal;
                case LogLevel.Trace:
                    return LogEventLevel.Verbose;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.None:
                    return LogEventLevel.Fatal;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, "Unable to determine correct log event level.");
            }
        }
    }
}