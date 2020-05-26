using Bogus;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Agents.Core.Configuration.Telemetry.Sinks;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Statsd.Configuration;

namespace Promitor.Tests.Unit.Generators.Config
{
    internal static class BogusScraperRuntimeConfigurationGenerator
    {
        internal static ScraperRuntimeConfiguration Generate()
        {
            var serverConfiguration = new Faker<ServerConfiguration>()
                .StrictMode(true)
                .RuleFor(srvConfig => srvConfig.HttpPort, faker => faker.Random.Int())
                .Generate();
            var metricsConfiguration = new Faker<MetricsConfiguration>()
                .StrictMode(true)
                .RuleFor(metricConfiguration => metricConfiguration.AbsolutePath, faker => faker.System.DirectoryPath())
                .Generate();
            var scrapeEndpointConfiguration = new Faker<ScrapeEndpointConfiguration>()
                .StrictMode(true)
                .RuleFor(flagsConfiguration => flagsConfiguration.BaseUriPath, faker => faker.System.DirectoryPath())
                .Generate();
            var prometheusConfiguration = new Faker<PrometheusLegacyConfiguration>()
                .StrictMode(true)
                .RuleFor(promConfiguration => promConfiguration.ScrapeEndpoint, scrapeEndpointConfiguration)
                .RuleFor(promConfiguration => promConfiguration.MetricUnavailableValue, faker => faker.Random.Double(min: 1))
                .RuleFor(promConfiguration => promConfiguration.EnableMetricTimestamps, faker => faker.Random.Bool())
                .Generate();

            var containerLogConfiguration = new Faker<ContainerLogConfiguration>()
                .StrictMode(true)
                .RuleFor(containerConfiguration => containerConfiguration.Verbosity, faker => LogLevel.Error)
                .RuleFor(containerConfiguration => containerConfiguration.IsEnabled, faker => faker.Random.Bool())
                .Generate();

            var applicationInsightsConfiguration = new Faker<ApplicationInsightsConfiguration>()
                .StrictMode(true)
                .RuleFor(containerConfiguration => containerConfiguration.Verbosity, faker => LogLevel.Error)
                .RuleFor(containerConfiguration => containerConfiguration.IsEnabled, faker => faker.Random.Bool())
                .RuleFor(containerConfiguration => containerConfiguration.InstrumentationKey, faker => faker.Random.Guid().ToString())
                .Generate();

            var telemetryConfiguration = new Faker<TelemetryConfiguration>()
                .StrictMode(true)
                .RuleFor(telemetry => telemetry.DefaultVerbosity, faker => LogLevel.Error)
                .RuleFor(telemetry => telemetry.ContainerLogs, faker => containerLogConfiguration)
                .RuleFor(telemetry => telemetry.ApplicationInsights, faker => applicationInsightsConfiguration)
                .Generate();
            var statsDConfiguration = new Faker<StatsdSinkConfiguration>()
                .StrictMode(true)
                .RuleFor(statsdSinkConfiguration => statsdSinkConfiguration.Host, faker => faker.Person.FirstName)
                .RuleFor(statsdSinkConfiguration => statsdSinkConfiguration.Port, faker => faker.Random.Int(min: 0))
                .RuleFor(statsdSinkConfiguration => statsdSinkConfiguration.MetricPrefix, faker => faker.Person.FirstName)
                .Generate();
            var prometheusScrapingEndpointSinkConfiguration = new Faker<PrometheusScrapingEndpointSinkConfiguration>()
                .StrictMode(true)
                .RuleFor(promConfiguration => promConfiguration.BaseUriPath, faker => faker.System.DirectoryPath())
                .RuleFor(promConfiguration => promConfiguration.MetricUnavailableValue, faker => faker.Random.Double(min: 1))
                .RuleFor(promConfiguration => promConfiguration.EnableMetricTimestamps, faker => faker.Random.Bool())
                .Generate();
            var metricSinkConfiguration = new Faker<MetricSinkConfiguration>()
                .StrictMode(true)
                .RuleFor(sinkConfiguration => sinkConfiguration.Statsd, statsDConfiguration)
                .RuleFor(sinkConfiguration => sinkConfiguration.PrometheusScrapingEndpoint, prometheusScrapingEndpointSinkConfiguration)
                .Generate();
            var resourceDiscovery = new Faker<ResourceDiscoveryConfiguration>()
                .StrictMode(true)
                .RuleFor(resourceDiscoveryConfiguration => resourceDiscoveryConfiguration.Host, faker => faker.Person.FirstName)
                .RuleFor(resourceDiscoveryConfiguration => resourceDiscoveryConfiguration.Port, faker => faker.Random.Int(min: 1))
                .Generate();

            var runtimeConfiguration = new ScraperRuntimeConfiguration
            {
                MetricsConfiguration = metricsConfiguration,
                MetricSinks = metricSinkConfiguration,
                ResourceDiscovery = resourceDiscovery,
                Prometheus = prometheusConfiguration,
                Server = serverConfiguration,
                Telemetry = telemetryConfiguration
            };

            return runtimeConfiguration;
        }
    }
}