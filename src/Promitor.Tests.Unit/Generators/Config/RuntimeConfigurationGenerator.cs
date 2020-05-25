using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Agents.Core.Configuration.Telemetry.Sinks;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Statsd.Configuration;

namespace Promitor.Tests.Unit.Generators.Config
{
    internal class RuntimeConfigurationGenerator
    {
        private readonly ScraperRuntimeConfiguration _runtimeConfiguration = new ScraperRuntimeConfiguration();
        private bool _isEnableMetricTimestampsInPrometheusSpecified;

        private RuntimeConfigurationGenerator(ServerConfiguration serverConfiguration)
        {
            _runtimeConfiguration.Server = serverConfiguration;
        }

        private RuntimeConfigurationGenerator(ScraperRuntimeConfiguration runtimeConfiguration)
        {
            _runtimeConfiguration = runtimeConfiguration;
            _isEnableMetricTimestampsInPrometheusSpecified = true;
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

        public static RuntimeConfigurationGenerator WithRuntimeConfiguration(ScraperRuntimeConfiguration runtimeConfiguration)
        {
            return new RuntimeConfigurationGenerator(runtimeConfiguration);
        }

        public RuntimeConfigurationGenerator WithPrometheusMetricSink(double? metricUnavailableValue = -1, bool? enableMetricsTimestamp = false, string scrapeEndpointBaseUri = "/scrape-endpoint")
        {
            PrometheusScrapingEndpointSinkConfiguration prometheusSinkConfiguration;
            if (string.IsNullOrWhiteSpace(scrapeEndpointBaseUri) && metricUnavailableValue == null)
            {
                prometheusSinkConfiguration = null;
            }
            else
            {
                prometheusSinkConfiguration = new PrometheusScrapingEndpointSinkConfiguration();

                if (string.IsNullOrWhiteSpace(scrapeEndpointBaseUri) == false)
                {
                    prometheusSinkConfiguration.BaseUriPath = scrapeEndpointBaseUri;
                }

                if (metricUnavailableValue != null)
                {
                    prometheusSinkConfiguration.MetricUnavailableValue = (double) metricUnavailableValue;
                }

                if (enableMetricsTimestamp != null)
                {
                    prometheusSinkConfiguration.EnableMetricTimestamps = (bool) enableMetricsTimestamp;
                    _isEnableMetricTimestampsInPrometheusSpecified = true;
                }
            }

            _runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint = prometheusSinkConfiguration;

            return this;
        }

        public RuntimeConfigurationGenerator WithStatsDMetricSink(int? port = 1234, string host = "automated-test.host", string metricPrefix = "test.")
        {
            StatsdSinkConfiguration statsdSinkConfiguration;
            if (string.IsNullOrWhiteSpace(host) && port == null)
            {
                statsdSinkConfiguration = null;
            }
            else
            {
                statsdSinkConfiguration = new StatsdSinkConfiguration();

                if (string.IsNullOrWhiteSpace(host) == false)
                {
                    statsdSinkConfiguration.Host = host;
                }

                if (string.IsNullOrWhiteSpace(metricPrefix) == false)
                {
                    statsdSinkConfiguration.MetricPrefix = metricPrefix;
                }

                if (port != null)
                {
                    statsdSinkConfiguration.Port = port.Value;
                }
            }

            _runtimeConfiguration.MetricSinks.Statsd = statsdSinkConfiguration;

            return this;
        }

        public RuntimeConfigurationGenerator WithResourceDiscovery(int? port = 1234, string host = "promitor.resource.discovery.host")
        {
            ResourceDiscoveryConfiguration resourceDiscoveryConfiguration;
            if (string.IsNullOrWhiteSpace(host) && port == null)
            {
                resourceDiscoveryConfiguration = null;
            }
            else
            {
                resourceDiscoveryConfiguration = new ResourceDiscoveryConfiguration();

                if (string.IsNullOrWhiteSpace(host) == false)
                {
                    resourceDiscoveryConfiguration.Host = host;
                }

                if (port != null)
                {
                    resourceDiscoveryConfiguration.Port = port.Value;
                }
            }

            _runtimeConfiguration.ResourceDiscovery = resourceDiscoveryConfiguration;

            return this;
        }

        public RuntimeConfigurationGenerator WithPrometheusLegacyConfiguration(double? metricUnavailableValue = -1, bool? enableMetricsTimestamp = false, string scrapeEndpointBaseUri = "/scrape-endpoint")
        {
            PrometheusLegacyConfiguration prometheusLegacyConfiguration;
            if (string.IsNullOrWhiteSpace(scrapeEndpointBaseUri) && metricUnavailableValue == null)
            {
                prometheusLegacyConfiguration = null;
            }
            else
            {
                prometheusLegacyConfiguration = new PrometheusLegacyConfiguration();

                if (string.IsNullOrWhiteSpace(scrapeEndpointBaseUri) == false)
                {
                    prometheusLegacyConfiguration.ScrapeEndpoint = new ScrapeEndpointConfiguration
                    {
                        BaseUriPath = scrapeEndpointBaseUri
                    };
                }

                if (metricUnavailableValue != null)
                {
                    prometheusLegacyConfiguration.MetricUnavailableValue = (double) metricUnavailableValue;
                }

                if (enableMetricsTimestamp != null)
                {
                    prometheusLegacyConfiguration.EnableMetricTimestamps = (bool) enableMetricsTimestamp;
                    _isEnableMetricTimestampsInPrometheusSpecified = true;
                }
            }

            _runtimeConfiguration.Prometheus = prometheusLegacyConfiguration;

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

            if (_runtimeConfiguration?.ResourceDiscovery != null)
            {
                configurationBuilder.AppendLine("resourceDiscovery:");
                configurationBuilder.AppendLine($"  host: {_runtimeConfiguration?.ResourceDiscovery.Host}");
                configurationBuilder.AppendLine($"  port: {_runtimeConfiguration?.ResourceDiscovery.Port}");
            }

            if (_runtimeConfiguration?.Prometheus != null)
            {
                configurationBuilder.AppendLine("prometheus:");
                if (_runtimeConfiguration?.Prometheus.ScrapeEndpoint != null)
                {
                    configurationBuilder.AppendLine("  scrapeEndpoint:");
                    configurationBuilder.AppendLine($"    baseUriPath: {_runtimeConfiguration?.Prometheus.ScrapeEndpoint.BaseUriPath}");
                }

                if (_isEnableMetricTimestampsInPrometheusSpecified)
                {
                    configurationBuilder.AppendLine($"  enableMetricTimestamps: {_runtimeConfiguration.Prometheus.EnableMetricTimestamps}");
                }

                if (_runtimeConfiguration?.Prometheus.MetricUnavailableValue != null)
                {
                    configurationBuilder.AppendLine($"  metricUnavailableValue: {_runtimeConfiguration?.Prometheus.MetricUnavailableValue}");
                }
            }

            if (_runtimeConfiguration?.MetricSinks != null)
            {
                configurationBuilder.AppendLine("metricSinks:");
                if (_runtimeConfiguration?.MetricSinks.Statsd != null)
                {
                    configurationBuilder.AppendLine("  statsD:");
                    configurationBuilder.AppendLine($"    host: {_runtimeConfiguration?.MetricSinks.Statsd.Host}");
                    configurationBuilder.AppendLine($"    port: {_runtimeConfiguration?.MetricSinks.Statsd.Port}");
                    configurationBuilder.AppendLine($"    metricPrefix: {_runtimeConfiguration?.MetricSinks.Statsd.MetricPrefix}");
                }
                if (_runtimeConfiguration?.MetricSinks.PrometheusScrapingEndpoint != null)
                {
                    configurationBuilder.AppendLine("  prometheusScrapingEndpoint:");
                    configurationBuilder.AppendLine($"    baseUriPath: {_runtimeConfiguration?.MetricSinks.PrometheusScrapingEndpoint.BaseUriPath}");
                    configurationBuilder.AppendLine($"    enableMetricTimestamps: {_runtimeConfiguration?.MetricSinks.PrometheusScrapingEndpoint.EnableMetricTimestamps}");
                    configurationBuilder.AppendLine($"    metricUnavailableValue: {_runtimeConfiguration?.MetricSinks.PrometheusScrapingEndpoint.MetricUnavailableValue}");
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