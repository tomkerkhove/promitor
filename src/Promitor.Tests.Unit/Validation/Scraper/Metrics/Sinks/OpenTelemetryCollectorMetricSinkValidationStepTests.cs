using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Validation.Steps.Sinks;
using Promitor.Tests.Unit.Generators.Config;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics.Sinks
{
    [Category("Unit")]
    public class OpenTelemetryCollectorMetricSinkValidationStepTests : UnitTest
    {
        [Fact]
        public void Validate_OpenTelemetryCollectorIsFullyConfigured_Success()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();

            // Act
            var openTelemetryCollectorValidationStep = new OpenTelemetryCollectorMetricSinkValidationStep(runtimeConfiguration, NullLogger<OpenTelemetryCollectorMetricSinkValidationStep>.Instance);
            var validationResult = openTelemetryCollectorValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_OpenTelemetryCollectorIsNotConfigured_Success()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.OpenTelemetryCollector = null;

            // Act
            var openTelemetryCollectorValidationStep = new OpenTelemetryCollectorMetricSinkValidationStep(runtimeConfiguration, NullLogger<OpenTelemetryCollectorMetricSinkValidationStep>.Instance);
            var validationResult = openTelemetryCollectorValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }
        [Fact]
        public void Validate_NoSinksConfigured_Success()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks = null;

            // Act
            var openTelemetryCollectorValidationStep = new OpenTelemetryCollectorMetricSinkValidationStep(runtimeConfiguration, NullLogger<OpenTelemetryCollectorMetricSinkValidationStep>.Instance);
            var validationResult = openTelemetryCollectorValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_OpenTelemetryCollectorWithUnspecifiedCollectorUri_Fails(string collectorUri)
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.OpenTelemetryCollector.CollectorUri = collectorUri;

            // Act
            var openTelemetryCollectorValidationStep = new OpenTelemetryCollectorMetricSinkValidationStep(runtimeConfiguration, NullLogger<OpenTelemetryCollectorMetricSinkValidationStep>.Instance);
            var validationResult = openTelemetryCollectorValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Theory]
        [InlineData("http://")]
        [InlineData("http://foo:bar")]
        [InlineData("foo.bar")]
        [InlineData("foo.bar:1337")]
        public void Validate_OpenTelemetryCollectorWithInvalidCollectorUri_Fails(string collectorUri)
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.OpenTelemetryCollector.CollectorUri = collectorUri;

            // Act
            var openTelemetryCollectorValidationStep = new OpenTelemetryCollectorMetricSinkValidationStep(runtimeConfiguration, NullLogger<OpenTelemetryCollectorMetricSinkValidationStep>.Instance);
            var validationResult = openTelemetryCollectorValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Theory]
        [InlineData("http://foo")]
        [InlineData("http://foo.bar")]
        [InlineData("http://foo.bar:1337")]
        [InlineData("http://12.34.56.78")]
        [InlineData("https://foo")]
        [InlineData("https://foo.bar")]
        [InlineData("https://foo.bar:1337")]
        [InlineData("https://12.34.56.78")]
        public void Validate_OpenTelemetryCollectorWithValidCollectorUri_Fails(string collectorUri)
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.OpenTelemetryCollector.CollectorUri = collectorUri;

            // Act
            var openTelemetryCollectorValidationStep = new OpenTelemetryCollectorMetricSinkValidationStep(runtimeConfiguration, NullLogger<OpenTelemetryCollectorMetricSinkValidationStep>.Instance);
            var validationResult = openTelemetryCollectorValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        private IOptions<ScraperRuntimeConfiguration> CreateRuntimeConfiguration()
        {
            var bogusRuntimeConfiguration = BogusScraperRuntimeConfigurationGenerator.Generate();

            return Options.Create(bogusRuntimeConfiguration);
        }
    }
}