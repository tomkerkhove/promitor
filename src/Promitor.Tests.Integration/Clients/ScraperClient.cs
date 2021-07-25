using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Polly;
using Promitor.Parsers.Prometheus.Core.Models;
using Promitor.Parsers.Prometheus.Core.Models.Interfaces;

namespace Promitor.Tests.Integration.Clients
{
    public class ScraperClient : AgentClient
    {
        public ScraperClient(IConfiguration configuration, ILogger logger)
            : base("Scraper", "Agents:Scraper:BaseUrl", configuration, logger)
        {
        }

        public async Task<HttpResponseMessage> GetRuntimeConfigurationWithResponseAsync()
        {
            return await GetAsync("/api/v1/configuration/runtime");
        }

        public async Task<HttpResponseMessage> GetMetricDeclarationWithResponseAsync()
        {
            return await GetAsync("/api/v1/configuration/metric-declaration");
        }

        public async Task<List<MetricDefinition>> GetMetricDeclarationAsync()
        {
            var response = await GetMetricDeclarationWithResponseAsync();
            var rawResponse = await response.Content.ReadAsStringAsync();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            };
            return JsonConvert.DeserializeObject<List<MetricDefinition>>(rawResponse, jsonSerializerSettings);
        }

        public async Task<HttpResponseMessage> ScrapeWithResponseAsync()
        {
            var scrapeUri = Configuration["Agents:Scraper:Prometheus:ScrapeUri"];
            return await GetAsync($"/{scrapeUri}");
        }

        public async Task<Gauge> WaitForPrometheusMetricAsync(string expectedMetricName)
        {
            return await WaitForPrometheusMetricAsync(x => x.Name.Equals(expectedMetricName, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Gauge> WaitForPrometheusMetricAsync(string expectedMetricName, string expectedLabelName, string expectedLabelValue)
        {
            Func<KeyValuePair<string, string>, bool> labelFilter = label => label.Key.Equals(expectedLabelName, StringComparison.InvariantCultureIgnoreCase)
                                                                            && label.Value.Equals(expectedLabelValue, StringComparison.InvariantCultureIgnoreCase);
            return await WaitForPrometheusMetricAsync(x => x.Name.Equals(expectedMetricName, StringComparison.InvariantCultureIgnoreCase)
                                                           && x.Measurements?.Any(measurement => measurement.Labels?.Any(labelFilter) == true) == true);
        }

        private async Task<Gauge> WaitForPrometheusMetricAsync(Predicate<Gauge> filter)
        {
            // Create retry to poll for metric to show up
            const int maxRetries = 5;
            var pollPolicy = Policy.HandleResult<List<IMetric>>(metrics => metrics?.Find(x => filter((Gauge) x)) == null)
                                   .WaitAndRetryAsync(5,
                                                  retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                                  (exception, timeSpan, retryCount, context) =>
                                                  {
                                                      Logger.LogInformation($"Metric was not found, retrying ({retryCount}/{maxRetries}).");
                                                  });

            // Poll
            var foundMetrics = await pollPolicy.ExecuteAsync(async () =>
            {
                var scrapeResponse = await ScrapeWithResponseAsync();
                return await scrapeResponse.ReadAsPrometheusMetricsAsync();
            });

            // Interpret results
            var matchingMetric = foundMetrics.Find(x => filter((Gauge) x));
            return (Gauge) matchingMetric;
        }
    }
}