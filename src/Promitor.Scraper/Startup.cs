using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promitor.Scraper.Scraping;

namespace Promitor.Scraper
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
            ScrapeEndpointBasePath = ScrapeEndpoint.GetBasePath(Configuration);
        }

        public IConfiguration Configuration { get; }
        public string ScrapeEndpointBasePath { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UsePrometheusScraper(ScrapeEndpointBasePath);
            app.UseOpenApiUi();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.UseOpenApiSpecifications(ScrapeEndpointBasePath, apiVersion: 1);
        }
    }
}