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
using Shuttle.Core.Cron;

namespace Promitor.Scraper
{
    public class Startup
    {
        private const string DefaultCronSchedule = "*/5 * * * *";

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
            services.AddTransient<IScrapeConfigurationProvider, ScrapeConfigurationProvider>();

            services.AddMvc();
            services.UseCronScheduler();
            services.UseOpenApiSpecifications(ScrapeEndpointBasePath, apiVersion: 1);
        }

        private static void ValidateScrapingConfiguration()
        {
            var configurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.ConfigurationPath);
            if (string.IsNullOrWhiteSpace(configurationPath))
            {
                Console.WriteLine("No scrape configuration configured, falling back to default one...");
                configurationPath = "default-scrape-configuration.yaml";
                Environment.SetEnvironmentVariable(EnvironmentVariables.ConfigurationPath, configurationPath);
            }

            if (File.Exists(configurationPath) == false)
            {
                var errorMessage = $"Scrape configuration at '{configurationPath}' does not exist";
                Console.WriteLine(errorMessage);
                throw new Exception(errorMessage);
            }

            Console.WriteLine($"Scrape configuration found at '{configurationPath}'");
        }

        private IConfiguration BuildConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        private void ValidateScrapingSchedule()
        {
            var scrapingCronSchedule = Environment.GetEnvironmentVariable(EnvironmentVariables.ScrapeCronSchedule);
            if (string.IsNullOrWhiteSpace(scrapingCronSchedule))
            {
                Console.WriteLine($"No scraping schedule was specified, falling back to default '{DefaultCronSchedule}' cron schedule...");
                Environment.SetEnvironmentVariable(EnvironmentVariables.ScrapeCronSchedule, DefaultCronSchedule);
                scrapingCronSchedule = DefaultCronSchedule;
            }

            try
            {
                new CronExpression(scrapingCronSchedule);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"No valid scraping schedule was specified - '{scrapingCronSchedule}'. Details: {exception.Message}");
                throw;
            }
        }

        private void ValidateSetup()
        {
            ValidateScrapingConfiguration();
            ValidateScrapingSchedule();
        }
    }
}