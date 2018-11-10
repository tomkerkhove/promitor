using System;
using System.ComponentModel;
using Promitor.Core;
using Promitor.Scraper.Host.Validation.Steps;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Authentication
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
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId);
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep();
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
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationId, validApplicationId);
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep();
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
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId);
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationKey, validApplicationKey);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep();
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
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId);
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationKey, invalidApplicationKey);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep();
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
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId);
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationKey, invalidApplicationKey);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep();
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
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationId, invalidApplicationId);
            Environment.SetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationKey, invalidApplicationKey);

            // Act
            var azureAuthenticationValidationStep = new AzureAuthenticationValidationStep();
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }
    }
}