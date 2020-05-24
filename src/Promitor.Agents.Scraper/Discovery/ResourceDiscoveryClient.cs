using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Promitor.Agents.Core.Serialization;

namespace Promitor.Agents.Scraper.Discovery
{
    public class ResourceDiscoveryClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ResourceDiscoveryClient(IHttpClientFactory httpClientFactory)
        {
            Guard.NotNull(httpClientFactory,nameof(httpClientFactory));

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
            var client = _httpClientFactory.CreateClient("Promitor Resource Discovery");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var rawResponse = await response.Content.ReadAsStringAsync();
            return rawResponse;
        }
    }
}
