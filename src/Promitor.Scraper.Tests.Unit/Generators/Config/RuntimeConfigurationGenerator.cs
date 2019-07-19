using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Promitor.Core.Configuration.Model;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Configuration.Model.Server;

namespace Promitor.Scraper.Tests.Unit.Generators.Config
{
    internal class RuntimeConfigurationGenerator
    {
        private readonly RuntimeConfiguration _runtimeConfiguration = new RuntimeConfiguration();

        private RuntimeConfigurationGenerator(ServerConfiguration serverConfiguration)
        {
            _runtimeConfiguration.Server = serverConfiguration;
        }

        public static RuntimeConfigurationGenerator WithServerConfiguration(int? httpPort = 88)
        {
            var serverConfiguration = httpPort == null
                ? null
                : new ServerConfiguration
                {
                    HttpPort = httpPort.Value
                };

            return new RuntimeConfigurationGenerator(serverConfiguration);
        }

        public RuntimeConfigurationGenerator WithPrometheusConfiguration(string scrapeEndpointBaseUri = "/scrape")
        {
            var prometheusConfiguration = scrapeEndpointBaseUri == null
                ? null
                : new PrometheusConfiguration
                {
                    ScrapeEndpoint = new ScrapeEndpointConfiguration
                    {
                        BaseUriPath = scrapeEndpointBaseUri
                    }
                };

            _runtimeConfiguration.Prometheus = prometheusConfiguration;

            return this;
        }

        public async Task<string> GenerateAsync()
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
            }

            if (_runtimeConfiguration?.MetricsConfiguration != null)
            {
                configurationBuilder.AppendLine("metricsConfiguration:");
                configurationBuilder.AppendLine($"  absolutePath: {_runtimeConfiguration?.MetricsConfiguration.AbsolutePath}");
            }

            if (_runtimeConfiguration?.Telemetry != null)
            {
                configurationBuilder.AppendLine("telemetry:");

                if (_runtimeConfiguration?.Telemetry?.ApplicationInsights != null)
                {
                    configurationBuilder.AppendLine("  applicationInsights:");
                    configurationBuilder.AppendLine($"    instrumentationKey: {_runtimeConfiguration?.Telemetry.ApplicationInsights.InstrumentationKey}");
                    configurationBuilder.AppendLine($"    isEnabled: {_runtimeConfiguration?.Telemetry.ApplicationInsights.IsEnabled}");
                }

                if (_runtimeConfiguration?.Telemetry.ContainerLogs != null)
                {
                    configurationBuilder.AppendLine("  containerLogs:");
                    configurationBuilder.AppendLine($"    isEnabled: {_runtimeConfiguration?.Telemetry.ContainerLogs.IsEnabled}");
                    configurationBuilder.AppendLine($"    verbosity: {_runtimeConfiguration?.Telemetry.ContainerLogs.Verbosity}");
                }

                if (_runtimeConfiguration?.Telemetry.DefaultVerbosity != null)
                    configurationBuilder.AppendLine($"  defaultVerbosity: {_runtimeConfiguration?.Telemetry.DefaultVerbosity}");
            }

            if (_runtimeConfiguration?.FeatureFlags != null)
            {
                configurationBuilder.AppendLine("featureFlags:");
                configurationBuilder.AppendLine($"  disableMetricTimestamps: {_runtimeConfiguration?.FeatureFlags.DisableMetricTimestamps} # false by default");
            }

            var rawYaml = configurationBuilder.ToString();

            var filePath = await PersistConfigurationAsync(rawYaml);
            return filePath;
        }

        private async Task<string> PersistConfigurationAsync(string rawYaml)
        {
            var fileUri = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/{Guid.NewGuid()}.yaml";
            await File.WriteAllTextAsync(fileUri, rawYaml);

            return fileUri;
        }
    }
}