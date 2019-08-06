using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Model;
using Promitor.Core.Configuration.Model.FeatureFlags;
using Promitor.Core.Configuration.Model.Metrics;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Configuration.Model.Server;
using Promitor.Core.Configuration.Model.Telemetry;
using Promitor.Core.Configuration.Model.Telemetry.Sinks;

namespace Promitor.Scraper.Tests.Unit.Generators.Config
{
    internal class RuntimeConfigurationGenerator
    {
        private readonly RuntimeConfiguration _runtimeConfiguration = new RuntimeConfiguration();

        private RuntimeConfigurationGenerator(ServerConfiguration serverConfiguration)
        {
            _runtimeConfiguration.Server = serverConfiguration;
        }

        private RuntimeConfigurationGenerator(RuntimeConfiguration runtimeConfiguration)
        {
            _runtimeConfiguration = runtimeConfiguration;
        }

        public static RuntimeConfigurationGenerator WithServerConfiguration(int? httpPort = 888)
        {
            var serverConfiguration = httpPort == null
                ? null
                : new ServerConfiguration
                {
                    HttpPort = httpPort.Value
                };

            return new RuntimeConfigurationGenerator(serverConfiguration);
        }

        public static RuntimeConfigurationGenerator WithRuntimeConfiguration(RuntimeConfiguration runtimeConfiguration)
        {
            return new RuntimeConfigurationGenerator(runtimeConfiguration);
        }

        public RuntimeConfigurationGenerator WithPrometheusConfiguration(double? metricUnavailableValue = -1, string scrapeEndpointBaseUri = "/scrape-endpoint")
        {
            PrometheusConfiguration prometheusConfiguration;
            if (string.IsNullOrWhiteSpace(scrapeEndpointBaseUri) && metricUnavailableValue == null)
            {
                prometheusConfiguration = null;
            }
            else
            {
                prometheusConfiguration = new PrometheusConfiguration();

                if (string.IsNullOrWhiteSpace(scrapeEndpointBaseUri) == false)
                {
                    prometheusConfiguration.ScrapeEndpoint = new ScrapeEndpointConfiguration
                    {
                        BaseUriPath = scrapeEndpointBaseUri
                    };
                }

                if (metricUnavailableValue != null)
                {
                    prometheusConfiguration.MetricUnavailableValue = (double)metricUnavailableValue;
                }

            }

            _runtimeConfiguration.Prometheus = prometheusConfiguration;

            return this;
        }

        public RuntimeConfigurationGenerator WithMetricsConfiguration(string absolutePath = "/metrics-declaration.yaml")
        {
            var metricsConfiguration = absolutePath == null
                ? null
                : new MetricsConfiguration
                {
                    AbsolutePath = absolutePath
                };

            _runtimeConfiguration.MetricsConfiguration = metricsConfiguration;

            return this;
        }

        public RuntimeConfigurationGenerator WithFeatureFlags(bool? disableMetricTimestamps = true)
        {
            var featureFlags = disableMetricTimestamps == null
                ? null
                : new FeatureFlagsConfiguration()
                {
                    DisableMetricTimestamps = disableMetricTimestamps.GetValueOrDefault()
                };

            _runtimeConfiguration.FeatureFlags = featureFlags;

            return this;
        }

        public RuntimeConfigurationGenerator WithGeneralTelemetry(LogLevel? defaultVerbosity = LogLevel.Trace)
        {
            var telemetryConfiguration = defaultVerbosity == null
                ? null
                : new TelemetryConfiguration()
                {
                    DefaultVerbosity = defaultVerbosity
                };

            _runtimeConfiguration.Telemetry = telemetryConfiguration;

            return this;
        }

        public RuntimeConfigurationGenerator WithContainerTelemetry(LogLevel? verbosity = LogLevel.Trace, bool? isEnabled = true)
        {
            var containerLogConfiguration = verbosity == null && isEnabled == null
                ? null
                : new ContainerLogConfiguration
                {
                    IsEnabled = isEnabled.GetValueOrDefault(),
                    Verbosity = verbosity
                };

            if (_runtimeConfiguration.Telemetry == null)
            {
                _runtimeConfiguration.Telemetry = new TelemetryConfiguration();
            }

            _runtimeConfiguration.Telemetry.ContainerLogs = containerLogConfiguration;

            return this;
        }

        public RuntimeConfigurationGenerator WithApplicationInsightsTelemetry(string instrumentationKey = "XYZ", LogLevel? verbosity = LogLevel.Trace, bool? isEnabled = true)
        {
            var applicationInsightsTelemetry = verbosity == null && isEnabled == null && string.IsNullOrWhiteSpace(instrumentationKey)
                ? null
                : new ApplicationInsightsConfiguration()
                {
                    InstrumentationKey = instrumentationKey,
                    IsEnabled = isEnabled.GetValueOrDefault(),
                    Verbosity = verbosity
                };

            if (_runtimeConfiguration.Telemetry == null)
            {
                _runtimeConfiguration.Telemetry = new TelemetryConfiguration();
            }

            _runtimeConfiguration.Telemetry.ApplicationInsights = applicationInsightsTelemetry;

            return this;
        }

        public async Task<IConfiguration> GenerateAsync()
        {
            var configurationBuilder = new StringBuilder();

            if (_runtimeConfiguration?.Server != null)
            {
                configurationBuilder.AppendLine("server:");
                configurationBuilder.AppendLine($"  httpPort: {_runtimeConfiguration?.Server.HttpPort}");
            }

            if (_runtimeConfiguration?.Prometheus != null)
            {
                configurationBuilder.AppendLine("prometheus:");
                if (_runtimeConfiguration?.Prometheus.ScrapeEndpoint != null)
                {
                    configurationBuilder.AppendLine("  scrapeEndpoint:");
                    configurationBuilder.AppendLine($"    baseUriPath: {_runtimeConfiguration?.Prometheus.ScrapeEndpoint.BaseUriPath}");
                }

                if (_runtimeConfiguration?.Prometheus.MetricUnavailableValue != null)
                {
                    configurationBuilder.AppendLine($"  metricUnavailableValue: {_runtimeConfiguration?.Prometheus.MetricUnavailableValue}");
                }
            }

            if (_runtimeConfiguration?.MetricsConfiguration != null)
            {
                configurationBuilder.AppendLine("metricsConfiguration:");
                if (_runtimeConfiguration?.MetricsConfiguration.AbsolutePath != null)
                {
                    configurationBuilder.AppendLine($"  absolutePath: {_runtimeConfiguration?.MetricsConfiguration.AbsolutePath}");
                }
            }

            if (_runtimeConfiguration?.Telemetry != null)
            {
                configurationBuilder.AppendLine("telemetry:");

                if (_runtimeConfiguration?.Telemetry?.ApplicationInsights != null)
                {
                    configurationBuilder.AppendLine("  applicationInsights:");
                    configurationBuilder.AppendLine($"    instrumentationKey: {_runtimeConfiguration?.Telemetry.ApplicationInsights.InstrumentationKey}");
                    configurationBuilder.AppendLine($"    isEnabled: {_runtimeConfiguration?.Telemetry.ApplicationInsights.IsEnabled}");
                    configurationBuilder.AppendLine($"    verbosity: {_runtimeConfiguration?.Telemetry.ApplicationInsights.Verbosity}");
                }

                if (_runtimeConfiguration?.Telemetry.ContainerLogs != null)
                {
                    configurationBuilder.AppendLine("  containerLogs:");
                    configurationBuilder.AppendLine($"    isEnabled: {_runtimeConfiguration?.Telemetry.ContainerLogs.IsEnabled}");
                    configurationBuilder.AppendLine($"    verbosity: {_runtimeConfiguration?.Telemetry.ContainerLogs.Verbosity}");
                }

                if (_runtimeConfiguration?.Telemetry.DefaultVerbosity != null)
                {
                    configurationBuilder.AppendLine($"  defaultVerbosity: {_runtimeConfiguration?.Telemetry.DefaultVerbosity}");
                }
            }

            if (_runtimeConfiguration?.FeatureFlags != null)
            {
                configurationBuilder.AppendLine("featureFlags:");
                configurationBuilder.AppendLine($"  disableMetricTimestamps: {_runtimeConfiguration?.FeatureFlags.DisableMetricTimestamps} # false by default");
            }

            var rawYaml = configurationBuilder.ToString();

            var filePath = await PersistConfigurationAsync(rawYaml);

            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(filePath))
                .AddYamlFile(Path.GetFileName(filePath))
                .Build();
        }

        private async Task<string> PersistConfigurationAsync(string rawYaml)
        {
            var fileUri = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/{Guid.NewGuid()}.yaml";
            await File.WriteAllTextAsync(fileUri, rawYaml);

            return fileUri;
        }
    }
}