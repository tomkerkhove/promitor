using System.Net;
using System.Threading.Tasks;
using Promitor.Agents.Core;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper
{
    public class MetricSinksTests : ScraperIntegrationTest
    {
        public MetricSinksTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Fact]
        public async Task Prometheus_Scrape_ReturnsOk()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.ScrapeAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Prometheus_Scrape_Get_ReturnsVersionHeader()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.ScrapeAsync();

            // Assert
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }
    }
}
