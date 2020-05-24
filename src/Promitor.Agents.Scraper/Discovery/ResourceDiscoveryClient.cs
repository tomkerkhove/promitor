using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Arcus.Observability.Telemetry.Core;
using GuardNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Promitor.Agents.Core.Serialization;

namespace Promitor.Agents.Scraper.Discovery
{
    public class ResourceDiscoveryClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ResourceDiscoveryClient> _logger;

        public ResourceDiscoveryClient(IHttpClientFactory httpClientFactory, ILogger<ResourceDiscoveryClient> logger)
        {
            Guard.NotNull(httpClientFactory, nameof(httpClientFactory));
            Guard.NotNull(httpClientFactory, nameof(httpClientFactory));

            _logger = logger;
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
            var client = _httpClientFactory.CreateClient("Promitor Resource Discovery");
            using (var dependencyMeasurement = DependencyMeasurement.Start())
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await client.SendAsync(request);
                    return response;
                }
                finally
                {
                    var statusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
                    _logger.LogHttpDependency(request, statusCode, dependencyMeasurement);
                }
            }
        }
    }
}