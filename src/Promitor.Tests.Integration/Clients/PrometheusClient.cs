using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Arcus.Observability.Telemetry.Core;
using GuardNet;
using Microsoft.Extensions.Logging;
using Polly;
using Promitor.Parsers.Prometheus.Core.Models;
using Promitor.Parsers.Prometheus.Core.Models.Interfaces;

namespace Promitor.Tests.Integration.Clients
{
    public class PrometheusClient
    {
        protected HttpClient HttpClient { get; }
        protected string MetricNamespace { get; }
        protected string ScrapeUri { get; }
        protected ILogger Logger { get; }
        
        public PrometheusClient(string baseUrl, string scrapingUri, ILogger logger)
            : this(baseUrl, scrapingUri, metricNamespace: null, logger: logger)
        {
        }

        public PrometheusClient(string baseUrl, string scrapeUri, string metricNamespace, ILogger logger)
        {
            Guard.NotNullOrWhitespace(baseUrl, nameof(baseUrl));
            Guard.NotNullOrWhitespace(scrapeUri, nameof(scrapeUri));
            Guard.NotNull(logger, nameof(logger));

            MetricNamespace = metricNamespace;
            ScrapeUri = scrapeUri;
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            Logger = logger;

            Logger.LogInformation("Base URL for Prometheus interaction is '{Url}' to scrape on '{ScrapeUri}' with metric namespace '{MetricNamespace}'", baseUrl, ScrapeUri, metricNamespace);
        }

        public async Task<HttpResponseMessage> ScrapeWithResponseAsync()
        {
            return await GetAsync(ScrapeUri);
        }

        public async Task<Gauge> WaitForPrometheusMetricAsync(string expectedMetricName)
        {
            var computedExpectedMetricName = string.IsNullOrWhiteSpace(MetricNamespace) ? expectedMetricName : $"{MetricNamespace}_{expectedMetricName}";
            Logger.LogInformation($"Starting to look for metric with name '{computedExpectedMetricName}'.");
            return await WaitForPrometheusMetricAsync(x => x.Name.Equals(computedExpectedMetricName, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Gauge> WaitForPrometheusMetricAsync(string expectedMetricName, string expectedLabelName, string expectedLabelValue)
        {
            var computedExpectedMetricName = string.IsNullOrWhiteSpace(MetricNamespace) ? expectedMetricName : $"{MetricNamespace}_{expectedMetricName}";

            Func<KeyValuePair<string, string>, bool> labelFilter = label => label.Key.Equals(expectedLabelName, StringComparison.InvariantCultureIgnoreCase)
                                                                            && label.Value.Equals(expectedLabelValue, StringComparison.InvariantCultureIgnoreCase);
            return await WaitForPrometheusMetricAsync(x => x.Name.Equals(computedExpectedMetricName, StringComparison.InvariantCultureIgnoreCase)
                                                           && x.Measurements?.Any(measurement => measurement.Labels?.Any(labelFilter) == true) == true);
        }

        private async Task<Gauge> WaitForPrometheusMetricAsync(Predicate<Gauge> filter)
        {
            // Create retry to poll for metric to show up
            const int maxRetries = 10;
            var pollPolicy = Policy.HandleResult<List<IMetric>>(metrics => metrics?.Find(x => filter((Gauge)x)) == null)
                                   .WaitAndRetryAsync(maxRetries,
                                                  retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                                  (_, _, retryCount, _) =>
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
            var matchingMetric = foundMetrics.Find(x => filter((Gauge)x));
            var gauge = (Gauge)matchingMetric;

            if (gauge == null)
            {
                Logger.LogInformation("No matching gauge was found.");
                if (foundMetrics.Any())
                {
                    Logger.LogInformation($"Found metrics are: {string.Join(", ", foundMetrics.Select(x => x.Name))}");
                }
            }
            else
            {
                Logger.LogInformation("Found matching gauge {Name} with {Measurements} measurements", gauge.Name, gauge.Measurements.Count);
            }

            return gauge;
        }

        protected async Task<HttpResponseMessage> GetAsync(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await HttpClient.SendAsync(request);
            
            using var durationMeasurement = DurationMeasurement.Start();
            var context = new Dictionary<string, object>();
            try
            {
                await response.Content.ReadAsStringAsync();
                // TODO: Uncomment for full payload during troubleshooting
                //context.Add("Body", rawResponse);
            }
            finally
            {
                Logger.LogRequest(request, response, durationMeasurement, context);
            }

            return response;
        }
    }
}