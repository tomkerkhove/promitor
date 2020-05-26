using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Arcus.Observability.Telemetry.Core;
using GuardNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Promitor.Agents.Core.Serialization;
using Promitor.Agents.Scraper.Configuration;

namespace Promitor.Agents.Scraper.Discovery
{
    public class ResourceDiscoveryClient
    {
        private readonly IOptionsMonitor<ResourceDiscoveryConfiguration> _configuration;
        private readonly ILogger<ResourceDiscoveryClient> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ResourceDiscoveryClient(IHttpClientFactory httpClientFactory, IOptionsMonitor<ResourceDiscoveryConfiguration> configuration, ILogger<ResourceDiscoveryClient> logger)
        {
            Guard.NotNull(httpClientFactory, nameof(httpClientFactory));
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(logger, nameof(logger));
            Guard.For<Exception>(() => configuration.CurrentValue.IsConfigured == false, "Resource Discovery is not configured");
            
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<object>> GetAsync(string resourceCollectionName)
        {
            var uri = $"/api/v1/resources/collections/{resourceCollectionName}/discovery";
            var rawResponse = await SendGetRequestAsync(uri);
            var foundResources = JsonConvert.DeserializeObject<List<object>>(rawResponse);
            return foundResources;
        }

        public async Task<HealthReport> GetHealthAsync()
        {
            var rawResponse = await SendGetRequestAsync("/api/v1/health");
            var healthReport = JsonConvert.DeserializeObject<HealthReport>(rawResponse, new HealthReportEntryConverter());
            return healthReport;
        }

        private async Task<string> SendGetRequestAsync(string uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await SendRequestToApiAsync(request);
            response.EnsureSuccessStatusCode();

            var rawResponse = await response.Content.ReadAsStringAsync();
            return rawResponse;
        }

        private async Task<HttpResponseMessage> SendRequestToApiAsync(HttpRequestMessage request)
        {
            var client = CreateHttpClient();
            using (var dependencyMeasurement = DependencyMeasurement.Start())
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await client.SendAsync(request);
                    _logger.LogRequest(request, response, dependencyMeasurement.Elapsed);

                    return response;
                }
                finally
                {
                    var statusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
                    _logger.LogHttpDependency(request, statusCode, dependencyMeasurement);
                }
            }
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient("Promitor Resource Discovery");
            httpClient.BaseAddress = new Uri($"http://{_configuration.CurrentValue.Host}:{_configuration.CurrentValue.Port}");
            return httpClient;
        }
    }
}