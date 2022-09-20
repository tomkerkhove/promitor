using System.Threading.Tasks;
using Promitor.Core;
using Promitor.Tests.Integration.Data;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper.MetricSinks
{
    public class OpenTelemetryMetricSinkTests : ScraperIntegrationTest
    {
        private readonly string _openTelemetryMetricNamespace;

        public OpenTelemetryMetricSinkTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
            _openTelemetryMetricNamespace = Configuration["OpenTelemetry:Collector:MetricNamespace"];
        }

        [SkippableTheory]
        [InlineData("promitor_ratelimit_arm")]
        [InlineData("promitor_scrape_success")]
        [InlineData("promitor_scrape_error")]
        public async Task OpenTelemetry_Scrape_ExpectedSystemMetricIsAvailable(string expectedMetricName)
        {
#if RELEASE
            Skip.If(System.OperatingSystem.IsWindows(), "OpenTelemetry Collector does not run on Windows");
#endif

            await AssertExpectedMetricIsAvailable(expectedMetricName);
        }

        [SkippableTheory]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_ExpectedScrapedMetricIsAvailable(string expectedMetricName)
        {
#if RELEASE
            Skip.If(System.OperatingSystem.IsWindows(), "OpenTelemetry Collector does not run on Windows");
#endif

            await AssertExpectedMetricIsAvailable(expectedMetricName);
        }

        [SkippableTheory]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_EveryMetricHasAnErrorMetric(string expectedMetricName)
        {
#if RELEASE
            Skip.If(System.OperatingSystem.IsWindows(), "OpenTelemetry Collector does not run on Windows");
#endif

            // Arrange
            const string errorMetricName = "promitor_scrape_error";
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [SkippableTheory]
        [ClassData(typeof(AvailableMetricsTestInputGenerator))]
        public async Task OpenTelemetry_Scrape_EveryMetricHasAnSuccessMetric(string expectedMetricName)
        {
#if RELEASE
            Skip.If(System.OperatingSystem.IsWindows(), "OpenTelemetry Collector does not run on Windows");
#endif

            // Arrange
            const string errorMetricName = "promitor_scrape_success";
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(errorMetricName, "metric_name", expectedMetricName);

            // Assert
            Assert.NotNull(gaugeMetric);
        }

        [SkippableFact]
        public async Task OpenTelemetry_Scrape_ExpectedRateLimitingForArmMetricIsAvailable()
        {
#if RELEASE
            Skip.If(System.OperatingSystem.IsWindows(), "OpenTelemetry Collector does not run on Windows");
#endif
            // Arrange
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.RateLimitingForArm);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal($"{_openTelemetryMetricNamespace}_{RuntimeMetricNames.RateLimitingForArm}", gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }
        
        [SkippableFact]
        public async Task OpenTelemetry_Scrape_ExpectedArmThrottledMetricIsAvailable()
        {
#if RELEASE
            Skip.If(System.OperatingSystem.IsWindows(), "OpenTelemetry Collector does not run on Windows");
#endif
            // Arrange
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration);

            // Act
            var gaugeMetric = await openTelemetryPrometheusClient.WaitForPrometheusMetricAsync(RuntimeMetricNames.ArmThrottled);

            // Assert
            Assert.NotNull(gaugeMetric);
            Assert.Equal($"{_openTelemetryMetricNamespace}_{RuntimeMetricNames.ArmThrottled}", gaugeMetric.Name);
            Assert.NotNull(gaugeMetric.Measurements);
            Assert.False(gaugeMetric.Measurements.Count < 1);
        }

        private async Task AssertExpectedMetricIsAvailable(string expectedMetricName)
        {
            // Arrange
            var openTelemetryPrometheusClient = PrometheusClientFactory.CreateForOpenTelemetryCollector(Configuration);

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
