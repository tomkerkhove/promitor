﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using Promitor.Core;
using Promitor.Integrations.Azure.Authentication;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureAuthenticationFactoryUnitTests
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
        public void GetConfiguredAzureAuthentication_UserAssignedManagedIdentityWithInvalidIdentity_Fails(string identityId)
        {
            // Arrange
            var expectedAuthenticationMode = AuthenticationMode.UserAssignedManagedIdentity;
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {ConfigurationKeys.Authentication.Mode, expectedAuthenticationMode.ToString()},
                {ConfigurationKeys.Authentication.IdentityId, identityId},
            };
            var config = CreateConfiguration(inMemoryConfiguration);

            // Act & Assert
            Assert.Throws<AuthenticationException>(() => AzureAuthenticationFactory.GetConfiguredAzureAuthentication(config));
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

        private IConfigurationRoot CreateConfiguration(Dictionary<string, string> inMemoryConfiguration)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfiguration)
                .Build();
        }
    }
}
