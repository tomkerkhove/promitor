using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core;
using Promitor.Agents.Core.Extensions;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Agents.Scraper.Extensions;
using Promitor.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Integrations.AzureMonitor.Logging;
using Serilog;
using Version = Promitor.Core.Version;

namespace Promitor.Agents.Scraper
{
    public class Startup : AgentStartup
    {
        private const string ApiName = "Promitor - Scraper API";
        private const string ComponentName = "Promitor Scraper";
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
            : base(configuration)
        {
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string agentVersion = Version.Get();

            var promitorUserAgent = UserAgent.Generate("Scraper", agentVersion);
            string openApiDescription = BuildOpenApiDescription(Configuration);

            services.UseWebApi()
                .AddMemoryCache()
                .AddResourceDiscoveryClient(promitorUserAgent, Configuration)
                .AddAtlassianStatuspageClient(promitorUserAgent, Configuration)
                .AddUsability()
                .AddHttpCorrelation(options => options.UpstreamService.ExtractFromRequest = true)
                .AddAutoMapper(typeof(V1MappingProfile).Assembly)
                .DefineDependencies()
                .AddValidationRules()
                .ConfigureYamlConfiguration(Configuration)
                .UseOpenApiSpecifications("Promitor - Scraper API v1", openApiDescription, 1);

            services.AddHealthChecks()
                   .AddResourceDiscoveryHealthCheck(Configuration);
            
            services.UseMetricSinks(Configuration, _logger)
                .AddSystemMetrics()
                .AddScrapingMutex(Configuration)
                .ScheduleMetricScraping();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandling()
                .UseRequestTracking()
                .UseHttpCorrelation()
                .UseVersionMiddleware()
                .UseRouting()
                .UseMetricSinks(Configuration, logger)
                .ExposeOpenApiUi(ApiName)
                .UseEndpoints(endpoints => endpoints.MapControllers());

            UseSerilog(ComponentName, app.ApplicationServices);
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

            if (metricSinkConfiguration == null)
            {
                return openApiDescriptionBuilder.ToString();
            }

            if (metricSinkConfiguration.PrometheusScrapingEndpoint != null)
            {
                var prometheusScrapingBaseUri = metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath;
                openApiDescriptionBuilder.AppendLine($"<li>Prometheus scrape endpoint is exposed at <a href=\"../../..{prometheusScrapingBaseUri}\" target=\"_blank\">{prometheusScrapingBaseUri}</a></li>");
            }

            if (metricSinkConfiguration.OpenTelemetryCollector != null)
            {
                openApiDescriptionBuilder.AppendLine($"<li>OpenTelemetry Collector located on {metricSinkConfiguration.OpenTelemetryCollector.CollectorUri}</li>");
            }

            if (metricSinkConfiguration.Statsd != null)
            {
                openApiDescriptionBuilder.AppendLine($"<li>StatsD server located on {metricSinkConfiguration.Statsd.Host}:{metricSinkConfiguration.Statsd.Port}</li>");
            }

            return openApiDescriptionBuilder.ToString();
        }
    }
}