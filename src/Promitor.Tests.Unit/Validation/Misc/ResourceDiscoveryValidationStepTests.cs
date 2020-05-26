using System.ComponentModel;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Tests.Unit.Generators.Config;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Misc
{
    [Category("Unit")]
    public class ResourceDiscoveryValidationStepTests
    {
        [Fact]
        public void Validate_ResourceDiscoveryIsFullyConfigured_Success()
        {
            // Arrange
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();

            // Act
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_ResourceDiscoveryIsNotConfigured_Success()
        {
            // Arrange
            ResourceDiscoveryConfiguration resourceDiscoveryConfiguration = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_StatsDWithNegativePort_Fails()
        {
            // Arrange
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();
            resourceDiscoveryConfiguration.Port = -1;

            // Act
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_StatsDWithPortZero_Fails()
        {
            // Arrange
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();
            resourceDiscoveryConfiguration.Port = 0;

            // Act
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_NoHostIsConfigured_Fails(string host)
        {
            // Arrange
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();
            resourceDiscoveryConfiguration.Host = host;

            // Act
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        private ResourceDiscoveryConfiguration CreateRuntimeConfiguration()
        {
            var bogusRuntimeConfiguration = BogusScraperRuntimeConfigurationGenerator.Generate();

            return bogusRuntimeConfiguration.ResourceDiscovery;
        }
    }
}