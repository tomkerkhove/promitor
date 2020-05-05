using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core;
using Promitor.Agents.Scraper.Extensions;
using Promitor.Agents.Scraper.Validation;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Serilog;

namespace Promitor.Agents.Scraper
{
    public class Startup : AgentStartup
    {
        private readonly string _prometheusBaseUriPath;

        public Startup(IConfiguration configuration)
        : base(configuration)
        {
            var scrapeEndpointConfiguration = configuration.GetSection("prometheus:scrapeEndpoint").Get<ScrapeEndpointConfiguration>();
            _prometheusBaseUriPath = scrapeEndpointConfiguration.BaseUriPath;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePrometheusScraper(_prometheusBaseUriPath)
                .ExposeOpenApiUi()
                .UseSerilogRequestLogging()
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(V1MappingProfile).Assembly)
                .DefineDependencies()
                .ConfigureYamlConfiguration(Configuration)
                .UseOpenApiSpecifications(_prometheusBaseUriPath, 1)
                .UseHealthChecks();

            ValidateRuntimeConfiguration(services);

            services.UseMetricSinks(Configuration)
                .ScheduleMetricScraping()
                .UseWebApi();
        }

        private void ValidateRuntimeConfiguration(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var runtimeValidator = serviceProvider.GetService<RuntimeValidator>();
            runtimeValidator.Run();
        }
    }
}