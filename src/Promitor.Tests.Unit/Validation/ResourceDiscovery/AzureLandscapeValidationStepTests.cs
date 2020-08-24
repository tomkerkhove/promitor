using System;
using System.Collections.Generic;
using System.ComponentModel;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Validation.Steps;
using Promitor.Core.Serialization.Enum;
using Xunit;

namespace Promitor.Tests.Unit.Validation.ResourceDiscovery
{
    [Category("Unit")]
    public class AzureLandscapeValidationStepTests
    {
        [Fact]
        public void Validate_AzureLandscapeIsFullyConfigured_Success()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateLandscapeConfiguration();

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_InvalidTenantIdIsConfigured_Fails(string tenantId)
        {
            // Arrange
            var azureLandscapeConfiguration = CreateLandscapeConfiguration();
            azureLandscapeConfiguration.Value.TenantId = tenantId;

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_AzureCloudIsUnspecifiedIsConfigured_Fails()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Cloud = AzureCloud.Unspecified;

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_NoSubscriptionIsConfigured_Fails()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Subscriptions = null;

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_DuplicateSubscriptionsAreConfigured_Fails()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            var azureLandscapeConfiguration = CreateLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Subscriptions = new List<string> { subscriptionId, subscriptionId };

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void Validate_EmptySubscriptionsAreConfigured_Fails()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Subscriptions = new List<string> { string.Empty };

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        private IOptions<AzureLandscape> CreateLandscapeConfiguration()
        {
            var azureLandscape = new Faker<AzureLandscape>()
                .StrictMode(true)
                .RuleFor(landscape => landscape.Subscriptions, faker => new List<string> { faker.Name.FirstName(), faker.Name.FirstName() })
                .RuleFor(landscape => landscape.TenantId, faker => faker.Name.FirstName())
                .RuleFor(landscape => landscape.Cloud, faker => faker.PickRandom<AzureCloud>())
                .Generate();

            return Options.Create(azureLandscape);
        }
    }
}