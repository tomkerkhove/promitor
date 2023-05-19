using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Agents.Core.Configuration.Telemetry.Sinks;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Core.Scraping.Configuration.Runtime;
using Promitor.Integrations.AzureMonitor.Configuration;
using Promitor.Integrations.Sinks.OpenTelemetry.Configuration;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Statsd.Configuration;

namespace Promitor.Tests.Unit.Generators.Config
{
    internal class RuntimeConfigurationGenerator
    {
        private readonly ScraperRuntimeConfiguration _runtimeConfiguration = new();

        private RuntimeConfigurationGenerator(ServerConfiguration serverConfiguration)
        {
            _runtimeConfiguration.Server = serverConfiguration;
        }

        private RuntimeConfigurationGenerator(ScraperRuntimeConfiguration runtimeConfiguration)
        {
            _runtimeConfiguration = runtimeConfiguration;
        }

        public static RuntimeConfigurationGenerator WithServerConfiguration(int httpPort = 888, int maxDegreeOfParallelism = 8)
        {
            var serverConfiguration = new ServerConfiguration
            {
                HttpPort = httpPort,
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            return new RuntimeConfigurationGenerator(serverConfiguration);
        }

        public static RuntimeConfigurationGenerator WithoutServerConfiguration()
        {
            return new RuntimeConfigurationGenerator(new ScraperRuntimeConfiguration());
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
                }
            }

            _runtimeConfiguration.MetricSinks.PrometheusScrapingEndpoint = prometheusSinkConfiguration;

            return this;
        }

        public RuntimeConfigurationGenerator WithOpenTelemetryCollectorMetricSink(string collectorUri = "https://opentelemetry-collector:8888")
        {
            if (string.IsNullOrWhiteSpace(collectorUri) == false)
            {
                _runtimeConfiguration.MetricSinks.OpenTelemetryCollector = new OpenTelemetryCollectorSinkConfiguration
                {
                    CollectorUri = collectorUri
                };
            }

            return this;
        }

        public RuntimeConfigurationGenerator WithStatsDMetricSink(int? port = 1234, string host = "automated-test.host", string metricPrefix = "test.", GenevaConfiguration genevaMetadata = null)
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

                if (genevaMetadata != null)
                {
                    statsdSinkConfiguration.Geneva = genevaMetadata;
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

            _runtimeConfiguration.Telemetry ??= new TelemetryConfiguration();
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

            _runtimeConfiguration.Telemetry ??= new TelemetryConfiguration();
            _runtimeConfiguration.Telemetry.ApplicationInsights = applicationInsightsTelemetry;

            return this;
        }

        public RuntimeConfigurationGenerator WithAzureMonitorLogging(bool isEnabled = true, HttpLoggingDelegatingHandler.Level informationLevel = HttpLoggingDelegatingHandler.Level.Headers)
        {
            _runtimeConfiguration.AzureMonitor ??= new AzureMonitorConfiguration();
            _runtimeConfiguration.AzureMonitor.Logging = new AzureMonitorLoggingConfiguration
            {
                IsEnabled = isEnabled,
                InformationLevel = informationLevel
            };

            return this;
        }

        public RuntimeConfigurationGenerator WithAzureMonitorIntegration(int? startingFromInHours = 100)
        {
            _runtimeConfiguration.AzureMonitor ??= new AzureMonitorConfiguration();
            _runtimeConfiguration.AzureMonitor.Integration ??= new AzureMonitorIntegrationConfiguration();

            _runtimeConfiguration.AzureMonitor.Integration.History = new AzureMonitorHistoryConfiguration();

            if (startingFromInHours != null)
            {
                _runtimeConfiguration.AzureMonitor.Integration.History.StartingFromInHours = startingFromInHours.Value;
            }

            return this;
        }

        public async Task<IConfiguration> GenerateAsync()
        {
            var configurationBuilder = new StringBuilder();

            if (_runtimeConfiguration?.Server != null)
            {
                configurationBuilder.AppendLine("server:");
                configurationBuilder.AppendLine($"  httpPort: {_runtimeConfiguration?.Server.HttpPort}");
                configurationBuilder.AppendLine($"  maxDegreeOfParallelism: {_runtimeConfiguration?.Server.MaxDegreeOfParallelism}");
            }

            if (_runtimeConfiguration?.ResourceDiscovery != null)
            {
                configurationBuilder.AppendLine("resourceDiscovery:");
                configurationBuilder.AppendLine($"  host: {_runtimeConfiguration?.ResourceDiscovery.Host}");
                configurationBuilder.AppendLine($"  port: {_runtimeConfiguration?.ResourceDiscovery.Port}");
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
                    configurationBuilder.AppendLine($"    metricFormat: {_runtimeConfiguration?.MetricSinks.Statsd.MetricFormat}");
                    if (_runtimeConfiguration?.MetricSinks.Statsd.Geneva != null)
                    {
                        configurationBuilder.AppendLine("    geneva:");
                        configurationBuilder.AppendLine($"      account: {_runtimeConfiguration?.MetricSinks.Statsd.Geneva.Account}");
                        configurationBuilder.AppendLine($"      namespace: {_runtimeConfiguration?.MetricSinks.Statsd.Geneva.Namespace}");
                    }
                }               
                if (_runtimeConfiguration?.MetricSinks.PrometheusScrapingEndpoint != null)
                {
                    configurationBuilder.AppendLine("  prometheusScrapingEndpoint:");
                    configurationBuilder.AppendLine($"    baseUriPath: {_runtimeConfiguration?.MetricSinks.PrometheusScrapingEndpoint.BaseUriPath}");
                    configurationBuilder.AppendLine($"    enableMetricTimestamps: {_runtimeConfiguration?.MetricSinks.PrometheusScrapingEndpoint.EnableMetricTimestamps}");
                    configurationBuilder.AppendLine($"    metricUnavailableValue: {_runtimeConfiguration?.MetricSinks.PrometheusScrapingEndpoint.MetricUnavailableValue}");
                }
                if (_runtimeConfiguration?.MetricSinks.OpenTelemetryCollector != null)
                {
                    configurationBuilder.AppendLine("  openTelemetryCollector:");
                    configurationBuilder.AppendLine($"    collectorUri: {_runtimeConfiguration?.MetricSinks.OpenTelemetryCollector.CollectorUri}");
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

            if (_runtimeConfiguration?.AzureMonitor != null)
            {
                configurationBuilder.AppendLine("azureMonitor:");

                if (_runtimeConfiguration?.AzureMonitor.Integration?.History != null)
                {
                    configurationBuilder.AppendLine("  integration:");
                    configurationBuilder.AppendLine("    history:");
                    configurationBuilder.AppendLine($"      startingFromInHours: {_runtimeConfiguration?.AzureMonitor.Integration.History.StartingFromInHours}");
                }

                if (_runtimeConfiguration?.AzureMonitor.Logging != null)
                {
                    configurationBuilder.AppendLine("  logging:");
                    configurationBuilder.AppendLine($"    isEnabled: {_runtimeConfiguration?.AzureMonitor.Logging.IsEnabled}");
                    configurationBuilder.AppendLine($"    informationLevel: {_runtimeConfiguration?.AzureMonitor.Logging.InformationLevel}");
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