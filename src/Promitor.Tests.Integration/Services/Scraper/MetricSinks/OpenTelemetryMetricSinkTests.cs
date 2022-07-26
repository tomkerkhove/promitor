using System.Threading.Tasks;
using Promitor.Core;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Data;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper.MetricSinks
{
    [Trait("Integrations", "OpenTelemetry")]
    public class OpenTelemetryMetricSinkTests : ScraperIntegrationTest
    {
        private readonly string _openTelemetryMetricNamespace;

        public OpenTelemetryMetricSinkTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
            _openTelemetryMetricNamespace = Configuration["OpenTelemetry:Collector:MetricNamespace"];
        }

        [Theory(Skip = "System metrics are not implemented yet")]
        [InlineData("promitor_ratelimit_arm")]
        [InlineData("promitor_scrape_success")]
        [InlineData("promitor_scrape_error")]
        public async Task OpenTelemetry_Scrape_ExpectedSystemMetricIsAvailable(string expectedMetricName)
        {
            await AssertExpectedMetricIsAvailable(expectedMetricName);
        }

        [Theory]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_ExpectedScrapedMetricIsAvailable(string expectedMetricName)
        {
            await AssertExpectedMetricIsAvailable(expectedMetricName);
        }

        [Theory(Skip = "System metrics are not implemented yet")]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_EveryMetricHasAnErrorMetric(string expectedMetricName)
        {
            // Arrange
            const string errorMetricName = "promitor_scrape_error";
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [Theory(Skip = "System metrics are not implemented yet")]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_EveryMetricHasAnSuccessMetric(string expectedMetricName)
        {
            // Arrange
            const string errorMetricName = "promitor_scrape_success";
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [Fact]
        public async Task OpenTelemetry_Scrape_ExpectedRateLimitingForArmMetricIsAvailable()
        {
            // Arrange
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.RateLimitingForArm);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal($"{_openTelemetryMetricNamespace}_{RuntimeMetricNames.RateLimitingForArm}", gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }
        
        [Fact(Skip = "System metrics are not implemented yet")]
        public async Task OpenTelemetry_Scrape_ExpectedArmThrottledMetricIsAvailable()
        {
            // Arrange
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.ArmThrottled);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal($"{_openTelemetryMetricNamespace}_{RuntimeMetricNames.RateLimitingForArm}", gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        private async Task AssertExpectedMetricIsAvailable(string expectedMetricName)
        {
            // Arrange
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal($"{_openTelemetryMetricNamespace}_{expectedMetricName}", gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }
    }
}
