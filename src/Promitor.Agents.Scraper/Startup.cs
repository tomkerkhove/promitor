﻿using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Agents.Scraper.Extensions;
using Promitor.Agents.Scraper.Validation;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Integrations.AzureMonitor.Logging;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Promitor.Agents.Scraper
{
    public class Startup : AgentStartup
    {
        private const string ApiName = "Promitor - Scraper API";
        private const string ComponentName = "Promitor Scraper";
        private readonly string _prometheusBaseUriPath;

        public Startup(IConfiguration configuration)
        : base(configuration)
        {
            var scrapeEndpointConfiguration = configuration.GetSection("prometheus:scrapeEndpoint").Get<ScrapeEndpointConfiguration>();
            _prometheusBaseUriPath = scrapeEndpointConfiguration.BaseUriPath;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string openApiDescription = BuildOpenApiDescription(Configuration);
            services.AddHttpClient()
                .UseWebApi()
                .AddHttpCorrelation()
                .AddAutoMapper(typeof(V1MappingProfile).Assembly)
                .DefineDependencies()
                .ConfigureYamlConfiguration(Configuration)
                .UseOpenApiSpecifications("Promitor - Scraper API v1", openApiDescription, 1)
                .AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy());

            ValidateRuntimeConfiguration(services);

            services.UseMetricSinks(Configuration)
                .ScheduleMetricScraping();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandling()
               .UseRequestTracking()
               .UseHttpCorrelation()
               .UseRouting()
               .UseMetricSinks(Configuration)
               .AddPrometheusScraperMetricSink(_prometheusBaseUriPath) // Deprecated and will be gone in 2.0
               .ExposeOpenApiUi() // New Swagger UI
               .ExposeOpenApiUi(ApiName, swaggerUiOptions =>
               {
                   swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName);
                   swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", "Promitor - Scraper API (OpenAPI 3.0)");
                   swaggerUiOptions.ConfigureDefaultOptions(ApiName);
               }, openApiOptions => openApiOptions.SerializeAsV2 = true)  // Deprecated Swagger UI
               .UseEndpoints(endpoints => endpoints.MapControllers());

            UseSerilog(ComponentName, app.ApplicationServices);
        }

        private void ValidateRuntimeConfiguration(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var runtimeValidator = serviceProvider.GetService<RuntimeValidator>();
            runtimeValidator.Run();
        }

        protected override LoggerConfiguration FilterTelemetry(LoggerConfiguration loggerConfiguration)
        {
            var standardConfiguration = base.FilterTelemetry(loggerConfiguration);

            var azureMonitorConfiguration = Configuration.Get<ScraperRuntimeConfiguration>()?.AzureMonitor?.Logging;
            if (azureMonitorConfiguration == null)
            {
                throw new Exception("Unable to get logging configuration for Azure Monitor");
            }

            standardConfiguration.Filter.With(new AzureMonitorLoggingFilter(azureMonitorConfiguration));

            return standardConfiguration;
        }

        private string BuildOpenApiDescription(IConfiguration configuration)
        {
            var metricSinkConfiguration = configuration.GetSection("metricSinks").Get<MetricSinkConfiguration>();

            var openApiDescriptionBuilder = new StringBuilder();
            openApiDescriptionBuilder.Append("Collection of APIs to manage the Promitor Scraper.\r\n\r\n");
            openApiDescriptionBuilder.AppendLine("Configured metric sinks are:\r\n");

            if (metricSinkConfiguration.PrometheusScrapingEndpoint != null)
            {
                var prometheusScrapingBaseUri = metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath;
                openApiDescriptionBuilder.AppendLine($"<li>Prometheus scrape endpoint is exposed at <a href=\"./../..{prometheusScrapingBaseUri}\" target=\"_blank\">{prometheusScrapingBaseUri}</a></li>");
            }

            if (metricSinkConfiguration.Statsd != null)
            {
                openApiDescriptionBuilder.AppendLine($"<li>StatsD server located on {metricSinkConfiguration.Statsd.Host}:{metricSinkConfiguration.Statsd.Port}</li>");
            }
            
            return openApiDescriptionBuilder.ToString();
        }
    }
}