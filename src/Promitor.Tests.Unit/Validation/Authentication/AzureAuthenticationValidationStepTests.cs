using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Promitor.Core;
using Promitor.Agents.Scraper.Validation.Steps;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Authentication
{
    [Category("Unit")]
    public class AzureAuthenticationValidationStepTests
    {
        [Fact]
        public void ApplicationId_EmptyString_Fails()
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
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ApplicationId_Valid_Succeeds()
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
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        [Fact]
        public void ApplicationId_Whitespace_Fails()
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
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ApplicationKey_EmptyString_Fails()
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
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ApplicationKey_Whitespace_Fails()
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
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful);
        }

        [Fact]
        public void ApplicationKey_Valid_Succeeds()
        {
            // Arrange
            var invalidApplicationId = Guid.NewGuid().ToString();
            var invalidApplicationKey = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId},
                {EnvironmentVariables.Authentication.ApplicationKey, invalidApplicationKey},
            };

            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep(config);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        private IConfigurationRoot CreateConfiguration(Dictionary<string, string> inMemoryConfiguration)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfiguration)
                .Build();
        }
    }
}