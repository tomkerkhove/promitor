using System;
using System.ComponentModel;
using Azure.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureCloudUnitTests : UnitTest
    {
        [Fact]
        public void GetAzureEnvironment_ForAzureGlobalCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Global;
            var expectedEnvironment = AzureEnvironment.AzureGlobalCloud;

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(null);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureChinaCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.China;
            var expectedEnvironment = AzureEnvironment.AzureChinaCloud;

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(null);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureGermanCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Germany;
            var expectedEnvironment = AzureEnvironment.AzureGermanCloud;

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(null);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureUSGovernmentCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.UsGov;
            var expectedEnvironment = AzureEnvironment.AzureUSGovernment;

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(null);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureCustomCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Custom;
            var azureEndpoints = new AzureEndpoints
            {
                AuthenticationEndpoint = "https://login.microsoftonline.com",
                ManagementEndpoint = "https://management.azure.com",
                ResourceManagerEndpoint = "https://management.azure.com",
                GraphEndpoint = "https://graph.windows.net",
                StorageEndpointSuffix = "core.windows.net",
                KeyVaultSuffix = "vault.azure.net"
            };

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(azureEndpoints);

            // Assert
            Assert.NotNull(azureEnvironment);
            Assert.Equal("Custom", azureEnvironment.Name);
            Assert.Equal(azureEndpoints.AuthenticationEndpoint, azureEnvironment.AuthenticationEndpoint);
            Assert.Equal(azureEndpoints.ManagementEndpoint, azureEnvironment.ManagementEndpoint);
            Assert.Equal(azureEndpoints.ResourceManagerEndpoint, azureEnvironment.ResourceManagerEndpoint);
            Assert.Equal(azureEndpoints.GraphEndpoint, azureEnvironment.GraphEndpoint);
        }

        [Fact]
        public void GetAzureEnvironment_ForUnspecifiedAzureCloud_ThrowsException()
        {
            // Arrange
            var azureCloud = AzureCloud.Unspecified;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(()=> azureCloud.GetAzureEnvironment(null));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureGlobalCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.Global;
            var expectedAuthorityHost = AzureAuthorityHosts.AzurePublicCloud;

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(null);

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureChinaCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.China;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureChina;

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(null);

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureGermanCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.Germany;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGermany;

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(null);

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureUSGovernmentCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.UsGov;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGovernment;

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(null);

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureCustomCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.Custom;
            var azureEndpoints = new AzureEndpoints
            {
                AuthenticationEndpoint = "https://login.microsoftonline.com",
            };
            var expectedAuthorityHost = new Uri(azureEndpoints.AuthenticationEndpoint);

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(azureEndpoints);
            
            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForUnspecifiedAzureCloud_ThrowsException()
        {
            // Arrange
            var azureCloud = AzureCloud.Unspecified;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => azureCloud.GetAzureAuthorityHost(null));
        }

        [Fact]
        public void DetermineMetricsClientAudience_ForAzureCustomCloud_ProvidesCorrectMetricsClientAudience()
        {
            // Arrange
            var azureCloud = AzureCloud.Custom;
            var expectedMetricsClientAudience = "https://custom.client.endpoint.com/";
            var azureEndpoints = new AzureEndpoints
            {
                MetricsClientAudience = expectedMetricsClientAudience
            };

            // Act
            var actualMetricsClientAudience = azureCloud.DetermineMetricsClientAudience(azureEndpoints);

            // Assert
            Assert.Equal(expectedMetricsClientAudience, actualMetricsClientAudience.ToString());
        }
        
        [Fact]
        public void DetermineMetricsClientBatchQueryAudience_ForAzureCustomCloud_ProvidesCorrectMetricsClientAudience()
        {
            // Arrange
            var azureCloud = AzureCloud.Custom;
            var expectedMetricsClientAudience = "https://custom.client.endpoint.com/";
            var azureEndpoints = new AzureEndpoints
            {
                MetricsClientAudience = expectedMetricsClientAudience
            };

            // Act
            var actualMetricsClientAudience = azureCloud.DetermineMetricsClientBatchQueryAudience(azureEndpoints);
            
            // Assert
            Assert.Equal(expectedMetricsClientAudience, actualMetricsClientAudience.ToString());
        }
    }
}
