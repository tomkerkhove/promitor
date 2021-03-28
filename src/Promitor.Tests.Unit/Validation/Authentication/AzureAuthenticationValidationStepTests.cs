using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Core;
using Promitor.Integrations.Azure.Authentication;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Authentication
{
    [Category("Unit")]
    public class AzureAuthenticationValidationStepTests
    {
        private const string IdentityIdConfigKey = "authentication:IdentityId";
        private const string AuthenticationModeConfigKey = "authentication:Mode";

        [Fact]
        public void ServicePrinciple_IdentityIdInYamlIsValid_Succeeds()
        {
            // Arrange
            var validApplicationId = Guid.NewGuid().ToString();
            var validApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {IdentityIdConfigKey, validApplicationId},
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
                {IdentityIdConfigKey, invalidApplicationId},
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
                {IdentityIdConfigKey, invalidApplicationId},
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
                {IdentityIdConfigKey,validApplicationId},
                {AuthenticationModeConfigKey, AuthenticationMode.UserAssignedManagedIdentity.ToString()},
            };

            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void UserAssignedManagedIdentity_IdentityIdIsEmptyString_Fails()
        {
            // Arrange
            var invalidApplicationId = string.Empty;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {IdentityIdConfigKey, invalidApplicationId},
                {AuthenticationModeConfigKey, AuthenticationMode.UserAssignedManagedIdentity.ToString()},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void UserAssignedManagedIdentity_IdentityIdIsValid_Succeeds()
        {
            // Arrange
            var validApplicationId = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {IdentityIdConfigKey, validApplicationId},
                {AuthenticationModeConfigKey, AuthenticationMode.UserAssignedManagedIdentity.ToString()},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void UserAssignedManagedIdentity_IdentityIdIsWhitespace_Fails()
        {
            // Arrange
            const string invalidApplicationId = " ";
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {IdentityIdConfigKey, invalidApplicationId},
                {AuthenticationModeConfigKey, AuthenticationMode.UserAssignedManagedIdentity.ToString()},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config, NullLogger<AzureAuthenticationValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void SystemAssignedManagedIdentity_ValidWithoutApplicationKey_Succeeds()
        {
            // Arrange
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {AuthenticationModeConfigKey, AuthenticationMode.SystemAssignedManagedIdentity.ToString()},
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