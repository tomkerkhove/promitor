using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Promitor.Tests.Integration.Clients
{
    public class PrometheusClientFactory
    {
        private readonly ILogger _logger;

        public PrometheusClientFactory(ILogger logger)
        {
            _logger = logger;
        }

        internal PrometheusClient CreateForOpenTelemetryCollector(IConfiguration configuration)
        {
            var baseUri = configuration["OpenTelemetry:Collector:Uri"];
            var metricNamespace = configuration["OpenTelemetry:Collector:MetricNamespace"];

            _logger.LogInformation("Creating Prometheus client for {BaseUri}/metrics with metric namespace {metricNamespace}", baseUri, metricNamespace);

            return new PrometheusClient(baseUri, "/metrics", metricNamespace, _logger);
        }

        internal PrometheusClient CreateForPrometheusScrapingEndpointInScraperAgent(IConfiguration configuration)
        {
            return CreateForPrometheusScrapingEndpoint("Scraper", configuration);
        }

        internal PrometheusClient CreateForPrometheusScrapingEndpointInResourceDiscoveryAgent(IConfiguration configuration)
        {
            return CreateForPrometheusScrapingEndpoint("ResourceDiscovery", configuration);
        }

        private PrometheusClient CreateForPrometheusScrapingEndpoint(string agentName, IConfiguration configuration)
        {
            var baseUri = configuration[$"Agents:{agentName}:BaseUrl"];
            var scrapeUri = configuration[$"Agents:{agentName}:Prometheus:ScrapeUri"];

            _logger.LogInformation("Creating Prometheus client for {BaseUri}/{ScrapeUri}", baseUri, scrapeUri);
            return new PrometheusClient(baseUri, scrapeUri, _logger);
        }
    }
}
