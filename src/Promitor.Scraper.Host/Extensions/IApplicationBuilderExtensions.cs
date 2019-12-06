using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.AspNetCore;
using Promitor.Core.Configuration.Model;
using Promitor.Core.Configuration.Model.Telemetry;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable once CheckNamespace
namespace Promitor.Scraper.Host.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Use Serilog to emit logs to multiple sinks
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="configuration">Configuration of the application</param>
        public static IApplicationBuilder UseSerilog(this IApplicationBuilder app, IConfiguration configuration)
        {
            var telemetryConfiguration = configuration.Get<RuntimeConfiguration>()?.Telemetry;
            if (telemetryConfiguration == null)
            {
                throw new Exception("Unable to get telemetry configuration");
            }

            var defaultLogLevel = DetermineSinkLogLevel(telemetryConfiguration.DefaultVerbosity);
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(defaultLogLevel)
                .Enrich.FromLogContext();

            var appInsightsConfig = telemetryConfiguration.ApplicationInsights;
            if (appInsightsConfig?.IsEnabled == true)
            {
                var logLevel = DetermineSinkLogLevel(appInsightsConfig.Verbosity);
                loggerConfiguration.WriteTo.ApplicationInsights(appInsightsConfig.InstrumentationKey, TelemetryConverter.Traces, restrictedToMinimumLevel: logLevel);
            }

            var consoleLogConfig = telemetryConfiguration.ContainerLogs;
            if (consoleLogConfig?.IsEnabled == true)
            {
                var logLevel = DetermineSinkLogLevel(consoleLogConfig.Verbosity);

                loggerConfiguration.WriteTo.Console(restrictedToMinimumLevel: logLevel);
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            return app;
        }

        private static LogEventLevel DetermineSinkLogLevel(LogEventLevel? logLevel)
        {
            return logLevel ?? LogEventLevel.Verbose;
        }

        /// <summary>
        ///     Add support for Open API with API explorer
        /// </summary>
        /// <param name="app">Application Builder</param>
        public static void UseOpenApiUi(this IApplicationBuilder app)
        {
            // New Swagger UI
            app.UseSwagger(setupAction => setupAction.RouteTemplate = "api/{documentName}/docs.json");
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.ConfigureDefaultOptions();
                swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", "Promitor API");
                swaggerUiOptions.RoutePrefix = "api/docs";
            });

            // Deprecated Swagger UI
            app.UseSwagger(setupAction => setupAction.SerializeAsV2 = true);
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Promitor API");
                swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", "Promitor API (OpenAPI 3.0)");
                swaggerUiOptions.ConfigureDefaultOptions();
            });
        }

        /// <summary>
        ///     Add support for exposing a prometheus scraping endpoint
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="scrapeEndpointPath">Path where the scrape endpoint will be exposed</param>
        public static IApplicationBuilder UsePrometheusScraper(this IApplicationBuilder app, string scrapeEndpointPath)
        {
            app.UsePrometheusServer(prometheusOptions =>
            {
                prometheusOptions.MapPath = scrapeEndpointPath;
                prometheusOptions.UseDefaultCollectors = false;
            });

            return app;
        }
    }
}
