using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
    }
}