using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Promitor.Parsers.Prometheus.Core.Models;
using Promitor.Parsers.Prometheus.Core.Models.Interfaces;

namespace Promitor.Tests.Integration.Clients
{
    public class ScraperClient : AgentClient
    {
        public ScraperClient(IConfiguration configuration, ILogger logger)
        :base("Scraper", "Agents:Scraper:BaseUrl", configuration,logger)
        {
        }

        public async Task<HttpResponseMessage> GetRuntimeConfigurationAsync()
        {
            return await GetAsync("/api/v1/configuration/runtime");
        }

        public async Task<HttpResponseMessage> GetMetricDeclarationAsync()
        {
            return await GetAsync("/api/v1/configuration/metric-declaration");
        }

        public async Task<HttpResponseMessage> ScrapeAsync()
        {
            var scrapeUri = Configuration["Agents:Scraper:Prometheus:ScrapeUri"];
            return await GetAsync($"/{scrapeUri}");
        }

        public async Task<Gauge> WaitForPrometheusMetricAsync(string expectedMetricName)
        {
            // Create retry to poll for metric to show up
            const int maxRetries = 5;
            var pollPolicy = Policy.HandleResult<List<IMetric>>(metrics => metrics?.Find(metric => metric.Name == expectedMetricName) == null)
                                   .WaitAndRetryAsync(5,
                                                  retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                                  (exception, timeSpan, retryCount, context) =>
                                                  {
                                                      Logger.LogInformation($"Metric {expectedMetricName} was not found, retrying ({retryCount}/{maxRetries}).");
                                                  });

            // Poll
            var foundMetrics = await pollPolicy.ExecuteAsync(async () =>
            {
                var scrapeResponse = await ScrapeAsync();
                return await scrapeResponse.ReadAsPrometheusMetricsAsync();
            });

            // Interpret results
            var matchingMetric = foundMetrics.Find(x => x.Name == expectedMetricName);
            return (Gauge) matchingMetric;
        }
    }
}