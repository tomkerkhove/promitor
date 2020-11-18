using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Promitor.Agents.Core;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Extensions;
using Xunit;
using Xunit.Abstractions;
using Promitor.Agents.Scraper.Configuration;

namespace Promitor.Tests.Integration.Services.Scraper
{
    public class ConfigurationTests : ScraperIntegrationTest
    {
        public ConfigurationTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Fact]
        public async Task RuntimeConfiguration_Get_ReturnsOk()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.GetRuntimeConfigurationAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawPayload = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawPayload);
            var metrics = JsonConvert.DeserializeObject<ScraperRuntimeConfiguration>(rawPayload);
            Assert.NotNull(metrics);
        }

        [Fact]
        public async Task RuntimeConfiguration_Get_ReturnsVersionHeader()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.GetRuntimeConfigurationAsync();

            // Assert
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }

        [Fact]
        public async Task MetricDeclaration_Get_ReturnsOk()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.GetMetricDeclarationAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task MetricDeclaration_Get_ReturnsVersionHeader()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.GetMetricDeclarationAsync();

            // Assert
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }
    }
}
