using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Promitor.Agents.Core;
using Promitor.Tests.Integration.Data;
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
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInScraperAgent(Configuration);

            // Act
            var response = await prometheusClient.ScrapeWithResponseAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("promitor_ratelimit_arm")]
        [InlineData("promitor_scrape_success")]
        [InlineData("promitor_scrape_error")]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task Prometheus_Scrape_ExpectedMetricIsAvailable(string expectedMetricName)
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInScraperAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(expectedMetricName, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        [Theory]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task Prometheus_Scrape_EveryMetricHasAnErrorMetric(string expectedMetricName)
        {
            // Arrange
            const string errorMetricName = "promitor_scrape_error";
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInScraperAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [Theory]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task Prometheus_Scrape_EveryMetricHasAnSuccessMetric(string expectedMetricName)
        {
            // Arrange
            const string errorMetricName = "promitor_scrape_success";
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInScraperAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [Fact]
        public async Task Prometheus_Scrape_Get_ReturnsVersionHeader()
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInScraperAgent(Configuration);

            // Act
            var response = await prometheusClient.ScrapeWithResponseAsync();

            // Assert
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }

        [Theory]
        [MemberData(nameof(DimensionsData))]
        public async Task Prometheus_Scrape_ExpectedDimensionsAreAvailable(string expectedMetricName, IReadOnlyCollection<string> expectedDimensionNames)
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInScraperAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(expectedMetricName, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);

            foreach (var expectedDimensionName in expectedDimensionNames)
            {
                var sanitizedDimensionName = expectedDimensionName.SanitizeForPrometheusLabelKey();
                Assert.True(gaugeMetric.Measurements[0].Labels.ContainsKey(sanitizedDimensionName));
                Assert.NotEqual("unknown", gaugeMetric.Measurements[0].Labels[sanitizedDimensionName]);
            }
        }

        public static IEnumerable<object[]> DimensionsData(){
            yield return new object[] { "application_insights_availability_per_name", new List<string>{ "availabilityResult/name" } };
            yield return new object[] { "application_insights_availability_per_name_and_location", new List<string>{ "availabilityResult/name", "availabilityResult/location" } };
        }
    }
}
