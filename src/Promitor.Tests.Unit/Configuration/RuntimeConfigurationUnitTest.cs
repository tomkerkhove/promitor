using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Integrations.Sinks.Statsd.Configuration;
using Promitor.Tests.Unit.Generators.Config;
using Xunit;
using DefaultsCore = Promitor.Agents.Core.Configuration.Defaults;

namespace Promitor.Tests.Unit.Configuration
{
    [Category("Unit")]
    public class RuntimeConfigurationUnitTest : UnitTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RuntimeConfiguration_HasConfiguredAzureMonitorLoggingEnabledFlag_UsesConfigured(bool azureMonitorLogsEnabled)
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithAzureMonitorLogging(isEnabled: azureMonitorLogsEnabled)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.AzureMonitor);
            Assert.NotNull(runtimeConfiguration.AzureMonitor.Logging);
            Assert.Equal(azureMonitorLogsEnabled, runtimeConfiguration.AzureMonitor.Logging.IsEnabled);
        }

        [Theory]
        [InlineData(HttpLoggingDelegatingHandler.Level.None)]
        [InlineData(HttpLoggingDelegatingHandler.Level.Basic)]
        [InlineData(HttpLoggingDelegatingHandler.Level.Headers)]
        [InlineData(HttpLoggingDelegatingHandler.Level.Body)]
        [InlineData(HttpLoggingDelegatingHandler.Level.BodyAndHeaders)]
        public async Task RuntimeConfiguration_HasConfiguredAzureMonitorLoggingInformationLevel_UsesConfigured(HttpLoggingDelegatingHandler.Level informationLevel)
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithAzureMonitorLogging(informationLevel: informationLevel)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.AzureMonitor);
            Assert.NotNull(runtimeConfiguration.AzureMonitor.Logging);
            Assert.Equal(informationLevel, runtimeConfiguration.AzureMonitor.Logging.InformationLevel);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoStartingFromInHoursConfiguredForAzureMonitorIntegration_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithAzureMonitorIntegration(startingFromInHours: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.AzureMonitor);
            Assert.NotNull(runtimeConfiguration.AzureMonitor.Integration);
            Assert.NotNull(runtimeConfiguration.AzureMonitor.Integration.History);
            Assert.Equal(12, runtimeConfiguration.AzureMonitor.Integration.History.StartingFromInHours);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasStartingFromInHoursConfiguredForAzureMonitorIntegration_UsesConfigured()
        {
            // Arrange
            var expectedStartingFromInHours = BogusGenerator.Random.Int(min: 1);
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithAzureMonitorIntegration(startingFromInHours: expectedStartingFromInHours)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.AzureMonitor);
            Assert.NotNull(runtimeConfiguration.AzureMonitor.Integration);
            Assert.NotNull(runtimeConfiguration.AzureMonitor.Integration.History);
            Assert.Equal(expectedStartingFromInHours, runtimeConfiguration.AzureMonitor.Integration.History.StartingFromInHours);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RuntimeConfiguration_HasConfiguredContainerLogEnabledFlag_UsesConfigured(bool containerLogsEnabled)
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithContainerTelemetry(isEnabled: containerLogsEnabled)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.NotNull(runtimeConfiguration.Telemetry.ContainerLogs);
            Assert.Equal(containerLogsEnabled, runtimeConfiguration.Telemetry.ContainerLogs.IsEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RuntimeConfiguration_HasConfiguredApplicationInsightsEnabledFlag_UsesConfigured(bool containerLogsEnabled)
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithApplicationInsightsTelemetry(isEnabled: containerLogsEnabled)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.Equal(containerLogsEnabled, runtimeConfiguration.Telemetry.ApplicationInsights.IsEnabled);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredApplicationInsightsInstrumentationKey_UsesConfigured()
        {
            // Arrange
            var instrumentationKey = BogusGenerator.Random.Guid().ToString();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithApplicationInsightsTelemetry(instrumentationKey)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.Equal(instrumentationKey, runtimeConfiguration.Telemetry.ApplicationInsights.InstrumentationKey);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredApplicationInsightsVerbosity_UsesConfigured()
        {
            // Arrange
            var verbosity = LogLevel.Error;
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithApplicationInsightsTelemetry(verbosity: verbosity)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.Equal(verbosity, runtimeConfiguration.Telemetry.ApplicationInsights.Verbosity);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredContainerLogVerbosity_UsesConfigured()
        {
            // Arrange
            var verbosity = LogLevel.Error;
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithContainerTelemetry(verbosity)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ContainerLogs);
            Assert.Equal(verbosity, runtimeConfiguration.Telemetry.ContainerLogs.Verbosity);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredDefaultTelemetryVerbosityConfigured_UsesConfigured()
        {
            // Arrange
            var defaultVerbosity = LogLevel.Error;
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithGeneralTelemetry(defaultVerbosity)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.Equal(defaultVerbosity, runtimeConfiguration.Telemetry.DefaultVerbosity);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredHttpPort_UsesConfigured()
        {
            // Arrange
            var bogusHttpPort = BogusGenerator.Random.Int();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration(bogusHttpPort)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.Equal(bogusHttpPort, runtimeConfiguration.Server.HttpPort);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredMaxDegreeOfParallelism_UsesConfigured()
        {
            // Arrange
            var bogusMaxDegreeOfParallelism = BogusGenerator.Random.Int();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration(maxDegreeOfParallelism: bogusMaxDegreeOfParallelism)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.Equal(bogusMaxDegreeOfParallelism, runtimeConfiguration.Server.MaxDegreeOfParallelism);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RuntimeConfiguration_HasConfiguredEnabledMetricTimestampsInPrometheusScrapeEndpointSink_UsesConfigured(bool enableMetricsTimestamp)
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusMetricSink(enableMetricsTimestamp: enableMetricsTimestamp)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint);
            Assert.Equal(enableMetricsTimestamp, runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.EnableMetricTimestamps);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredMetricUnavailableValueInPrometheusScrapeEndpointSink_UsesConfigured()
        {
            // Arrange
            var metricUnavailableValue = BogusGenerator.Random.Double(min: 1);
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusMetricSink(metricUnavailableValue: metricUnavailableValue)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint);
            Assert.Equal(metricUnavailableValue, runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.MetricUnavailableValue);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredPrometheusScrapeEndpointSinkConfigured_UsesConfigured()
        {
            // Arrange
            var scrapeEndpointBaseUri = BogusGenerator.System.DirectoryPath();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusMetricSink(scrapeEndpointBaseUri: scrapeEndpointBaseUri)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint);
            Assert.Equal(scrapeEndpointBaseUri, runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.BaseUriPath);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredHostInStatsDEndpoint_UsesConfigured()
        {
            // Arrange
            var statsdHost = BogusGenerator.System.DirectoryPath();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithStatsDMetricSink(host: statsdHost)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.Statsd);
            Assert.Equal(statsdHost, runtimeConfiguration.MetricSinks.Statsd.Host);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredMetricPrefixInStatsDEndpoint_UsesConfigured()
        {
            // Arrange
            var metricPrefix = BogusGenerator.Name.FirstName();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithStatsDMetricSink(metricPrefix: metricPrefix)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.Statsd);
            Assert.Equal(metricPrefix, runtimeConfiguration.MetricSinks.Statsd.MetricPrefix);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredPortInStatsDEndpoint_UsesConfigured()
        {
            // Arrange
            var port = BogusGenerator.Random.Int();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithStatsDMetricSink(port: port)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.Statsd);
            Assert.Equal(port, runtimeConfiguration.MetricSinks.Statsd.Port);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredGenevaInStatsDEndpoint_UsesConfigured()
        {
            // Arrange
            var port = BogusGenerator.Random.Int();
            var genevaAccountName = "abc";
            var genevaNamespace = "xyz";

            var geneva = new GenevaConfiguration {
                Account = genevaAccountName,
                Namespace = genevaNamespace
            };

            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithStatsDMetricSink(port: port, genevaMetadata: geneva)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.Statsd);
            Assert.Equal(port, runtimeConfiguration.MetricSinks.Statsd.Port);
            Assert.Equal(genevaAccountName, runtimeConfiguration.MetricSinks.Statsd.Geneva.Account);
            Assert.Equal(genevaNamespace, runtimeConfiguration.MetricSinks.Statsd.Geneva.Namespace);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredCollectorUriInOpenTelemetryCollectorEndpoint_UsesConfigured()
        {
            // Arrange
            var collectorUri = "https://foo.bar";
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithOpenTelemetryCollectorMetricSink(collectorUri: collectorUri)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.OpenTelemetryCollector);
            Assert.Equal(collectorUri, runtimeConfiguration.MetricSinks.OpenTelemetryCollector.CollectorUri);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredNoCollectorUriInOpenTelemetryCollectorEndpoint_OpenTelemetryIsIgnored()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithOpenTelemetryCollectorMetricSink(collectorUri: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.Null(runtimeConfiguration.MetricSinks.OpenTelemetryCollector);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoDefaultApplicationInsights_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithApplicationInsightsTelemetry(verbosity: null, isEnabled: null, instrumentationKey: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.Null(runtimeConfiguration.Telemetry.ApplicationInsights.InstrumentationKey);
            Assert.Equal(DefaultsCore.Telemetry.ApplicationInsights.Verbosity, runtimeConfiguration.Telemetry.ApplicationInsights.Verbosity);
            Assert.Equal(DefaultsCore.Telemetry.ApplicationInsights.IsEnabled, runtimeConfiguration.Telemetry.ApplicationInsights.IsEnabled);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoDefaultContainerLogsConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithContainerTelemetry(null, null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.NotNull(runtimeConfiguration.Telemetry.ContainerLogs);
            Assert.Equal(DefaultsCore.Telemetry.ContainerLogs.Verbosity, runtimeConfiguration.Telemetry.ContainerLogs.Verbosity);
            Assert.Equal(DefaultsCore.Telemetry.ContainerLogs.IsEnabled, runtimeConfiguration.Telemetry.ContainerLogs.IsEnabled);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoDefaultTelemetryVerbosityConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithGeneralTelemetry(null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.Equal(DefaultsCore.Telemetry.DefaultVerbosity, runtimeConfiguration.Telemetry.DefaultVerbosity);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoHttpPort_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithoutServerConfiguration()
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.Equal(DefaultsCore.Server.HttpPort, runtimeConfiguration.Server.HttpPort);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoMaxDegreeOfParallelism_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithoutServerConfiguration()
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.Equal(DefaultsCore.Server.MaxDegreeOfParallelism, runtimeConfiguration.Server.MaxDegreeOfParallelism);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoMetricsConfigurationBasePathConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithMetricsConfiguration(absolutePath: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricsConfiguration);
            Assert.Equal(Defaults.MetricsConfiguration.AbsolutePath, runtimeConfiguration.MetricsConfiguration.AbsolutePath);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoResourceDiscoveryPortConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithResourceDiscovery(port: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.ResourceDiscovery);
            Assert.NotNull(runtimeConfiguration.ResourceDiscovery.Port);
        }

        [Fact]
        public async Task RuntimeConfiguration_IsFullyConfigured_UsesCorrectValues()
        {
            // Arrange
            var bogusRuntimeConfiguration = BogusScraperRuntimeConfigurationGenerator.Generate();
            var configuration = await RuntimeConfigurationGenerator.WithRuntimeConfiguration(bogusRuntimeConfiguration)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<ScraperRuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.NotNull(runtimeConfiguration.MetricSinks);
            Assert.NotNull(runtimeConfiguration.MetricSinks.Statsd);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.NotNull(runtimeConfiguration.Telemetry.ContainerLogs);
            Assert.NotNull(runtimeConfiguration.MetricsConfiguration);
            Assert.NotNull(runtimeConfiguration.ResourceDiscovery);
            Assert.Equal(bogusRuntimeConfiguration.Server.HttpPort, runtimeConfiguration.Server.HttpPort);
            Assert.Equal(bogusRuntimeConfiguration.Server.MaxDegreeOfParallelism, runtimeConfiguration.Server.MaxDegreeOfParallelism);
            Assert.Equal(bogusRuntimeConfiguration.ResourceDiscovery.Host, runtimeConfiguration.ResourceDiscovery.Host);
            Assert.Equal(bogusRuntimeConfiguration.ResourceDiscovery.Port, runtimeConfiguration.ResourceDiscovery.Port);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.DefaultVerbosity, runtimeConfiguration.Telemetry.DefaultVerbosity);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ApplicationInsights.Verbosity, runtimeConfiguration.Telemetry.ApplicationInsights.Verbosity);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ApplicationInsights.InstrumentationKey, runtimeConfiguration.Telemetry.ApplicationInsights.InstrumentationKey);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ApplicationInsights.IsEnabled, runtimeConfiguration.Telemetry.ApplicationInsights.IsEnabled);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ContainerLogs.Verbosity, runtimeConfiguration.Telemetry.ContainerLogs.Verbosity);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ContainerLogs.IsEnabled, runtimeConfiguration.Telemetry.ContainerLogs.IsEnabled);
            Assert.Equal(bogusRuntimeConfiguration.MetricsConfiguration.AbsolutePath, runtimeConfiguration.MetricsConfiguration.AbsolutePath);
            Assert.Equal(bogusRuntimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.BaseUriPath, runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.BaseUriPath);
            Assert.Equal(bogusRuntimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.EnableMetricTimestamps, runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.EnableMetricTimestamps);
            Assert.Equal(bogusRuntimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.MetricUnavailableValue, runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint.MetricUnavailableValue);
            Assert.Equal(bogusRuntimeConfiguration.MetricSinks.Statsd.Host, runtimeConfiguration.MetricSinks.Statsd.Host);
            Assert.Equal(bogusRuntimeConfiguration.MetricSinks.Statsd.Port, runtimeConfiguration.MetricSinks.Statsd.Port);
            Assert.Equal(bogusRuntimeConfiguration.MetricSinks.Statsd.MetricPrefix, runtimeConfiguration.MetricSinks.Statsd.MetricPrefix);
            Assert.Equal(bogusRuntimeConfiguration.MetricSinks.Statsd.MetricFormat, runtimeConfiguration.MetricSinks.Statsd.MetricFormat);
        }
    }
}