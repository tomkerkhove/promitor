using System;
using GuardNet;
using JustEat.StatsD;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Agents.Core.Configuration.Telemetry.Sinks;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.Scraper;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Agents.Scraper.Discovery;
using Promitor.Agents.Scraper.Discovery.Interfaces;
using Promitor.Agents.Scraper.Scheduling;
using Promitor.Agents.Scraper.Usability;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Agents.Scraper.Validation.Steps.Sinks;
using Promitor.Core;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Factories;
using Promitor.Integrations.Azure.Authentication.Configuration;
using Promitor.Integrations.AzureMonitor.Configuration;
using Promitor.Integrations.Sinks.Atlassian.Statuspage;
using Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration;
using Promitor.Integrations.Sinks.OpenTelemetry;
using Promitor.Integrations.Sinks.OpenTelemetry.Extensions;
using Promitor.Integrations.Sinks.Prometheus;
using Promitor.Integrations.Sinks.Prometheus.Collectors;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Prometheus.Extensions;
using Promitor.Integrations.Sinks.Statsd;
using Promitor.Integrations.Sinks.Statsd.Configuration;
using Spectre.Console;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///     Add the Promitor Resource Discovery client
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        /// <param name="promitorUserAgent">User agent for Promitor</param>
        /// <param name="configuration"></param>
        public static IServiceCollection AddResourceDiscoveryClient(this IServiceCollection services, string promitorUserAgent, IConfiguration configuration)
        {
            Guard.NotNull(services, nameof(services));

            var resourceDiscoveryConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            if(resourceDiscoveryConfiguration?.ResourceDiscovery?.IsConfigured == true)
            {
                services.AddHttpClient<ResourceDiscoveryClient>(client =>
                {
                    // Provide Promitor User-Agent
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd(promitorUserAgent);
                });
                services.AddTransient<IResourceDiscoveryRepository, ResourceDiscoveryRepository>();
            }
            else
            {
                services.AddTransient<IResourceDiscoveryRepository, StubResourceDiscoveryRepository>();
            }

            return services;
        }

        /// <summary>
        ///     Add the Atlassian Statuspage client
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        /// <param name="promitorUserAgent">User agent for Promitor</param>
        /// <param name="configuration">Configuration of the agent</param>
        public static IServiceCollection AddAtlassianStatuspageClient(this IServiceCollection services, string promitorUserAgent, IConfiguration configuration)
        {
            Guard.NotNull(services, nameof(services));

            services.AddHttpClient<IAtlassianStatuspageClient, AtlassianStatuspageClient>(client =>
            {
                // Provide Promitor User-Agent
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(promitorUserAgent);

                // Auth all requests
                var apiKey = configuration[EnvironmentVariables.Integrations.AtlassianStatuspage.ApiKey];
                client.DefaultRequestHeaders.Add("Authorization", $"OAuth {apiKey}");
            });

            return services;
        }

        /// <summary>
        ///     Defines the dependencies that Promitor requires
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        public static IServiceCollection DefineDependencies(this IServiceCollection services)
        {
            Guard.NotNull(services, nameof(services));
            
            services.AddTransient<IMetricsDeclarationProvider, MetricsDeclarationProvider>();
            services.AddTransient<IAzureScrapingSystemMetricsPublisher, AzureScrapingSystemMetricsPublisher>();
            services.AddTransient<MetricScraperFactory>();
            services.AddTransient<ConfigurationSerializer>();
            services.AddSingleton<AzureMonitorClientFactory>();

            services.AddSingleton<IDeserializer<MetricsDeclarationV1>, V1Deserializer>();
            services.AddSingleton<IDeserializer<AzureMetadataV1>, AzureMetadataDeserializer>();
            services.AddSingleton<IDeserializer<MetricDefaultsV1>, MetricDefaultsDeserializer>();
            services.AddSingleton<IDeserializer<MetricDefinitionV1>, MetricDefinitionDeserializer>();
            services.AddSingleton<IDeserializer<AggregationV1>, AggregationDeserializer>();
            services.AddSingleton<IDeserializer<MetricDimensionV1>, MetricDimensionDeserializer>();
            services.AddSingleton<IDeserializer<ScrapingV1>, ScrapingDeserializer>();
            services.AddSingleton<IDeserializer<AzureMetricConfigurationV1>, AzureMetricConfigurationDeserializer>();
            services.AddSingleton<IDeserializer<LogAnalyticsConfigurationV1>, LogAnalyticsConfigurationDeserializer>();
            services.AddSingleton<IDeserializer<AzureResourceDiscoveryGroupDefinitionV1>, AzureResourceDiscoveryGroupDeserializer>();
            services.AddSingleton<IAzureResourceDeserializerFactory, AzureResourceDeserializerFactory>();
            services.AddSingleton<IDeserializer<MetricAggregationV1>, MetricAggregationDeserializer>();
            services.AddSingleton<IDeserializer<SecretV1>, SecretDeserializer>();

            return services;
        }

        /// <summary>
        ///     Adds usability
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        public static IServiceCollection AddUsability(this IServiceCollection services)
        {
            services.AddTransient<MetricsTableGenerator>();

            return services;
        }

        /// <summary>
        ///     Defines the validation for when Promitor starts up
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        public static IServiceCollection AddValidationRules(this IServiceCollection services)
        {
            services.AddTransient<IValidationStep, ConfigurationPathValidationStep>();
            services.AddTransient<IValidationStep, AzureAuthenticationValidationStep>();
            services.AddTransient<IValidationStep, MetricsDeclarationValidationStep>();
            services.AddTransient<IValidationStep, ResourceDiscoveryValidationStep>();
            services.AddTransient<IValidationStep, AtlassianStatuspageMetricSinkValidationStep>();
            services.AddTransient<IValidationStep, OpenTelemetryCollectorMetricSinkValidationStep>();
            services.AddTransient<IValidationStep, PrometheusScrapingEndpointMetricSinkValidationStep>();
            services.AddTransient<IValidationStep, StatsDMetricSinkValidationStep>();
            services.AddTransient<RuntimeValidator>();

            return services;
        }

        /// <summary>
        ///     Adds the required metric sinks
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <param name="agentVersion">Version of Promitor Scraper agent</param>
        /// <param name="logger">Logger to write logs to</param>
        public static IServiceCollection UseMetricSinks(this IServiceCollection services, IConfiguration configuration, string agentVersion, ILogger<Startup> logger)
        {
            var metricSinkConfiguration = configuration.GetSection("metricSinks").Get<MetricSinkConfiguration>();

            logger.LogInformation("The following metric sinks were configured and are being enabled:");

            // Create a table for listing all configured sinks
            var metricSinkAsciiTable = CreateAsciiTable();

            if (metricSinkConfiguration?.Statsd != null)
            {
                AddStatsdMetricSink(services, metricSinkConfiguration.Statsd, metricSinkAsciiTable);
            }

            if (metricSinkConfiguration?.PrometheusScrapingEndpoint != null)
            {
                AddPrometheusMetricSink(metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath, services, metricSinkAsciiTable);
            }

            if (metricSinkConfiguration?.AtlassianStatuspage != null)
            {
                AddAtlassianStatuspageMetricSink(metricSinkConfiguration.AtlassianStatuspage.PageId, services, metricSinkAsciiTable);
            }

            if (metricSinkConfiguration?.OpenTelemetryCollector != null
                && string.IsNullOrWhiteSpace(metricSinkConfiguration.OpenTelemetryCollector.CollectorUri) == false)
            {
                AddOpenTelemetryCollectorMetricSink(metricSinkConfiguration.OpenTelemetryCollector.CollectorUri, agentVersion, services, metricSinkAsciiTable);
            }

            AnsiConsole.Write(metricSinkAsciiTable);

            services.TryAddSingleton<MetricSinkWriter>();

            return services;
        }

        private static void AddPrometheusMetricSink(string baseUri, IServiceCollection services, Table metricSinkAsciiTable)
        {
            metricSinkAsciiTable.AddRow("Prometheus Scraping Endpoint", $"Url: {baseUri}.");
            services.AddTransient<IMetricSink, PrometheusScrapingEndpointMetricSink>();
            services.AddPrometheusSystemMetrics();
        }

        private static void AddAtlassianStatuspageMetricSink(string pageId, IServiceCollection services, Table metricSinkAsciiTable)
        {
            metricSinkAsciiTable.AddRow("Atlassian Statuspage", $"Page ID: {pageId}.");

            services.AddTransient<IMetricSink, AtlassianStatuspageMetricSink>();
        }

        const string OpenTelemetryServiceName = "promitor-scraper";

        private static void AddOpenTelemetryCollectorMetricSink(string collectorUri, string agentVersion, IServiceCollection services, Table metricSinkAsciiTable)
        {
            metricSinkAsciiTable.AddRow("OpenTelemetry Collector", $"Url: {collectorUri}.");

            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(OpenTelemetryServiceName, serviceVersion: agentVersion);

            services.AddOpenTelemetry()
                    .WithMetrics(metricsBuilder =>
                    {
                        metricsBuilder.SetResourceBuilder(resourceBuilder)
                                      .AddMeter("Promitor.Scraper.Metrics.AzureMonitor")
                                      .AddOtlpExporter(options => options.Endpoint = new Uri(collectorUri));
                    });
            services.AddTransient<IMetricSink, OpenTelemetryCollectorMetricSink>();
            services.AddTransient<OpenTelemetryCollectorMetricSink>();
            services.AddOpenTelemetrySystemMetrics();
        }

        private static void AddStatsdMetricSink(IServiceCollection services, StatsdSinkConfiguration statsdConfiguration, Table metricSinkAsciiTable)
        {            
            metricSinkAsciiTable.AddRow("StatsD", $"Url: {statsdConfiguration.Host}:{statsdConfiguration.Port}.");
            metricSinkAsciiTable.AddRow("", $"Format: {statsdConfiguration.MetricFormat}.");

            if (statsdConfiguration.MetricFormat == StatsdFormatterTypesEnum.Geneva)
            {
                metricSinkAsciiTable.AddRow("", $"  Geneva account: {statsdConfiguration.Geneva.Account}.");
                metricSinkAsciiTable.AddRow("", $"  Geneva namespace: {statsdConfiguration.Geneva.Namespace}.");
            }

            services.AddTransient<IMetricSink, StatsdMetricSink>();
            services.AddStatsD(provider =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var sinkLogger = loggerFactory.CreateLogger<StatsdMetricSink>();
                var host = statsdConfiguration.Host;
                var port = statsdConfiguration.Port;
                var metricPrefix = statsdConfiguration.MetricPrefix;

                return new StatsDConfiguration
                {
                    Host = host,
                    Port = port,
                    Prefix = metricPrefix,
                    OnError = ex =>
                    {
                        sinkLogger.LogCritical(ex, "Failed to emit metric to {StatsdHost} on {StatsdPort} with prefix {StatsdPrefix}", host, port, metricPrefix);
                        return true;
                    }
                };
            });
        }

        private static Table CreateAsciiTable()
        {
            var asciiTable = new Table
            {
                Border = TableBorder.HeavyEdge
            };

            // Add some columns
            asciiTable.AddColumn("Metric Sink");
            asciiTable.AddColumn("Details");
            asciiTable.Caption("Configured Metric Sinks");

            return asciiTable;
        }

        /// <summary>
        /// Adds a semaphore-based implementation of <see cref="IScrapingMutex"/> to the <see cref="IServiceCollection" />.
        /// </summary>
        public static IServiceCollection AddScrapingMutex(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var serverConfiguration = configuration.GetSection("server").Get<ServerConfiguration>();
            
            services.TryAdd(ServiceDescriptor.Singleton<IScrapingMutex, ScrapingMutex>(_ => ScrapingMutexBuilder(serverConfiguration)));
            
            return services;
        }

        private static ScrapingMutex ScrapingMutexBuilder(ServerConfiguration serverConfiguration)
        {
            return serverConfiguration.MaxDegreeOfParallelism > 0
                ? new ScrapingMutex(serverConfiguration.MaxDegreeOfParallelism)
                : null;
        }

        /// <summary>
        ///     Inject configuration
        /// </summary>
        public static IServiceCollection ConfigureYamlConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ScraperRuntimeConfiguration>(configuration);
            services.Configure<MetricsConfiguration>(configuration.GetSection("metricsConfiguration"));
            services.Configure<ResourceDiscoveryConfiguration>(configuration.GetSection("resourceDiscovery"));
            services.Configure<TelemetryConfiguration>(configuration.GetSection("telemetry"));
            services.Configure<ServerConfiguration>(configuration.GetSection("server"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("authentication"));
            services.Configure<PrometheusScrapingEndpointSinkConfiguration>(configuration.GetSection("metricSinks:prometheusScrapingEndpoint"));
            services.Configure<StatsdSinkConfiguration>(configuration.GetSection("metricSinks:statsd"));
            services.Configure<AtlassianStatusPageSinkConfiguration>(configuration.GetSection("metricSinks:atlassianStatuspage"));
            services.Configure<ApplicationInsightsConfiguration>(configuration.GetSection("telemetry:applicationInsights"));
            services.Configure<ContainerLogConfiguration>(configuration.GetSection("telemetry:containerLogs"));
            services.Configure<ScrapeEndpointConfiguration>(configuration.GetSection("prometheus:scrapeEndpoint"));
            services.Configure<AzureMonitorConfiguration>(configuration.GetSection("azureMonitor"));
            services.Configure<AzureMonitorIntegrationConfiguration>(configuration.GetSection("azureMonitor:integration"));
            services.Configure<AzureMonitorLoggingConfiguration>(configuration.GetSection("azureMonitor:logging"));

            return services;
        }
    }
}