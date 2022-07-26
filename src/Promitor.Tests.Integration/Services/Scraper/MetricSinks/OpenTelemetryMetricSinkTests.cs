using System.Threading.Tasks;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Data;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper.MetricSinks
{
    [Trait("Integrations", "OpenTelemetry")]
    public class OpenTelemetryMetricSinkTests : ScraperIntegrationTest
    {
        public OpenTelemetryMetricSinkTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Theory]
        [InlineData("promitor_ratelimit_arm")]
        [InlineData("promitor_scrape_success")]
        [InlineData("promitor_scrape_error")]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_ExpectedMetricIsAvailable(string expectedMetricName)
        {
            // Arrange
            var openTelemetryPrometheusClient = GetOpenTelemetryPrometheusClient();

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(expectedMetricName, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        [Theory]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_EveryMetricHasAnErrorMetric(string expectedMetricName)
        {
            // Arrange
            const string errorMetricName = "promitor_scrape_error";
            var openTelemetryPrometheusClient = GetOpenTelemetryPrometheusClient();

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [Theory]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_EveryMetricHasAnSuccessMetric(string expectedMetricName)
        {
            // Arrange
            const string errorMetricName = "promitor_scrape_success";
            var openTelemetryPrometheusClient = GetOpenTelemetryPrometheusClient();

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        private PrometheusClient GetOpenTelemetryPrometheusClient()
        {
            return new PrometheusClient("OpenTelemetry:Collector:Uri", "OpenTelemetry:Collector:ScrapeUri", Configuration, Logger);
        }
    }
}
