using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Promitor.Tests.Integration
{
    public class ResourceDiscoveryClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ResourceDiscoveryClient(IConfiguration configuration, ILogger logger)
        {
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(logger, nameof(logger));

            var baseUrl = configuration["Agent:BaseUrl"];
            logger.LogInformation("Base URL for discovery agent is '{Url}'", baseUrl);

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _logger = logger;
        }

        public async Task<HttpResponseMessage> GetResourceCollectionsAsync()
        {
            return await GetAsync("/api/v1/resources/collections");
        }

        public async Task<HttpResponseMessage> GetHealthAsync()
        {
            return await GetAsync("/api/v1/health");
        }

        public async Task<HttpResponseMessage> GetDiscoveredResourcesAsync(string resourceCollectionName)
        {
            return await GetAsync($"/api/v1/resources/collections/{resourceCollectionName}/discovery");
        }

        private async Task<HttpResponseMessage> GetAsync(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.SendAsync(request);
            stopwatch.Stop();

            _logger.LogRequest(request, response, stopwatch.Elapsed);

            return response;
        }
    }
}