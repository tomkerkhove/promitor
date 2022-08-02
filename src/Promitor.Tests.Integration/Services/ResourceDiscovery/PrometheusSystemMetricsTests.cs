using System.Threading.Tasks;
using Promitor.Agents.ResourceDiscovery.Scheduling;
using Promitor.Core;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.ResourceDiscovery
{
    public class PrometheusSystemMetricsTests : ResourceDiscoveryIntegrationTest
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
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInResourceDiscoveryAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(expectedMetricName, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        [Fact]
        public async Task Prometheus_Scrape_ExpectedAzureSubscriptionInfoMetricIsAvailable()
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInResourceDiscoveryAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(AzureSubscriptionDiscoveryBackgroundJob.MetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(AzureSubscriptionDiscoveryBackgroundJob.MetricName, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.Single(gaugeMetric.Measurements);
        }

        [Fact]
        public async Task Prometheus_Scrape_ExpectedAzureResourceGroupInfoMetricIsAvailable()
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInResourceDiscoveryAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(AzureResourceGroupsDiscoveryBackgroundJob.MetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(AzureResourceGroupsDiscoveryBackgroundJob.MetricName, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        [Fact]
        public async Task Prometheus_Scrape_ExpectedAzureResourceGraphThrottlingMetricIsAvailable()
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInResourceDiscoveryAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.RateLimitingForResourceGraph);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(RuntimeMetricNames.RateLimitingForResourceGraph, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        [Fact]
        public async Task Prometheus_Scrape_ExpectedResourceGraphThrottledMetricIsAvailable()
        {
            // Arrange
            var prometheusClient = PrometheusClientFactory.CreateForPrometheusScrapingEndpointInResourceDiscoveryAgent(Configuration);

            // Act
            var gaugeMetric = await prometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.ResourceGraphThrottled);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal(RuntimeMetricNames.ResourceGraphThrottled, gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }
    }
}
