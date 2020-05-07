using Arcus.WebApi.Logging;
using Arcus.WebApi.Logging.Correlation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core;
using Promitor.Agents.ResourceDiscovery.Health;
using Promitor.Agents.Scraper.Extensions;

namespace Promitor.Agents.ResourceDiscovery
{
    public class Startup : AgentStartup
    {
        private const string ApiName = "Promitor - Resource Discovery API";
        private const string ApiDescription = "Collection of APIs to provide automatic resource discovery for scraping resources with Promitor Scraper";
        private const string ComponentName = "Promitor Resource Discovery";

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
            services.AddRuntimeConfiguration(Configuration)
                .AddAzureResourceGraph(Configuration)
                .AddHealthChecks()
                    .AddCheck<AzureResourceGraphHealthCheck>("azure-resource-graph", failureStatus: HealthStatus.Unhealthy);

            services.UseOpenApiSpecifications($"{ApiName} v1", ApiDescription, 1)
                    .UseWebApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestTrackingMiddleware>();
            app.UseHttpCorrelation();
            app.UseRouting();

            app.ExposeOpenApiUi(ApiName);
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            UseSerilog(ComponentName, app.ApplicationServices);
        }
    }
}