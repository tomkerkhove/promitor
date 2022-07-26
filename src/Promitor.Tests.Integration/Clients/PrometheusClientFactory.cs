using Arcus.Testing.Logging;
using Microsoft.Extensions.Configuration;

namespace Promitor.Tests.Integration.Clients
{
    internal class PrometheusClientFactory
    {
        internal static PrometheusClient CreateForOpenTelemetryCollector(IConfiguration configuration, XunitTestLogger logger)
        {
            var baseUri = configuration["OpenTelemetry:Collector:Uri"];
            var metricNamespace = configuration["OpenTelemetry:Collector:MetricNamespace"];
            return new PrometheusClient(baseUri, "/metrics", metricNamespace, logger);
        }

        internal static PrometheusClient CreateForPrometheusScrapingEndpointInScraperAgent(IConfiguration configuration, XunitTestLogger logger)
        {
            return CreateForPrometheusScrapingEndpoint("Scraper", configuration, logger);
        }

        internal static PrometheusClient CreateForPrometheusScrapingEndpointInResourceDiscoveryAgent(IConfiguration configuration, XunitTestLogger logger)
        {
            return CreateForPrometheusScrapingEndpoint("ResourceDiscovery", configuration, logger);
        }

        private static PrometheusClient CreateForPrometheusScrapingEndpoint(string agentName, IConfiguration configuration, XunitTestLogger logger)
        {
            var baseUri = configuration["Agents:Scraper:BaseUrl"];
            var scrapeUri = configuration[$"Agents:{agentName}:Prometheus:ScrapeUri"];
            return new PrometheusClient(baseUri, scrapeUri, logger);
        }
    }
}
