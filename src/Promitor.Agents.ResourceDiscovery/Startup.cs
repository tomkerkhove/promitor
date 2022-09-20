using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core;
using Promitor.Agents.Core.Extensions;
using Promitor.Agents.ResourceDiscovery.Extensions;
using Promitor.Agents.ResourceDiscovery.Health;
using Promitor.Integrations.Sinks.Prometheus.Extensions;

namespace Promitor.Agents.ResourceDiscovery
{
    public class Startup : AgentStartup
    {
        private const string ApiName = "Promitor - Resource Discovery API";
        private const string ApiDescription = "Collection of APIs to provide automatic resource discovery for scraping resources with Promitor Scraper";
        private const string ComponentName = "Promitor Resource Discovery";
        private const string PrometheusBaseUri = "/metrics";

        /// <summary>
        ///     Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.UseWebApi()
                .AddMemoryCache()
                .AddRuntimeConfiguration(Configuration)
                .AddSystemMetrics(sinkBuilder =>
                {
                    sinkBuilder.AddPrometheusSystemMetrics();
                })
                .AddAzureResourceGraph(Configuration)
                .AddBackgroundJobs()
                .AddUsability()
                .UseOpenApiSpecifications($"{ApiName} v1", ApiDescription, 1)
                .AddValidationRules()
                .AddHttpCorrelation(options => options.UpstreamService.ExtractFromRequest = true)
                .AddHealthChecks()
                    .AddCheck<AzureResourceGraphHealthCheck>("azure-resource-graph", failureStatus: HealthStatus.Unhealthy);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandling();
            app.UseRequestTracking();
            app.UseHttpCorrelation();
            app.UseVersionMiddleware();
            app.UseRouting();

            app.ExposeOpenApiUi(ApiName);
            app.UsePrometheusMetrics(PrometheusBaseUri, logger);
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            UseSerilog(ComponentName, app.ApplicationServices);
        }
    }
}