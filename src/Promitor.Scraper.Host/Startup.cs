using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Scraper.Host.Extensions;
using Promitor.Scraper.Host.Validation;

namespace Promitor.Scraper.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            var scrapeEndpointConfiguration = configuration.GetSection("prometheus:scrapeEndpoint").Get<ScrapeEndpointConfiguration>();
            _prometheusBaseUriPath = scrapeEndpointConfiguration.BaseUriPath;
        }

        private readonly IConfiguration _configuration;
        private readonly string _prometheusBaseUriPath;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ValidateRuntimeConfiguration(app);

            app.UseMvc();
            app.UsePrometheusScraper(_prometheusBaseUriPath);
            app.UseOpenApiUi();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InjectConfiguration(_configuration);
            services.InjectDependencies();

            services.UseWebApi();
            services.UseOpenApiSpecifications(_prometheusBaseUriPath, apiVersion: 1);
            services.ScheduleMetricScraping();
        }

        private void ValidateRuntimeConfiguration(IApplicationBuilder app)
        {
            var runtimeValidator = app.ApplicationServices.GetService<RuntimeValidator>();
            runtimeValidator.Run();
        }
    }
}