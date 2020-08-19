using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Promitor.Tests.Integration.Clients
{
    public class ScraperClient: AgentClient
    {
        public ScraperClient(IConfiguration configuration, ILogger logger)
        :base("Scraper", "Agents:Scraper:BaseUrl", configuration,logger)
        {
        }
    }
}