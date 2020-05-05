using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Core;
using Promitor.Integrations.AzureMonitor.Logging;
using Serilog;
using Serilog.Events;

namespace Promitor.Agents.Scraper
{
    public class Program : AgentProgram
    {
        public void Main(string[] args)
        {
            Welcome();

            // Let's hook in a logger for start-up purposes.
            ConfigureStartupLogging();

            BuildWebHost(args)
                .Build()
                .Run();
        }

        private void Welcome()
        {
            Console.WriteLine(Constants.Texts.Welcome);
        }

        public IHostBuilder BuildWebHost(string[] args)
        {
            var configuration = BuildConfiguration();
            var httpPort =  DetermineHttpPort(configuration);
            var endpointUrl = $"http://+:{httpPort}";

            return Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseKestrel(kestrelServerOptions =>
                        {
                            kestrelServerOptions.AddServerHeader = false;
                        })
                        .UseConfiguration(configuration)
                        .UseUrls(endpointUrl)
                        .UseSerilog((hostingContext, loggerConfiguration) => ConfigureSerilog(configuration, loggerConfiguration))
                        .UseStartup<Startup>();
                });
        }

        private int DetermineHttpPort(IConfiguration configuration)
        {
            var serverConfiguration = configuration.GetSection("server").Get<ServerConfiguration>();

            return base.DetermineHttpPort(serverConfiguration);
        }

        public LoggerConfiguration ConfigureSerilog(IConfiguration configuration, LoggerConfiguration loggerConfiguration)
        {
            var telemetryConfiguration = configuration.Get<ScraperRuntimeConfiguration>()?.Telemetry;
            if (telemetryConfiguration == null)
            {
                throw new Exception("Unable to get telemetry configuration");
            }

            var azureMonitorConfiguration = configuration.Get<ScraperRuntimeConfiguration>()?.AzureMonitor?.Logging;
            if (azureMonitorConfiguration == null)
            {
                throw new Exception("Unable to get logging configuration for Azure Monitor");
            }

            var defaultLogLevel = DetermineSinkLogLevel(telemetryConfiguration.DefaultVerbosity);
            loggerConfiguration.MinimumLevel.Is(defaultLogLevel)
                               .Enrich.FromLogContext();

            var appInsightsConfig = telemetryConfiguration.ApplicationInsights;
            if (appInsightsConfig?.IsEnabled == true)
            {
                var logLevel = DetermineSinkLogLevel(appInsightsConfig.Verbosity);
                loggerConfiguration.WriteTo.ApplicationInsights(appInsightsConfig.InstrumentationKey, TelemetryConverter.Traces, restrictedToMinimumLevel: logLevel)
                    .Filter.With(new AzureMonitorLoggingFilter(azureMonitorConfiguration));
            }

            var consoleLogConfig = telemetryConfiguration.ContainerLogs;
            if (consoleLogConfig?.IsEnabled == true)
            {
                var logLevel = DetermineSinkLogLevel(consoleLogConfig.Verbosity);

                loggerConfiguration.WriteTo.Console(restrictedToMinimumLevel: logLevel)
                                   .Filter.With(new AzureMonitorLoggingFilter(azureMonitorConfiguration));
            }

            return loggerConfiguration;
        }

        private LogEventLevel DetermineSinkLogLevel(LogLevel? logLevel)
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