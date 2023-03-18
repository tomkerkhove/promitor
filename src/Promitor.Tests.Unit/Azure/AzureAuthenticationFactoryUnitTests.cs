using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Extensions.Configuration;
using Promitor.Core;
using Promitor.Integrations.Azure.Authentication;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureAuthenticationFactoryUnitTests : UnitTest
    {
        [Fact]
        public void GetConfiguredAzureAuthentication_SystemAssignedManagedIdentityIsValid_Succeeds()
        {
            // Arrange
            var expectedAuthenticationMode = AuthenticationMode.SystemAssignedManagedIdentity;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var authenticationInfo = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config);

            // Assert
            Assert.Equal(expectedAuthenticationMode, authenticationInfo.Mode);
            Assert.Null(authenticationInfo.IdentityId);
            Assert.Null(authenticationInfo.Secret);
        }

        [Fact]
        public void GetConfiguredAzureAuthentication_UserAssignedManagedIdentityIsValid_Succeeds()
        {
            // Arrange
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.UserAssignedManagedIdentity;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {ConfigurationKeys.Authentication.IdentityId, expectedIdentityId},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var authenticationInfo = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config);

            // Assert
            Assert.Equal(expectedAuthenticationMode, authenticationInfo.Mode);
            Assert.Equal(expectedIdentityId, authenticationInfo.IdentityId);
            Assert.Null(authenticationInfo.Secret);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetConfiguredAzureAuthentication_UserAssignedManagedIdentityWithEmptyIdentity_Succeeds(string identityId)
        {
            // Arrange
            var expectedAuthenticationMode = AuthenticationMode.UserAssignedManagedIdentity;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {ConfigurationKeys.Authentication.IdentityId, identityId},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var authenticationInfo = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config);

            // Assert
            Assert.Equal(expectedAuthenticationMode, authenticationInfo.Mode);
            Assert.Equal(identityId, authenticationInfo.IdentityId);
            Assert.Null(authenticationInfo.Secret);
        }

        [Fact]
        public void GetConfiguredAzureAuthentication_ServicePrincipleIsValid_Succeeds()
        {
            // Arrange
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedSecret = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.ServicePrincipal;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {ConfigurationKeys.Authentication.IdentityId, expectedIdentityId},
                {EnvironmentVariables.Authentication.ApplicationKey, expectedSecret},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var authenticationInfo = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config);

            // Assert
            Assert.Equal(expectedAuthenticationMode, authenticationInfo.Mode);
            Assert.Equal(expectedIdentityId, authenticationInfo.IdentityId);
            Assert.Equal(expectedSecret, authenticationInfo.Secret);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetConfiguredAzureAuthentication_ServicePrincipleWithInvalidIdentityInYamlConfiguration_Fails(string identityId)
        {
            // Arrange
            var expectedSecret = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.ServicePrincipal;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {ConfigurationKeys.Authentication.IdentityId, identityId},
                {EnvironmentVariables.Authentication.ApplicationKey, expectedSecret},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act & Assert
            Assert.Throws<AuthenticationException>(() => AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetConfiguredAzureAuthentication_ServicePrincipleWithInvalidIdentityInLegacyConfiguration_Fails(string identityId)
        {
            // Arrange
            var expectedSecret = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.ServicePrincipal;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {EnvironmentVariables.Authentication.ApplicationId, identityId},
                {EnvironmentVariables.Authentication.ApplicationKey, expectedSecret},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act & Assert
            Assert.Throws<AuthenticationException>(() => AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetConfiguredAzureAuthentication_ServicePrincipleWithInvalidSecret_Fails(string identitySecret)
        {
            // Arrange
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.ServicePrincipal;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {EnvironmentVariables.Authentication.ApplicationId, expectedIdentityId},
                {EnvironmentVariables.Authentication.ApplicationKey, identitySecret},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act & Assert
            Assert.Throws<AuthenticationException>(() => AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config));
        }

        [Fact]
        public void GetConfiguredAzureAuthentication_NoAuthenticationModeIsConfigured_DefaultsToServicePrinciple()
        {
            // Arrange
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedSecret = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.IdentityId, expectedIdentityId},
                {EnvironmentVariables.Authentication.ApplicationKey, expectedSecret},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var authenticationInfo = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config);

            // Assert
            Assert.Equal(AuthenticationMode.ServicePrincipal, authenticationInfo.Mode);
            Assert.Equal(expectedIdentityId, authenticationInfo.IdentityId);
            Assert.Equal(expectedSecret, authenticationInfo.Secret);
        }

        [Fact]
        public void GetConfiguredAzureAuthentication_ServicePrincipleIsValidWithLegacyAndNewConfig_UsesNewConfig()
        {
            // Arrange
            var configuredIdentityIdThroughNewApproach = Guid.NewGuid().ToString();
            var configuredIdentityIdThroughLegacyApproach = Guid.NewGuid().ToString();
            var expectedSecret = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.ServicePrincipal;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {ConfigurationKeys.Authentication.IdentityId, configuredIdentityIdThroughNewApproach},
                {EnvironmentVariables.Authentication.ApplicationId, configuredIdentityIdThroughLegacyApproach},
                {EnvironmentVariables.Authentication.ApplicationKey, expectedSecret},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var authenticationInfo = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config);

            // Assert
            Assert.Equal(expectedAuthenticationMode, authenticationInfo.Mode);
            Assert.Equal(configuredIdentityIdThroughNewApproach, authenticationInfo.IdentityId);
            Assert.Equal(expectedSecret, authenticationInfo.Secret);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetConfiguredAzureAuthentication_ServicePrincipleWithInvalidSecretFilePath_Fails(string secretFilePath)
        {
            // Arrange
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.ServicePrincipal;
            var expectedSecretFileName = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {EnvironmentVariables.Authentication.ApplicationId, expectedIdentityId},
                {ConfigurationKeys.Authentication.SecretFilePath, secretFilePath},
                {ConfigurationKeys.Authentication.SecretFileName, expectedSecretFileName}
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act & Assert
            Assert.Throws<AuthenticationException>(() => AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetConfiguredAzureAuthentication_ServicePrincipleWithInvalidSecretFileName_Fails(string secretFileName)
        {
            // Arrange
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.ServicePrincipal;
            var expectedSecretFilePath = Guid.NewGuid().ToString();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {EnvironmentVariables.Authentication.ApplicationId, expectedIdentityId},
                {ConfigurationKeys.Authentication.SecretFilePath, expectedSecretFilePath},
                {ConfigurationKeys.Authentication.SecretFileName, secretFileName}
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act & Assert
            Assert.Throws<AuthenticationException>(() => AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config));
        }

        [Fact]
        public void GetConfiguredAzureAuthentication_ServicePrincipleWithValidSecretFileName_Succeeds()
        {
            // Arrange
            const string secretFilePath = "Files/valid-secret-file";
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedAuthenticationMode = AuthenticationMode.ServicePrincipal;
            var expectedSecretFilePath = "Files";
            var expectedSecretFileName = "valid-secret-file";
            var expectedSecret = File.ReadAllText(secretFilePath);
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {EnvironmentVariables.Authentication.ApplicationId, expectedIdentityId},
                {ConfigurationKeys.Authentication.SecretFilePath, expectedSecretFilePath},
                {ConfigurationKeys.Authentication.SecretFileName, expectedSecretFileName}
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act
            var authenticationInfo = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config);

            // Act & Assert
            Assert.Equal(expectedSecret, authenticationInfo.Secret);
        }

        [Fact]
        public void CreateAzureAuthentication_SystemAssignedManagedIdentityIsValid_Succeeds()
        {
            // Arrange
            var expectedTenantId = Guid.NewGuid().ToString();
            var azureCloud = AzureEnvironment.AzureChinaCloud;
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                Mode = AuthenticationMode.SystemAssignedManagedIdentity
            };
            var azureCredentialFactory = new AzureCredentialsFactory();

            // Act
            var azureCredentials = AzureAuthenticationFactory.CreateAzureAuthentication(azureCloud, expectedTenantId, azureAuthenticationInfo, azureCredentialFactory);

            // Assert
            Assert.Equal(expectedTenantId, azureCredentials.TenantId);
            Assert.Equal(azureCloud, azureCredentials.Environment);
            Assert.Null(azureCredentials.ClientId);
        }

        [Fact]
        public void CreateAzureAuthentication_UserAssignedManagedIdentityIsValid_Succeeds()
        {
            // Arrange
            var expectedTenantId = Guid.NewGuid().ToString();
            var expectedIdentityId = Guid.NewGuid().ToString();
            var azureCloud = AzureEnvironment.AzureChinaCloud;
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                Mode = AuthenticationMode.UserAssignedManagedIdentity,
                IdentityId = expectedIdentityId
            };
            var azureCredentialFactory = new AzureCredentialsFactory();

            // Act
            var azureCredentials = AzureAuthenticationFactory.CreateAzureAuthentication(azureCloud, expectedTenantId, azureAuthenticationInfo, azureCredentialFactory);

            // Assert
            Assert.Equal(expectedTenantId, azureCredentials.TenantId);
            Assert.Equal(azureCloud, azureCredentials.Environment);
            // Client id for user-assigned MI is not exposed
            Assert.Null(azureCredentials.ClientId);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CreateAzureAuthentication_UserAssignedManagedIdentityWithEmptyIdentity_Succeeds(string identityId)
        {
            // Arrange
            var expectedTenantId = Guid.NewGuid().ToString();
            var azureCloud = AzureEnvironment.AzureChinaCloud;
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                Mode = AuthenticationMode.UserAssignedManagedIdentity,
                IdentityId = identityId
            };
            var azureCredentialFactory = new AzureCredentialsFactory();

            // Act
            var azureCredentials = AzureAuthenticationFactory.CreateAzureAuthentication(azureCloud, expectedTenantId, azureAuthenticationInfo, azureCredentialFactory);

            // Assert
            Assert.Equal(expectedTenantId, azureCredentials.TenantId);
            Assert.Equal(azureCloud, azureCredentials.Environment);
            // Client id for user-assigned MI is not exposed
            Assert.Null(azureCredentials.ClientId);
        }

        [Fact]
        public void CreateAzureAuthentication_NoModeSpecified_AssumesServicePrinciple()
        {
            // Arrange
            var expectedTenantId = Guid.NewGuid().ToString();
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedSecret = Guid.NewGuid().ToString();
            var azureCloud = AzureEnvironment.AzureChinaCloud;
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                IdentityId = expectedIdentityId,
                Secret = expectedSecret
            };
            var azureCredentialFactory = new AzureCredentialsFactory();

            // Act
            var azureCredentials = AzureAuthenticationFactory.CreateAzureAuthentication(azureCloud, expectedTenantId, azureAuthenticationInfo, azureCredentialFactory);

            // Assert
            Assert.Equal(expectedTenantId, azureCredentials.TenantId);
            Assert.Equal(expectedIdentityId, azureCredentials.ClientId);
            Assert.Equal(azureCloud, azureCredentials.Environment);
        }

        [Fact]
        public void CreateAzureAuthentication_ServicePrincipleIsValid_Succeeds()
        {
            // Arrange
            var expectedTenantId = Guid.NewGuid().ToString();
            var expectedIdentityId = Guid.NewGuid().ToString();
            var expectedSecret = Guid.NewGuid().ToString();
            var azureCloud = AzureEnvironment.AzureChinaCloud;
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                Mode = AuthenticationMode.ServicePrincipal,
                IdentityId = expectedIdentityId,
                Secret = expectedSecret
            };
            var azureCredentialFactory = new AzureCredentialsFactory();

            // Act
            var azureCredentials = AzureAuthenticationFactory.CreateAzureAuthentication(azureCloud, expectedTenantId, azureAuthenticationInfo, azureCredentialFactory);

            // Assert
            Assert.Equal(expectedTenantId, azureCredentials.TenantId);
            Assert.Equal(expectedIdentityId, azureCredentials.ClientId);
            Assert.Equal(azureCloud, azureCredentials.Environment);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CreateAzureAuthentication_ServicePrincipleWithInvalidSecret_Fails(string secret)
        {
            // Arrange
            var expectedTenantId = Guid.NewGuid().ToString();
            var expectedIdentityId = Guid.NewGuid().ToString();
            var azureCloud = AzureEnvironment.AzureChinaCloud;
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                Mode = AuthenticationMode.ServicePrincipal,
                IdentityId = expectedIdentityId,
                Secret = secret
            };
            var azureCredentialFactory = new AzureCredentialsFactory();

            // Act & Assert
            Assert.Throws<AuthenticationException>(() => AzureAuthenticationFactory.CreateAzureAuthentication(azureCloud, expectedTenantId, azureAuthenticationInfo, azureCredentialFactory));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CreateAzureAuthentication_ServicePrincipleWithInvalidIdentity_Fails(string identityId)
        {
            // Arrange
            var expectedTenantId = Guid.NewGuid().ToString();
            var expectedSecret = Guid.NewGuid().ToString();
            var azureCloud = AzureEnvironment.AzureChinaCloud;
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                Mode = AuthenticationMode.ServicePrincipal,
                IdentityId = identityId,
                Secret = expectedSecret
            };
            var azureCredentialFactory = new AzureCredentialsFactory();

            // Act & Assert
            Assert.Throws<AuthenticationException>(() => AzureAuthenticationFactory.CreateAzureAuthentication(azureCloud, expectedTenantId, azureAuthenticationInfo, azureCredentialFactory));
        }

        private IConfigurationRoot CreateConfiguration(Dictionary<string, string> inMemoryConfiguration)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfiguration)
                .Build();
        }
    }
}
