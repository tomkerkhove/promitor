using System.ComponentModel;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Validation.Steps.Sinks;
using Promitor.Tests.Unit.Generators.Config;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Metrics.Sinks
{
    [Category("Unit")]
    public class PrometheusScrapingEndpointMetricSinkValidationStepTests
    {
        [Fact]
        public void Validate_PrometheusScrapingEndpointIsFullyConfigured_Success()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();

            // Act
            var prometheusScrapingEndpointMetricSinkValidationStep = new PrometheusScrapingEndpointMetricSinkValidationStep(runtimeConfiguration);
            var validationResult = prometheusScrapingEndpointMetricSinkValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_PrometheusScrapingEndpointIsNotConfigured_Success()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.PrometheusScrapingEndpoint = null;

            // Act
            var prometheusScrapingEndpointMetricSinkValidationStep = new PrometheusScrapingEndpointMetricSinkValidationStep(runtimeConfiguration);
            var validationResult = prometheusScrapingEndpointMetricSinkValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }
        [Fact]
        public void Validate_NoSinksConfigured_Success()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks = null;

            // Act
            var prometheusScrapingEndpointMetricSinkValidationStep = new PrometheusScrapingEndpointMetricSinkValidationStep(runtimeConfiguration);
            var validationResult = prometheusScrapingEndpointMetricSinkValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_PrometheusScrapingEndpointWithoutBaseUriPath_Fails()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.PrometheusScrapingEndpoint.BaseUriPath = string.Empty;

            // Act
            var prometheusScrapingEndpointMetricSinkValidationStep = new PrometheusScrapingEndpointMetricSinkValidationStep(runtimeConfiguration);
            var validationResult = prometheusScrapingEndpointMetricSinkValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_PrometheusScrapingEndpointHasSameBaseUriPathAsLegacyPrometheusConfiguration_Fails()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.PrometheusScrapingEndpoint.BaseUriPath = runtimeConfiguration.Value.Prometheus.ScrapeEndpoint.BaseUriPath;

            // Act
            var prometheusScrapingEndpointMetricSinkValidationStep = new PrometheusScrapingEndpointMetricSinkValidationStep(runtimeConfiguration);
            var validationResult = prometheusScrapingEndpointMetricSinkValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        private IOptions<ScraperRuntimeConfiguration> CreateRuntimeConfiguration()
        {
            var bogusRuntimeConfiguration = BogusScraperRuntimeConfigurationGenerator.Generate();

            return Options.Create(bogusRuntimeConfiguration);
        }
    }
}