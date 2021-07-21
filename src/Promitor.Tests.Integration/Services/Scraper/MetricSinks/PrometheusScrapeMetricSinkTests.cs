using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Promitor.Agents.Core;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper.MetricSinks
{
    public class PrometheusScrapeMetricSinkTests : ScraperIntegrationTest
    {
        public PrometheusScrapeMetricSinkTests(ITestOutputHelper testOutput)
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

        [Theory]
        [InlineData("promitor_ratelimit_arm")]
        [InlineData("promitor_scrape_success")]
        [InlineData("promitor_scrape_error")]
        public async Task Prometheus_Scrape_ReturnsArmRateLimitingHeader(string expectedMetricName)
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var gaugeMetric = await scraperClient.WaitForPrometheusMetricAsync(expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(expectedMetricName, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
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
