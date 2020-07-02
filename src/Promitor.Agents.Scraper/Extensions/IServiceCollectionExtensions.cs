using JustEat.StatsD;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Agents.Core.Configuration.Telemetry.Sinks;
using Promitor.Agents.Scraper;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Agents.Scraper.Discovery;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Factories;
using Promitor.Agents.Scraper.Validation;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Integrations.AzureMonitor.Configuration;
using Promitor.Integrations.Sinks.Prometheus;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Statsd;
using Promitor.Integrations.Sinks.Statsd.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///     Defines the dependencies that Promitor requires
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        public static IServiceCollection DefineDependencies(this IServiceCollection services)
        {
            services.AddTransient<ResourceDiscoveryClient>();
            services.AddTransient<ResourceDiscoveryRepository>();
            services.AddTransient<IMetricsDeclarationProvider, MetricsDeclarationProvider>();
            services.AddTransient<IRuntimeMetricsCollector, RuntimeMetricsCollector>();
            services.AddTransient<MetricScraperFactory>();
            services.AddTransient<RuntimeValidator>();
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
            services.AddSingleton<IDeserializer<AzureResourceDiscoveryGroupDefinitionV1>, AzureResourceDiscoveryGroupDeserializer>();
            services.AddSingleton<IAzureResourceDeserializerFactory, AzureResourceDeserializerFactory>();
            services.AddSingleton<IDeserializer<MetricAggregationV1>, MetricAggregationDeserializer>();
            services.AddSingleton<IDeserializer<SecretV1>, SecretDeserializer>();

            return services;
        }

        /// <summary>
        ///     Adds the required metric sinks
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        /// <param name="configuration">Configuration of the application</param>
        public static IServiceCollection UseMetricSinks(this IServiceCollection services, IConfiguration configuration)
        {
            var metricSinkConfiguration = configuration.GetSection("metricSinks").Get<MetricSinkConfiguration>();
            if (metricSinkConfiguration?.Statsd != null)
            {
                AddStatsdMetricSink(services, metricSinkConfiguration.Statsd);
            }

            if (metricSinkConfiguration?.PrometheusScrapingEndpoint != null)
            {
                AddPrometheusMetricSink(services);
            }

            services.TryAddSingleton<MetricSinkWriter>();

            return services;
        }

        private static void AddPrometheusMetricSink(IServiceCollection services)
        {
            services.AddTransient<IMetricSink, PrometheusScrapingEndpointMetricSink>();
        }

        private static void AddStatsdMetricSink(IServiceCollection services, StatsdSinkConfiguration statsdConfiguration)
        {
            services.AddTransient<IMetricSink, StatsdMetricSink>();
            services.AddStatsD(provider =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<StatsdMetricSink>();
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
                        logger.LogCritical(ex, "Failed to emit metric to {StatsdHost} on {StatsdPort} with prefix {StatsdPrefix}", host, port, metricPrefix);
                        return true;
                    }
                };
            });
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
            services.Configure<PrometheusScrapingEndpointSinkConfiguration>(configuration.GetSection("prometheus"));
            services.Configure<ApplicationInsightsConfiguration>(configuration.GetSection("telemetry:applicationInsights"));
            services.Configure<ContainerLogConfiguration>(configuration.GetSection("telemetry:containerLogs"));
            services.Configure<ScrapeEndpointConfiguration>(configuration.GetSection("prometheus:scrapeEndpoint"));
            services.Configure<AzureMonitorConfiguration>(configuration.GetSection("azureMonitor"));
            services.Configure<AzureMonitorLoggingConfiguration>(configuration.GetSection("azureMonitor:logging"));

            return services;
        }
    }
}