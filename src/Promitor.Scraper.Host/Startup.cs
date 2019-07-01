using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Telemetry;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Core.Telemetry.Loggers;
using Promitor.Core.Telemetry.Metrics;
using Promitor.Core.Telemetry.Metrics.Interfaces;
using Promitor.Scraper.Host.Extensions;
using Promitor.Scraper.Host.Validation;

namespace Promitor.Scraper.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            _prometheusBaseUriPath = ScrapeEndpoint.DetermineBaseUri(configuration);
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

            app.UseMvc();
            app.UsePrometheusScraper(_prometheusBaseUriPath);
            app.UseOpenApiUi();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ValidateRuntimeConfiguration();

            services.AddTransient<IExceptionTracker, ApplicationInsightsTelemetry>();
            services.AddTransient<ILogger, RuntimeLogger>();
            services.AddTransient<IMetricsDeclarationProvider, MetricsDeclarationProvider>();
            services.AddTransient<IRuntimeMetricsCollector, RuntimeMetricsCollector>();

            services.AddMvc()
                    .AddJsonOptions(jsonOptions =>
                    {
                        jsonOptions.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    });

            services.UseOpenApiSpecifications(_prometheusBaseUriPath, apiVersion: 1);
            services.ScheduleMetricScraping();
        }

        private void ValidateRuntimeConfiguration()
        {
            var runtimeValidator = new RuntimeValidator(_configuration);
            runtimeValidator.Run();
        }
    }
}