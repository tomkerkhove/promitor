using System.Net;
using System.Threading.Tasks;
using Promitor.Tests.Integration.Clients;
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
            var resourceDiscoveryClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetHealthAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
