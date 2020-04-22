using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Agents.Scraper.Validation;
using Promitor.Agents.Scraper.Extensions;
using Serilog;

namespace Promitor.Agents.Scraper
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string _prometheusBaseUriPath;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

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

            ValidateRuntimeConfiguration(app);

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
                .ConfigureYamlConfiguration(_configuration)
                .UseOpenApiSpecifications(_prometheusBaseUriPath, 1)
                .UseHealthChecks()
                .ScheduleMetricScraping()
                .UseWebApi();
        }

        private void ValidateRuntimeConfiguration(IApplicationBuilder app)
        {
            var runtimeValidator = app.ApplicationServices.GetService<RuntimeValidator>();
            runtimeValidator.Run();
        }
    }
}