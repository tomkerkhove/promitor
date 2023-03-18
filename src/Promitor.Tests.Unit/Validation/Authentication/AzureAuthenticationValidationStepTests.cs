using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Core;
using Promitor.Integrations.Azure.Authentication;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Authentication
{
    [Category("Unit")]
    public class AzureAuthenticationValidationStepTests : UnitTest
    {
        [Fact]
        public void ServicePrinciple_IdentityIdInYamlIsValid_Succeeds()
        {
            // Arrange
            var validApplicationId = Guid.NewGuid().ToString();
            var validApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.IdentityId, validApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void ServicePrinciple_IdentityIdInYamlIsEmptyString_Fails()
        {
            // Arrange
            var invalidApplicationId = string.Empty;
            var validApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId},
                {ConfigurationKeys.Authentication.IdentityId, invalidApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ServicePrinciple_IdentityIdInYamlIsWhitespace_Fails()
        {
            // Arrange
            const string invalidApplicationId = " ";
            var validApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.IdentityId, invalidApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ServicePrinciple_IdentityIdInEnvironmentVariableIsEmptyString_Fails()
        {
            // Arrange
            var invalidApplicationId = string.Empty;
            var validApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ServicePrinciple_IdentityIdInEnvironmentVariableIsValid_Succeeds()
        {
            // Arrange
            var validApplicationId = Guid.NewGuid().ToString();
            var validApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {EnvironmentVariables.Authentication.ApplicationId, validApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void ServicePrinciple_IdentityIdInEnvironmentVariableIsWhitespace_Fails()
        {
            // Arrange
            const string invalidApplicationId = " ";
            var validApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ServicePrinciple_ApplicationKeyIsEmptyString_Fails()
        {
            // Arrange
            var invalidApplicationId = Guid.NewGuid().ToString();
            var invalidApplicationKey = string.Empty;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, invalidApplicationKey},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ServicePrinciple_ApplicationKeyHasWhitespace_Fails()
        {
            // Arrange
            var invalidApplicationId = Guid.NewGuid().ToString();
            const string invalidApplicationKey = " ";
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, invalidApplicationKey},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ServicePrinciple_ApplicationKeyIsValid_Succeeds()
        {
            // Arrange
            var invalidApplicationId = Guid.NewGuid().ToString();
            var validApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey},
            };

            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void UserAssignedManagedIdentity_ValidWithoutApplicationKey_Succeeds()
        {
            // Arrange
            var validApplicationId = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.IdentityId,validApplicationId},
                {ConfigurationKeys.Authentication.Mode, AuthenticationMode.UserAssignedManagedIdentity.ToString()},
            };

            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void UserAssignedManagedIdentity_IdentityIdIsEmptyString_Succeeds()
        {
            // Arrange
            var emptyApplicationId = string.Empty;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.IdentityId, emptyApplicationId},
                {ConfigurationKeys.Authentication.Mode, AuthenticationMode.UserAssignedManagedIdentity.ToString()},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void UserAssignedManagedIdentity_IdentityIdIsValid_Succeeds()
        {
            // Arrange
            var validApplicationId = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.IdentityId, validApplicationId},
                {ConfigurationKeys.Authentication.Mode, AuthenticationMode.UserAssignedManagedIdentity.ToString()},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void UserAssignedManagedIdentity_IdentityIdIsWhitespace_Succeeds()
        {
            // Arrange
            const string whitespaceApplicationId = " ";
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.IdentityId, whitespaceApplicationId},
                {ConfigurationKeys.Authentication.Mode, AuthenticationMode.UserAssignedManagedIdentity.ToString()},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void SystemAssignedManagedIdentity_ValidWithoutApplicationKey_Succeeds()
        {
            // Arrange
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, AuthenticationMode.SystemAssignedManagedIdentity.ToString()},
            };

            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        private IConfigurationRoot CreateConfiguration(Dictionary<string, string> inMemoryConfiguration)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfiguration)
                .Build();
        }
    }
}