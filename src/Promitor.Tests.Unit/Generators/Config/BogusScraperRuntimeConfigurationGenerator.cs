using System.Collections.Generic;
using Bogus;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Agents.Core.Configuration.Telemetry.Sinks;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Configuration.Sinks;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration;
using Promitor.Integrations.Sinks.OpenTelemetry.Configuration;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Statsd.Configuration;

namespace Promitor.Tests.Unit.Generators.Config
{
    internal static class BogusScraperRuntimeConfigurationGenerator
    {
        internal static ScraperRuntimeConfiguration Generate()
        {
            var serverConfiguration = GenerateServerConfiguration();
            var metricsConfiguration = GenerateMetricsConfiguration();
            var containerLogConfiguration = GenerateContainerLogConfiguration();
            var applicationInsightsConfiguration = GenerateApplicationInsightsConfiguration();
            var telemetryConfiguration = GenerateTelemetryConfiguration(containerLogConfiguration, applicationInsightsConfiguration);
            var metricSinkConfiguration = GenerateMetricSinkConfiguration();
            var resourceDiscovery = GenerateResourceDiscoveryConfiguration();

            var runtimeConfiguration = new ScraperRuntimeConfiguration
            {
                MetricsConfiguration = metricsConfiguration,
                MetricSinks = metricSinkConfiguration,
                ResourceDiscovery = resourceDiscovery,
                Server = serverConfiguration,
                Telemetry = telemetryConfiguration
            };

            return runtimeConfiguration;
        }

        private static ResourceDiscoveryConfiguration GenerateResourceDiscoveryConfiguration()
        {
            var resourceDiscovery = new Faker<ResourceDiscoveryConfiguration>()
                .StrictMode(true)
                .RuleFor(resourceDiscoveryConfiguration => resourceDiscoveryConfiguration.Host, faker => faker.Person.FirstName)
                .RuleFor(resourceDiscoveryConfiguration => resourceDiscoveryConfiguration.Port, faker => faker.Random.Int(min: 1))
                .Generate();
            return resourceDiscovery;
        }

        private static MetricSinkConfiguration GenerateMetricSinkConfiguration()
        {
            var atlassianStatusPageSinkConfiguration = GenerateAtlassianStatusPageSinkConfiguration();
            var openTelemetryCollectorConfiguration = GenerateOpenTelemetryCollectorSinkConfiguration();
            var prometheusScrapingEndpointSinkConfiguration = GeneratePrometheusScrapingEndpointSinkConfiguration();
            var statsDConfiguration = GenerateStatsdSinkConfiguration();
            var metricSinkConfiguration = new Faker<MetricSinkConfiguration>()
                .StrictMode(true)
                .RuleFor(sinkConfiguration => sinkConfiguration.Statsd, statsDConfiguration)
                .RuleFor(sinkConfiguration => sinkConfiguration.OpenTelemetryCollector, openTelemetryCollectorConfiguration)
                .RuleFor(sinkConfiguration => sinkConfiguration.AtlassianStatuspage, atlassianStatusPageSinkConfiguration)
                .RuleFor(sinkConfiguration => sinkConfiguration.PrometheusScrapingEndpoint, prometheusScrapingEndpointSinkConfiguration)
                .Generate();
            return metricSinkConfiguration;
        }

        private static StatsdSinkConfiguration GenerateStatsdSinkConfiguration()
        {
            var statsDConfiguration = new Faker<StatsdSinkConfiguration>()
                .StrictMode(true)
                .RuleFor(statsdSinkConfiguration => statsdSinkConfiguration.Host, faker => faker.Person.FirstName)
                .RuleFor(statsdSinkConfiguration => statsdSinkConfiguration.Port, faker => faker.Random.Int(min: 0))
                .RuleFor(statsdSinkConfiguration => statsdSinkConfiguration.MetricPrefix, faker => faker.Person.FirstName)
                .RuleFor(statsdSinkConfiguration => statsdSinkConfiguration.MetricFormat, _ => StatsdFormatterTypesEnum.Default)
                .RuleFor(statsdSinkConfiguration => statsdSinkConfiguration.Geneva, faker => new GenevaConfiguration 
                { 
                    Account = faker.Person.FirstName, 
                    Namespace = faker.Person.LastName 
                })
                .Generate();
            return statsDConfiguration;
        }

        private static TelemetryConfiguration GenerateTelemetryConfiguration(ContainerLogConfiguration containerLogConfiguration, ApplicationInsightsConfiguration applicationInsightsConfiguration)
        {
            var telemetryConfiguration = new Faker<TelemetryConfiguration>()
                .StrictMode(true)
                .RuleFor(telemetry => telemetry.DefaultVerbosity, _ => LogLevel.Error)
                .RuleFor(telemetry => telemetry.ContainerLogs, _ => containerLogConfiguration)
                .RuleFor(telemetry => telemetry.ApplicationInsights, _ => applicationInsightsConfiguration)
                .Generate();
            return telemetryConfiguration;
        }

        private static ApplicationInsightsConfiguration GenerateApplicationInsightsConfiguration()
        {
            var applicationInsightsConfiguration = new Faker<ApplicationInsightsConfiguration>()
                .StrictMode(true)
                .RuleFor(containerConfiguration => containerConfiguration.Verbosity, _ => LogLevel.Error)
                .RuleFor(containerConfiguration => containerConfiguration.IsEnabled, faker => faker.Random.Bool())
                .RuleFor(containerConfiguration => containerConfiguration.InstrumentationKey, faker => faker.Random.Guid().ToString())
                .Generate();
            return applicationInsightsConfiguration;
        }

        private static MetricsConfiguration GenerateMetricsConfiguration()
        {
            var metricsConfiguration = new Faker<MetricsConfiguration>()
                .StrictMode(true)
                .RuleFor(metricConfiguration => metricConfiguration.AbsolutePath, faker => faker.System.DirectoryPath())
                .Generate();
            return metricsConfiguration;
        }

        private static ContainerLogConfiguration GenerateContainerLogConfiguration()
        {
            var containerLogConfiguration = new Faker<ContainerLogConfiguration>()
                .StrictMode(true)
                .RuleFor(containerConfiguration => containerConfiguration.Verbosity, _ => LogLevel.Error)
                .RuleFor(containerConfiguration => containerConfiguration.IsEnabled, faker => faker.Random.Bool())
                .Generate();
            return containerLogConfiguration;
        }

        private static ServerConfiguration GenerateServerConfiguration()
        {
            var serverConfiguration = new Faker<ServerConfiguration>()
                .StrictMode(true)
                .RuleFor(srvConfig => srvConfig.HttpPort, faker => faker.Random.Int())
                .RuleFor(srvConfig => srvConfig.MaxDegreeOfParallelism, faker => faker.Random.Int())
                .Generate();
            return serverConfiguration;
        }
        
        private static AtlassianStatusPageSinkConfiguration GenerateAtlassianStatusPageSinkConfiguration()
        {
            var atlassianStatusPageSinkConfiguration = new Faker<AtlassianStatusPageSinkConfiguration>()
                .StrictMode(true)
                .RuleFor(promConfiguration => promConfiguration.PageId, faker => faker.Person.FirstName)
                .RuleFor(promConfiguration => promConfiguration.SystemMetricMapping, _ => new List<SystemMetricMapping> {  GenerateAtlassianStatuspageSystemMetricMapping() })
                .Generate();
            return atlassianStatusPageSinkConfiguration;
        }

        private static SystemMetricMapping GenerateAtlassianStatuspageSystemMetricMapping()
        {
            var systemMetricMapping = new Faker<SystemMetricMapping>()
                .StrictMode(true)
                .RuleFor(promConfiguration => promConfiguration.Id, faker => faker.Person.FirstName)
                .RuleFor(promConfiguration => promConfiguration.PromitorMetricName, faker => faker.Person.FirstName)
                .Generate();
            return systemMetricMapping;
        }
        private static OpenTelemetryCollectorSinkConfiguration GenerateOpenTelemetryCollectorSinkConfiguration()
        {
            var openTelemetryCollectorSinkConfiguration = new Faker<OpenTelemetryCollectorSinkConfiguration>()
                .StrictMode(true)
                .RuleFor(promConfiguration => promConfiguration.CollectorUri, faker => faker.Internet.Url())
                .Generate();
            return openTelemetryCollectorSinkConfiguration;
        }

        private static PrometheusScrapingEndpointSinkConfiguration GeneratePrometheusScrapingEndpointSinkConfiguration()
        {
            var prometheusLabelConfiguration = new Faker<LabelConfiguration>()
                .StrictMode(true)
                .RuleFor(labelConfiguration => labelConfiguration.Transformation, faker => faker.PickRandom<LabelTransformation>())
                .Generate();
            var prometheusScrapingEndpointSinkConfiguration = new Faker<PrometheusScrapingEndpointSinkConfiguration>()
                .StrictMode(true)
                .RuleFor(promConfiguration => promConfiguration.BaseUriPath, faker => faker.System.DirectoryPath())
                .RuleFor(promConfiguration => promConfiguration.MetricUnavailableValue, faker => faker.Random.Double(min: 1))
                .RuleFor(promConfiguration => promConfiguration.EnableMetricTimestamps, faker => faker.Random.Bool())
                .RuleFor(promConfiguration => promConfiguration.Labels, _ => prometheusLabelConfiguration)
                .Generate();
            return prometheusScrapingEndpointSinkConfiguration;
        }
    }
}