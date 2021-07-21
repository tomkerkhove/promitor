using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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

        public static IEnumerable<object[]> ConfiguredMetricNames { get; set; } = new List<object[]>();

        [Fact]
        public async Task Prometheus_Scrape_ReturnsOk()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.ScrapeWithResponseAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("promitor_ratelimit_arm")]
        [InlineData("promitor_scrape_success")]
        [InlineData("promitor_scrape_error")]
        [ClassData(typeof(TestDataGenerator))]
        public async Task Prometheus_Scrape_ExpectedMetricIsAvailable(string expectedMetricName)
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

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public async Task Prometheus_Scrape_EveryMetricHasAnErrorMetric(string expectedMetricName)
        {
            // Arrange
            const string errorMetricName = "promitor_scrape_error";
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var gaugeMetric = await scraperClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public async Task Prometheus_Scrape_EveryMetricHasAnSuccessMetric(string expectedMetricName)
        {
            // Arrange
            const string errorMetricName = "promitor_scrape_success";
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var gaugeMetric = await scraperClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [Fact]
        public async Task Prometheus_Scrape_Get_ReturnsVersionHeader()
        {
            // Arrange
            var scraperClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await scraperClient.ScrapeWithResponseAsync();

            // Assert
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }
    }
    
    public class TestDataGenerator : IEnumerable<object[]>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        IEnumerator<object[]> IEnumerable<object[]>.GetEnumerator()
        {
            var scraperClient = new ScraperClient(_configuration, _logger);
            var declaration = scraperClient.GetMetricDeclarationAsync().Result;
            return declaration.Select(x => new object[] { x.PrometheusMetricDefinition.Name }).GetEnumerator();
        }

        public IEnumerator GetEnumerator() => GetEnumerator();

        public TestDataGenerator()
        {
            _logger = NullLogger<TestDataGenerator>.Instance;

            // TODO: Move to central place and re-use for regular tests
            // The appsettings.local.json allows users to override (gitignored) settings locally for testing purposes
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(path: "appsettings.json")
                .AddJsonFile(path: "appsettings.local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
