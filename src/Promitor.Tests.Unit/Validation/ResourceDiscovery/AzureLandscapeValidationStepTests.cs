using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Validation.Steps;
using Promitor.Core.Configuration;
using Promitor.Core.Serialization.Enum;
using Xunit;

namespace Promitor.Tests.Unit.Validation.ResourceDiscovery
{
    [Category("Unit")]
    public class AzureLandscapeValidationStepTests : UnitTest
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
            PromitorAssert.ValidationIsSuccessful(validationResult);
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
            PromitorAssert.ValidationFailed(validationResult);
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
            PromitorAssert.ValidationFailed(validationResult);
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
            PromitorAssert.ValidationFailed(validationResult);
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
            PromitorAssert.ValidationFailed(validationResult);
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
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_ForCustomCloudEndpointsAreFullyConfigured_Success()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateCustomCloudLandscapeConfiguration();

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_ForCustomCloudEndpointsAreNull_Fails()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateCustomCloudLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Endpoints = null;

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_ForCustomCloudAuthenticationEndpointIsNotConfigured_Fails()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateCustomCloudLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Endpoints.AuthenticationEndpoint = null;
            
            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();
            
            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_ForCustomCloudManagementEndpointIsNotConfigured_Fails()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateCustomCloudLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Endpoints.ManagementEndpoint = null;

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_ForCustomCloudResourceManagerEndpointIsNotConfigured_Fails()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateCustomCloudLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Endpoints.ResourceManagerEndpoint = null;
        
            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();
            
            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_ForCustomCloudGraphEndpointIsNotConfigured_Fails()
        {
            // Arrange
            var azureLandscapeConfiguration = CreateCustomCloudLandscapeConfiguration();
            azureLandscapeConfiguration.Value.Endpoints.GraphEndpoint = null;

            // Act
            var azureLandscapeValidationStep = new AzureLandscapeValidationStep(azureLandscapeConfiguration, NullLogger<AzureLandscapeValidationStep>.Instance);
            var validationResult = azureLandscapeValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        private IOptions<AzureLandscape> CreateLandscapeConfiguration()
        {
            var allAzureCloudValues = Enum.GetValues(typeof(AzureCloud));
            var allowedAzureClouds = allAzureCloudValues.OfType<AzureCloud>().Where(entry => (entry != AzureCloud.Unspecified && entry != AzureCloud.Custom)).ToList();

            var azureLandscape = new Faker<AzureLandscape>()
                .StrictMode(true)
                .RuleFor(landscape => landscape.Subscriptions, faker => new List<string> { faker.Name.FirstName(), faker.Name.FirstName() })
                .RuleFor(landscape => landscape.TenantId, faker => faker.Name.FirstName())
                .RuleFor(landscape => landscape.Cloud, faker => faker.PickRandom(allowedAzureClouds))
                .RuleFor(landscape => landscape.Endpoints, faker => null)
                .Generate();

            return Options.Create(azureLandscape);
        }

        private IOptions<AzureLandscape> CreateCustomCloudLandscapeConfiguration()
        {
            var azureLandscape = new Faker<AzureLandscape>()
                .StrictMode(true)
                .RuleFor(landscape => landscape.Subscriptions, faker => new List<string> { faker.Name.FirstName(), faker.Name.FirstName() })
                .RuleFor(landscape => landscape.TenantId, faker => faker.Name.FirstName())
                .RuleFor(landscape => landscape.Cloud, faker => AzureCloud.Custom)
                .RuleFor(landscape => landscape.Endpoints, faker => new AzureEndpoints 
                {
                    AuthenticationEndpoint = "https://auth.com/",
                    ResourceManagerEndpoint = "https://resource.com/",
                    GraphEndpoint = "https://graph.net/",
                    ManagementEndpoint = "https://management.net/",
                    StorageEndpointSuffix = "storage.net",
                    KeyVaultSuffix = "vault.net",
                    MetricsQueryAudience = "https://monitoring.azure.com/",
                    MetricsClientAudience = "https://api.monitoring.azure.com/"
                })
                .Generate();

            return Options.Create(azureLandscape);
        }
    }
}