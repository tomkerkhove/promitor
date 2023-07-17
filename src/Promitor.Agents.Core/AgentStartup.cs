using System;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Promitor.Agents.Core.Configuration;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Agents.Core.Observability;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Promitor.Agents.Core
{
    public class AgentStartup
    {
        /// <summary>
        ///     Configuration of the application
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="configuration">Configuration of the application</param>
        protected AgentStartup(IConfiguration configuration)
        {
            Guard.NotNull(configuration, nameof(configuration));

            Configuration = configuration;
        }

        /// <summary>
        /// Use & wire up Serilog with a configuration
        /// </summary>
        /// <param name="componentName">Name of the component which is starting up</param>
        /// <param name="serviceProvider">Registered services in the application</param>
        protected void UseSerilog(string componentName, IServiceProvider serviceProvider)
        {
            var telemetryConfiguration = Configuration.Get<RuntimeConfiguration>()?.Telemetry;
            if (telemetryConfiguration == null)
            {
                throw new Exception("Unable to get telemetry configuration");
            }

            Log.Logger = CreateSerilogConfiguration(componentName, telemetryConfiguration, serviceProvider).CreateLogger();
        }

        /// <summary>
        /// Creates a configuration for Serilog
        /// </summary>
        /// <param name="componentName">Name of the component which is starting up</param>
        /// <param name="telemetryConfiguration">Configuration around telemetry for agent</param>
        /// <param name="serviceProvider">Registered services in the application</param>
        protected virtual LoggerConfiguration CreateSerilogConfiguration(string componentName, TelemetryConfiguration telemetryConfiguration, IServiceProvider serviceProvider)
        {
            var defaultLogLevel = SerilogFactory.DetermineSinkLogLevel(telemetryConfiguration.DefaultVerbosity);
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(defaultLogLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);

            loggerConfiguration = EnrichTelemetry(componentName, serviceProvider, loggerConfiguration);
            loggerConfiguration = FilterTelemetry(loggerConfiguration);
            loggerConfiguration = WriteTelemetryToSinks(telemetryConfiguration,loggerConfiguration);

            return loggerConfiguration;
        }

        /// <summary>
        /// Configures how to enrich telemetry
        /// </summary>
        /// <param name="componentName">Name of the component which is starting up</param>
        /// <param name="serviceProvider">Registered services in the application</param>
        /// <param name="loggerConfiguration">Current Serilog configuration</param>
        /// <returns>Updated Serilog configuration with enrichment</returns>
        protected virtual LoggerConfiguration EnrichTelemetry(string componentName, IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration.Enrich.FromLogContext()
                                      .Enrich.WithComponentName(componentName)
                                      .Enrich.WithVersion()
                                      .Enrich.WithHttpCorrelationInfo(serviceProvider);
        }

        /// <summary>
        /// Configures what telemetry to filter
        /// </summary>
        /// <param name="loggerConfiguration">Current Serilog configuration</param>
        /// <returns>Updated Serilog configuration with filtering</returns>
        protected virtual LoggerConfiguration FilterTelemetry(LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration;
        }

        /// <summary>
        /// Configures what sinks to write telemetry to
        /// </summary>
        /// <param name="telemetryConfiguration">Agent configuration with regards to telemetry</param>
        /// <param name="loggerConfiguration">Current Serilog configuration</param>
        /// <returns>Updated Serilog configuration with sinks</returns>
        protected virtual LoggerConfiguration WriteTelemetryToSinks(TelemetryConfiguration telemetryConfiguration, LoggerConfiguration loggerConfiguration)
        {
            var appInsightsConfig = telemetryConfiguration.ApplicationInsights;
            if (appInsightsConfig?.IsEnabled == true)
            {
                var logLevel = SerilogFactory.DetermineSinkLogLevel(appInsightsConfig.Verbosity);
                loggerConfiguration.WriteTo.AzureApplicationInsightsWithInstrumentationKey(appInsightsConfig.InstrumentationKey, restrictedToMinimumLevel: logLevel);
            }

            var consoleLogConfig = telemetryConfiguration.ContainerLogs;
            if (consoleLogConfig?.IsEnabled == true)
            {
                var logLevel = SerilogFactory.DetermineSinkLogLevel(consoleLogConfig.Verbosity);
                loggerConfiguration.WriteTo.Console(restrictedToMinimumLevel: logLevel);
            }

            return loggerConfiguration;
        }
    }
}