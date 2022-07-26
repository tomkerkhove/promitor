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
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

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
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

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
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [Theory]
        [InlineData("promitor_runtime_dotnet_totalmemory")]
        [InlineData("promitor_runtime_process_virtual_bytes")]
        [InlineData("promitor_runtime_process_working_set")]
        [InlineData("promitor_runtime_process_private_bytes")]
        [InlineData("promitor_runtime_process_num_threads")]
        [InlineData("promitor_runtime_process_processid")]
        [InlineData("promitor_runtime_process_start_time_seconds")]
        public async Task OpenTelemetry_Scrape_ExpectedSystemPerformanceMetricIsAvailable(string expectedMetricName)
        {
            // Arrange
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(expectedMetricName, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
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
            Assert.Equal(RuntimeMetricNames.RateLimitingForArm, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        [Fact]
        public async Task OpenTelemetry_Scrape_ExpectedArmThrottledMetricIsAvailable()
        {
            // Arrange
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration, Logger);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.ArmThrottled);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(RuntimeMetricNames.ArmThrottled, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }
    }
}
