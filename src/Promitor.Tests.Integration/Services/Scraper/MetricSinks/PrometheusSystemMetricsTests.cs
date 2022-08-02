using System.Threading.Tasks;
using Promitor.Core;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper.MetricSinks
{
    public class PrometheusSystemMetricsTests : ScraperIntegrationTest
    {
        public PrometheusSystemMetricsTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Theory]
        [InlineData("promitor_runtime_dotnet_totalmemory")]
        [InlineData("promitor_runtime_process_virtual_bytes")]
        [InlineData("promitor_runtime_process_working_set")]
        [InlineData("promitor_runtime_process_private_bytes")]
        [InlineData("promitor_runtime_process_num_threads")]
        [InlineData("promitor_runtime_process_processid")]
        [InlineData("promitor_runtime_process_start_time_seconds")]
        public async Task Prometheus_Scrape_ExpectedSystemPerformanceMetricIsAvailable(string expectedMetricName)
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

        [Fact]
        public async Task Prometheus_Scrape_ExpectedRateLimitingForArmMetricIsAvailable()
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInScraperAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.RateLimitingForArm);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(RuntimeMetricNames.RateLimitingForArm, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        [Fact]
        public async Task Prometheus_Scrape_ExpectedArmThrottledMetricIsAvailable()
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInScraperAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.ArmThrottled);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(RuntimeMetricNames.ArmThrottled, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }
    }
}
