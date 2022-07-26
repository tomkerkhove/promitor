﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Promitor.Parsers.Prometheus.Core.Models;
using Promitor.Parsers.Prometheus.Core.Models.Interfaces;

namespace Promitor.Tests.Integration.Clients
{
    public class PrometheusClient
    {
        protected IConfiguration Configuration { get; }
        protected HttpClient HttpClient { get; }
        protected string ScrapeUri { get; }
        protected ILogger Logger { get; }

        public PrometheusClient(string baseUrlConfigKey, string scrapeUriConfigKey, IConfiguration configuration, ILogger logger)
        {
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(logger, nameof(logger));

            var baseUrl = configuration[baseUrlConfigKey];

            var configuredScrapingUri = configuration[scrapeUriConfigKey];
            ScrapeUri = string.IsNullOrWhiteSpace(configuredScrapingUri)? "/scrape" : $"/{configuration[scrapeUriConfigKey]}";

            logger.LogInformation("Base URL for Prometheus interaction is '{Url}' to scrape on '{ScrapeUri}'", baseUrl, ScrapeUri);

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            Configuration = configuration;
            Logger = logger;
        }

        public async Task<HttpResponseMessage> ScrapeWithResponseAsync()
        {
            return await GetAsync(ScrapeUri);
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
            var pollPolicy = Policy.HandleResult<List<IMetric>>(metrics => metrics?.Find(x => filter((Gauge)x)) == null)
                                   .WaitAndRetryAsync(5,
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
                Logger.LogInformation("No matching gauge was found");
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

            var stopwatch = Stopwatch.StartNew();
            var response = await HttpClient.SendAsync(request);
            stopwatch.Stop();

            var context = new Dictionary<string, object>();
            try
            {
                await response.Content.ReadAsStringAsync();
                // TODO: Uncomment for full payload during troubleshooting
                //context.Add("Body", rawResponse);
            }
            finally
            {
                Logger.LogRequest(request, response, stopwatch.Elapsed, context);
            }

            return response;
        }
    }
}