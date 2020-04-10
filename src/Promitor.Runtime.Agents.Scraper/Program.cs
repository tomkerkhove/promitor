using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Configuration.Model;
using Promitor.Core.Configuration.Model.Server;
using Promitor.Integrations.AzureMonitor.Logging;
using Serilog;
using Serilog.Events;

namespace Promitor.Runtime.Agents.Scraper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Welcome();

            // Let's hook in a logger for start-up purposes.
            ConfigureStartupLogging();

            BuildWebHost(args)
                .Build()
                .Run();
        }

        private static void Welcome()
        {
            Console.WriteLine(Constants.Texts.Welcome);
        }

        public static IHostBuilder BuildWebHost(string[] args)
        {
            var configuration = CreateConfiguration();
            var httpPort = DetermineHttpPort(configuration);
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
                        .UseStartup<Startup>()
                        .UseSerilog((hostingContext, loggerConfiguration) => ConfigureSerilog(configuration, loggerConfiguration));
                });
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            var configurationFolder = Environment.GetEnvironmentVariable(EnvironmentVariables.Configuration.Folder);
            if (string.IsNullOrWhiteSpace(configurationFolder))
            {
                throw new Exception("Unable to determine the configuration folder");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile($"{configurationFolder}/runtime.yaml", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariables(prefix: "PROMITOR_") // Used for all environment variables for Promitor
                .AddEnvironmentVariables(prefix: "PROMITOR_YAML_OVERRIDE_") // Used to overwrite runtime YAML
                .Build();

            return configuration;
        }

        private static int DetermineHttpPort(IConfiguration configuration)
        {
            var serverConfiguration = configuration.GetSection("server").Get<ServerConfiguration>();

            return serverConfiguration?.HttpPort ?? 80;
        }

        private static void ConfigureStartupLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static LoggerConfiguration ConfigureSerilog(IConfiguration configuration, LoggerConfiguration loggerConfiguration)
        {
            var telemetryConfiguration = configuration.Get<RuntimeConfiguration>()?.Telemetry;
            if (telemetryConfiguration == null)
            {
                throw new Exception("Unable to get telemetry configuration");
            }

            var azureMonitorConfiguration = configuration.Get<RuntimeConfiguration>()?.AzureMonitor?.Logging;
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

        private static LogEventLevel DetermineSinkLogLevel(LogLevel? logLevel)
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