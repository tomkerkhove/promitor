using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promitor.Scraper.Configuration;
using Promitor.Scraper.Configuration.Providers;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using Promitor.Scraper.Scraping;

namespace Promitor.Scraper
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            ValidateSetup();

            Configuration = BuildConfiguration();
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

            services.AddTransient<IScrapeConfigurationProvider, ScrapeConfigurationProvider>();
        }

        private IConfiguration BuildConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        private void ValidateSetup()
        {
            var configurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.ConfigurationPath);
            if (File.Exists(configurationPath) == false)
            {
                var errorMessage = $"Scrape configuration at '{configurationPath}' does not exist";
                Console.WriteLine(errorMessage);
                throw new Exception(errorMessage);
            }

            Console.WriteLine($"Scrape configuration found at '{configurationPath}'");
        }
    }
}