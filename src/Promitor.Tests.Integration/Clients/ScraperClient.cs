using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

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
            
            return GetDeserializedResponse<List<MetricDefinition>>(rawResponse);
        }
    }
}