using Bogus;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Model;
using Promitor.Core.Configuration.Model.Metrics;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Configuration.Model.Server;
using Promitor.Core.Configuration.Model.Telemetry;
using Promitor.Core.Configuration.Model.Telemetry.Sinks;

namespace Promitor.Tests.Unit.Generators.Config
{
    internal static class BogusRuntimeConfigurationGenerator
    {
        internal static RuntimeConfiguration Generate()
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
            var prometheusConfiguration = new Faker<PrometheusConfiguration>()
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

            var runtimeConfiguration = new RuntimeConfiguration
            {
                Server = serverConfiguration,
                MetricsConfiguration = metricsConfiguration,
                Prometheus = prometheusConfiguration,
                Telemetry = telemetryConfiguration
            };

            return runtimeConfiguration;
        }
    }
}