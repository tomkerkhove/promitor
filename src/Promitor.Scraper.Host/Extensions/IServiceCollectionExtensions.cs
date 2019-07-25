using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.FeatureFlags;
using Promitor.Core.Configuration.Model.FeatureFlags;
using Promitor.Core.Configuration.Model.Metrics;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Configuration.Model.Server;
using Promitor.Core.Configuration.Model.Telemetry;
using Promitor.Core.Configuration.Model.Telemetry.Sinks;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Factories;
using Promitor.Core.Telemetry;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Core.Telemetry.Loggers;
using Promitor.Core.Telemetry.Metrics;
using Promitor.Core.Telemetry.Metrics.Interfaces;
using Promitor.Scraper.Host.Scheduling;
using Promitor.Scraper.Host.Validation;
using Swashbuckle.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace Promitor.Scraper.Host.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///     Defines to use the cron scheduler
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        public static void ScheduleMetricScraping(this IServiceCollection services)
        {
            var spToCreateJobsWith = services.BuildServiceProvider();
            var metricsProvider = spToCreateJobsWith.GetService<IMetricsDeclarationProvider>();
            var metrics = metricsProvider.Get(applyDefaults: true);

            foreach (var metric in metrics.Metrics)
            {
                services.AddScheduler(builder =>
                {
                    builder.AddJob(serviceProvider => new MetricScrapingJob(metric,
                        metricsProvider,
                        serviceProvider.GetService<IRuntimeMetricsCollector>(),
                        serviceProvider.GetService<MetricScraperFactory>(),
                        serviceProvider.GetService<ILogger>(),
                        serviceProvider.GetService<IExceptionTracker>()));
                    builder.UnobservedTaskExceptionHandler = (sender, exceptionEventArgs) => UnobservedJobHandlerHandler(sender, exceptionEventArgs, services);
                });
            }
        }

        /// <summary>
        ///     Expose services as Web API
        /// </summary>
        public static void UseWebApi(this IServiceCollection services)
        {
            services.AddMvc()
                    .AddJsonOptions(jsonOptions =>
                    {
                        jsonOptions.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    });
        }

        /// <summary>
        ///     Inject configuration
        /// </summary>
        public static void InjectConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FeatureFlagsConfiguration>(configuration.GetSection("featureFlags"));
            services.Configure<MetricsConfiguration>(configuration.GetSection("metricsConfiguration"));
            services.Configure<TelemetryConfiguration>(configuration.GetSection("telemetry"));
            services.Configure<ApplicationInsightsConfiguration>(configuration.GetSection("telemetry:applicationInsights"));
            services.Configure<ContainerLogConfiguration>(configuration.GetSection("telemetry:containerLogs"));
            services.Configure<ServerConfiguration>(configuration.GetSection("server"));
            services.Configure<PrometheusConfiguration>(configuration.GetSection("prometheus"));
            services.Configure<ScrapeEndpointConfiguration>(configuration.GetSection("prometheus:scrapeEndpoint"));
        }

        /// <summary>
        ///     Inject dependencies
        /// </summary>
        public static void InjectDependencies(this IServiceCollection services)
        {
            services.AddTransient<IExceptionTracker, ApplicationInsightsTelemetry>();
            services.AddTransient<ILogger, RuntimeLogger>();
            services.AddTransient<IMetricsDeclarationProvider, MetricsDeclarationProvider>();
            services.AddTransient<IRuntimeMetricsCollector, RuntimeMetricsCollector>();
            services.AddTransient<FeatureToggleClient>();
            services.AddTransient<MetricScraperFactory>();
            services.AddTransient<RuntimeValidator>();
            services.AddTransient<ValidationLogger>();
        }

        /// <summary>
        ///     Use OpenAPI specification
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        /// <param name="prometheusScrapeEndpointPath">Endpoint where the prometheus scraping is exposed</param>
        /// <param name="apiVersion">Version of the API</param>
        public static void UseOpenApiSpecifications(this IServiceCollection services, string prometheusScrapeEndpointPath, int apiVersion)
        {
            var openApiInformation = new Info
            {
                Contact = new Contact
                {
                    Name = "Tom Kerkhove",
                    Url = "https://blog.tomkerkhove.be"
                },
                Title = $"Promitor v{apiVersion}",
                Description = $"Collection of APIs to manage the Azure Monitor scrape endpoint for Prometheus.\r\nThe scrape endpoint is exposed at '<a href=\"./..{prometheusScrapeEndpointPath}\" target=\"_blank\">{prometheusScrapeEndpointPath}</a>'",
                Version = $"v{apiVersion}",
                License = new License
                {
                    Name = "MIT",
                    Url = "https://github.com/tomkerkhove/promitor/LICENSE"
                }
            };

            var xmlDocumentationPath = GetXmlDocumentationPath(services);

            services.AddSwaggerGen(swaggerGenerationOptions =>
            {
                swaggerGenerationOptions.EnableAnnotations();
                swaggerGenerationOptions.SwaggerDoc($"v{apiVersion}", openApiInformation);
                swaggerGenerationOptions.DescribeAllEnumsAsStrings();

                if (string.IsNullOrEmpty(xmlDocumentationPath) == false)
                {
                    swaggerGenerationOptions.IncludeXmlComments(xmlDocumentationPath);
                }
            });
        }

        private static string GetXmlDocumentationPath(IServiceCollection services)
        {
            var hostingEnvironment = services.FirstOrDefault(service => service.ServiceType == typeof(IHostingEnvironment));
            if (hostingEnvironment == null)
            {
                return string.Empty;
            }

            var contentRootPath = ((IHostingEnvironment)hostingEnvironment.ImplementationInstance).ContentRootPath;
            var xmlDocumentationPath = $"{contentRootPath}/Docs/Open-Api.xml";

            return File.Exists(xmlDocumentationPath) ? xmlDocumentationPath : string.Empty;
        }

        // ReSharper disable once UnusedParameter.Local
        private static void UnobservedJobHandlerHandler(object sender, UnobservedTaskExceptionEventArgs e, IServiceCollection services)
        {
            var exceptionTrackerService = services.FirstOrDefault(service => service.ServiceType == typeof(IExceptionTracker));
            var exceptionTracker = (IExceptionTracker)exceptionTrackerService?.ImplementationInstance;
            exceptionTracker?.Track(e.Exception);

            e.SetObserved();
        }
    }
}
