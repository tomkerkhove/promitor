using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Observability;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Core;
using Promitor.Integrations.AzureMonitor.Logging;
using Serilog;
using Serilog.Events;

namespace Promitor.Agents.Scraper
{
    public class Program : AgentProgram
    {
        public static int Main(string[] args)
        {
            try
            {
                Welcome();

                // Let's hook in a logger for start-up purposes.
                ConfigureStartupLogging();

                CreateHostBuilder(args)
                    .Build()
                    .Run();

                return 0;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void Welcome()
        {
            Console.WriteLine(Constants.Texts.Welcome);
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration configuration = BuildConfiguration();
            ServerConfiguration serverConfiguration = GetServerConfiguration(configuration);
            IHostBuilder webHostBuilder = CreatePromitorWebHost<Startup>(args, configuration, serverConfiguration);

            return webHostBuilder;
        }

        private static ServerConfiguration GetServerConfiguration(IConfiguration configuration)
        {
            var serverConfiguration = configuration.GetSection("server").Get<ServerConfiguration>();
            return serverConfiguration;
        }

        private static IConfigurationRoot BuildConfiguration()
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

        public LoggerConfiguration DefineSerilogConfiguration(IConfiguration configuration, LoggerConfiguration loggerConfiguration)
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

            var defaultLogLevel = SerilogFactory.DetermineSinkLogLevel(telemetryConfiguration.DefaultVerbosity);
            loggerConfiguration.MinimumLevel.Is(defaultLogLevel)
                               .Enrich.FromLogContext();

            var appInsightsConfig = telemetryConfiguration.ApplicationInsights;
            if (appInsightsConfig?.IsEnabled == true)
            {
                var logLevel = SerilogFactory.DetermineSinkLogLevel(appInsightsConfig.Verbosity);
                loggerConfiguration.WriteTo.ApplicationInsights(appInsightsConfig.InstrumentationKey, TelemetryConverter.Traces, restrictedToMinimumLevel: logLevel)
                    .Filter.With(new AzureMonitorLoggingFilter(azureMonitorConfiguration));
            }

            var consoleLogConfig = telemetryConfiguration.ContainerLogs;
            if (consoleLogConfig?.IsEnabled == true)
            {
                var logLevel = SerilogFactory.DetermineSinkLogLevel(consoleLogConfig.Verbosity);
                loggerConfiguration.WriteTo.Console(restrictedToMinimumLevel: logLevel)
                                   .Filter.With(new AzureMonitorLoggingFilter(azureMonitorConfiguration));
            }

            return loggerConfiguration;
        }
    }
}