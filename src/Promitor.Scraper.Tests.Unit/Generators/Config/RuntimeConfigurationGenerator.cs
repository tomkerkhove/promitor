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
        private readonly RuntimeConfiguration runtimeConfiguration = new RuntimeConfiguration();

        private RuntimeConfigurationGenerator(ServerConfiguration serverConfiguration)
        {
            runtimeConfiguration.Server = serverConfiguration;
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

            runtimeConfiguration.Prometheus = prometheusConfiguration;

            return this;
        }

        public async Task<string> GenerateAsync()
        {
            var configurationBuilder = new StringBuilder();

            if (runtimeConfiguration?.Server != null)
            {
                configurationBuilder.AppendLine("server:");
                configurationBuilder.AppendLine($"  httpPort: {runtimeConfiguration?.Server.HttpPort}");
            }

            if (runtimeConfiguration?.Prometheus != null)
            {
                configurationBuilder.AppendLine("prometheus:");
                if (runtimeConfiguration?.Prometheus.ScrapeEndpoint != null)
                {
                    configurationBuilder.AppendLine("  scrapeEndpoint:");
                    configurationBuilder.AppendLine($"    baseUriPath: {runtimeConfiguration?.Prometheus.ScrapeEndpoint.BaseUriPath}");
                }
            }

            if (runtimeConfiguration?.MetricsConfiguration != null)
            {
                configurationBuilder.AppendLine("metricsConfiguration:");
                configurationBuilder.AppendLine($"  absolutePath: {runtimeConfiguration?.MetricsConfiguration.AbsolutePath}");
            }

            if (runtimeConfiguration?.Telemetry != null)
            {
                configurationBuilder.AppendLine("telemetry:");

                if (runtimeConfiguration?.Telemetry?.ApplicationInsights != null)
                {
                    configurationBuilder.AppendLine("  applicationInsights:");
                    configurationBuilder.AppendLine($"    instrumentationKey: {runtimeConfiguration?.Telemetry.ApplicationInsights.InstrumentationKey}");
                    configurationBuilder.AppendLine($"    isEnabled: {runtimeConfiguration?.Telemetry.ApplicationInsights.IsEnabled}");
                }

                if (runtimeConfiguration?.Telemetry.ContainerLogs != null)
                {
                    configurationBuilder.AppendLine("  containerLogs:");
                    configurationBuilder.AppendLine($"    isEnabled: {runtimeConfiguration?.Telemetry.ContainerLogs.IsEnabled}");
                    configurationBuilder.AppendLine($"    verbosity: {runtimeConfiguration?.Telemetry.ContainerLogs.Verbosity}");
                }

                if (runtimeConfiguration?.Telemetry.DefaultVerbosity != null)
                    configurationBuilder.AppendLine($"  defaultVerbosity: {runtimeConfiguration?.Telemetry.DefaultVerbosity}");
            }

            if (runtimeConfiguration?.FeatureFlags != null)
            {
                configurationBuilder.AppendLine("featureFlags:");
                configurationBuilder.AppendLine($"  disableMetricTimestamps: {runtimeConfiguration?.FeatureFlags.DisableMetricTimestamps} # false by default");
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