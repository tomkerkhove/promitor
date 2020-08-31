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
    public class StatsDMetricSinkValidationStepTests
    {
        [Fact]
        public void Validate_StatsDIsFullyConfigured_Success()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();

            // Act
            var azureAuthenticationValidationStep = new StatsDMetricSinkValidationStep(runtimeConfiguration, NullLogger<StatsDMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_StatsDIsNotConfigured_Success()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.Statsd = null;

            // Act
            var azureAuthenticationValidationStep = new StatsDMetricSinkValidationStep(runtimeConfiguration, NullLogger<StatsDMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

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
            var azureAuthenticationValidationStep = new StatsDMetricSinkValidationStep(runtimeConfiguration, NullLogger<StatsDMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_StatsDWithNegativePort_Fails()
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.Statsd.Port = -1;

            // Act
            var azureAuthenticationValidationStep = new StatsDMetricSinkValidationStep(runtimeConfiguration, NullLogger<StatsDMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_StatsDWithoutHost_Fails(string host)
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.Statsd.Host = host;

            // Act
            var azureAuthenticationValidationStep = new StatsDMetricSinkValidationStep(runtimeConfiguration, NullLogger<StatsDMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_StatsDWithoutMetricPrefix_Succeeds(string metricPrefix)
        {
            // Arrange
            var runtimeConfiguration = CreateRuntimeConfiguration();
            runtimeConfiguration.Value.MetricSinks.Statsd.MetricPrefix = metricPrefix;

            // Act
            var azureAuthenticationValidationStep = new StatsDMetricSinkValidationStep(runtimeConfiguration, NullLogger<StatsDMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

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