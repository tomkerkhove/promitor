using System.Net;
using System.Threading.Tasks;
using Promitor.Agents.Core;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper
{
    public class HealthTests : ScraperIntegrationTest
    {
        public HealthTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Fact]
        public async Task Health_Get_ReturnsOk()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.GetHealthAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }
    }
}
