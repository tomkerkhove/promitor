using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Arcus.Observability.Telemetry.Core;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Promitor.Agents.Core.Contracts;
using Promitor.Agents.Core.Serialization;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Core.Contracts;

namespace Promitor.Agents.Scraper.Discovery
{
    public class ResourceDiscoveryClient
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        private readonly IOptionsMonitor<ResourceDiscoveryConfiguration> _configuration;
        private readonly ILogger<ResourceDiscoveryClient> _logger;
        private readonly HttpClient _httpClient;

        public ResourceDiscoveryClient(HttpClient httpClient, IOptionsMonitor<ResourceDiscoveryConfiguration> configuration, ILogger<ResourceDiscoveryClient> logger)
        {
            Guard.NotNull(httpClient, nameof(httpClient));
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(logger, nameof(logger));
            Guard.For<Exception>(() => configuration.CurrentValue.IsConfigured == false, "Resource Discovery is not configured");
            
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<PagedPayload<AzureResourceDefinition>> GetAsync(string resourceDiscoveryGroupName, int currentPage)
        {
            var uri = $"api/v2/resources/groups/{resourceDiscoveryGroupName}/discover?currentPage={currentPage}";
            var rawResponse = await SendGetRequestAsync(uri);

            var foundResources = JsonConvert.DeserializeObject<PagedPayload<AzureResourceDefinition>>(rawResponse, _serializerSettings);
            return foundResources;
        }

        public async Task<AgentHealthReport> GetHealthAsync()
        {
            var rawResponse = await SendGetRequestAsync("api/v1/health");
            var healthReport = JsonConvert.DeserializeObject<AgentHealthReport>(rawResponse, new HealthReportEntryConverter());
            return healthReport;
        }

        private async Task<string> SendGetRequestAsync(string uriPath)
        {
            var url = ComposeUrl(uriPath);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await SendRequestToApiAsync(request);
            response.EnsureSuccessStatusCode();

            var rawResponse = await response.Content.ReadAsStringAsync();
            return rawResponse;
        }

        private async Task<HttpResponseMessage> SendRequestToApiAsync(HttpRequestMessage request)
        {
            using (var dependencyMeasurement = DependencyMeasurement.Start())
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await _httpClient.SendAsync(request);
                    _logger.LogRequest(request, response, dependencyMeasurement.Elapsed);

                    return response;
                }
                finally
                {
                    try
                    {
                        var statusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
                        _logger.LogHttpDependency(request, statusCode, dependencyMeasurement);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Failed to log HTTP dependency. Reason: {Message}", ex.Message);
                    }
                }
            }
        }

        private Uri ComposeUrl(string uriPath)
        {
            return new Uri($"http://{_configuration.CurrentValue.Host}:{_configuration.CurrentValue.Port}/{uriPath}");
        }
    }
}