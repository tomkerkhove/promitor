using System;
using Arcus.WebApi.Logging;
using Arcus.WebApi.Logging.Correlation;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core;
using Promitor.Agents.Scraper.Configuration;
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
            var openApiDescription = $"Collection of APIs to manage the Azure Monitor scrape endpoint for Prometheus.\r\nThe scrape endpoint is exposed at '<a href=\"./../..{_prometheusBaseUriPath}\" target=\"_blank\">{_prometheusBaseUriPath}</a>'";
            services.AddAutoMapper(typeof(V1MappingProfile).Assembly)
                .DefineDependencies()
                .ConfigureYamlConfiguration(Configuration)
                .UseOpenApiSpecifications("Promitor - Scraper API v1", openApiDescription, 1)
                .AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy());

            ValidateRuntimeConfiguration(services);

            services.UseMetricSinks(Configuration)
                .ScheduleMetricScraping()
                .AddHttpCorrelation()
                .UseWebApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>()
               .UseMiddleware<RequestTrackingMiddleware>()
               .UseHttpCorrelation()
               .UseRouting()
               .UsePrometheusScraper(_prometheusBaseUriPath)
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
    }
}