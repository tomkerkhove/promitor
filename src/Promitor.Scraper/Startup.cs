using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promitor.Scraper.Configuration.Providers;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using Promitor.Scraper.Scraping;
using Promitor.Scraper.Validation;
using Promitor.Scraper.Validation.Interfaces;
using Promitor.Scraper.Validation.Steps;

namespace Promitor.Scraper
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            WelcomeToPromitor();

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
            services.AddTransient<IMetricsDeclarationProvider, MetricsDeclarationProvider>();

            services.AddMvc();
            services.UseCronScheduler();
            services.UseOpenApiSpecifications(ScrapeEndpointBasePath, apiVersion: 1);
        }

        private static void WelcomeToPromitor()
        {
            Console.WriteLine(Constants.Texts.Welcome);
        }

        private IConfiguration BuildConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        private void ValidateSetup()
        {
            var scrapeConfigurationProvider = new MetricsDeclarationProvider();
            var validationSteps = new List<IValidationStep>
            {
                new ConfigurationPathValidationStep(),
                new ScrapingScheduleValidationStep(),
                new AzureAuthenticationValidationStep(),
                new MetricsDeclarationValidationStep(scrapeConfigurationProvider)
            };

            Validator.Run(validationSteps);
        }
    }
}