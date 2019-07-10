using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Promitor.Scraper.Tests.Unit.Generators.Config
{
    internal class RuntimeConfigurationGenerator
    {
        public static RuntimeConfigurationGenerator WithServerConfiguration()
        {
            return new RuntimeConfigurationGenerator();
        }

        public async Task<string> GenerateAsync()
        {
            var rawYaml = @"server:
  httpPort: 88
prometheus:
  scrapeEndpoint:
    baseUriPath: /scrape
metricsConfiguration:
  absolutePath: /config/metrics-declaration.yaml
telemetry:
  applicationInsights:
    instrumentationKey: ABC
    isEnabled: true
  containerLogs:
    isEnabled: true
    verbosity: trace
  defaultVerbosity: warning
featureFlags:
  disableMetricTimestamps: true # false by default";

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