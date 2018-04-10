using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promitor.Scraper.Configuration.Providers;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using Promitor.Scraper.Scraping;
using Promitor.Scraper.Validation;
using Promitor.Scraper.Validation;
using Promitor.Scraper.Validation.Interfaces;
using Promitor.Scraper.Validation.Steps;
using Validator = Promitor.Scraper.Validation.Validator;

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
            services.AddTransient<IScrapeConfigurationProvider, ScrapeConfigurationProvider>();

            services.AddMvc();
            services.UseCronScheduler();
            services.UseOpenApiSpecifications(ScrapeEndpointBasePath, apiVersion: 1);
        }

        private IConfiguration BuildConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        private void ValidateSetup()
        {
            var validationSteps = new List<IValidationStep>
            {
                new ConfigurationPathValidationStep(),
                new ScrapingScheduleValidationStep()
            };

            Validator.Run(validationSteps);
        }
    }
}