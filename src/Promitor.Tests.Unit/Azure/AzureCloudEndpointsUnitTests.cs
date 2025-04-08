using System;
using Azure.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    public class AzureCloudEndpointsUnitTests : UnitTest
    {
        [Fact]
        public void GetAzureEnvironment_ForAzureGlobalCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Global, null);
            var expectedEnvironment = AzureEnvironment.AzureGlobalCloud;

            // Act
            var azureEnvironment = cloudEndpoint.GetAzureEnvironment();

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureChinaCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.China, null);
            var expectedEnvironment = AzureEnvironment.AzureChinaCloud;

            // Act
            var azureEnvironment = cloudEndpoint.GetAzureEnvironment();

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureGermanCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Germany, null);
            var expectedEnvironment = AzureEnvironment.AzureGermanCloud;

            // Act
            var azureEnvironment = cloudEndpoint.GetAzureEnvironment();

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureUSGovernmentCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.UsGov, null);
            var expectedEnvironment = AzureEnvironment.AzureUSGovernment;

            // Act
            var azureEnvironment = cloudEndpoint.GetAzureEnvironment();

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureCustomCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureEndpoints = new AzureEndpoints
            {
                AuthenticationEndpoint = "https://login.microsoftonline.com",
                ManagementEndpoint = "https://management.azure.com",
                ResourceManagerEndpoint = "https://management.azure.com",
                GraphEndpoint = "https://graph.windows.net",
                StorageEndpointSuffix = "core.windows.net",
                KeyVaultSuffix = "vault.azure.net"
            };
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Custom, azureEndpoints);

            // Act
            var azureEnvironment = cloudEndpoint.GetAzureEnvironment();

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
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Unspecified, null);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cloudEndpoint.GetAzureEnvironment());
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureGlobalCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Global, null);
            var expectedAuthorityHost = AzureAuthorityHosts.AzurePublicCloud;

            // Act
            var actualAuthorityHost = cloudEndpoint.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureChinaCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.China, null);
            var expectedAuthorityHost = AzureAuthorityHosts.AzureChina;

            // Act
            var actualAuthorityHost = cloudEndpoint.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureGermanCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Germany, null);
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGermany;

            // Act
            var actualAuthorityHost = cloudEndpoint.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureUSGovernmentCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.UsGov, null);
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGovernment;

            // Act
            var actualAuthorityHost = cloudEndpoint.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureCustomCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureEndpoints = new AzureEndpoints
            {
                AuthenticationEndpoint = "https://login.microsoftonline.com",
            };
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Custom, azureEndpoints);
            var expectedAuthorityHost = new Uri(azureEndpoints.AuthenticationEndpoint);

            // Act
            var actualAuthorityHost = cloudEndpoint.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForUnspecifiedAzureCloud_ThrowsException()
        {
            // Arrange
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Unspecified, null);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cloudEndpoint.GetAzureAuthorityHost());
        }

        [Fact]
        public void DetermineMetricsClientAudience_ForAzureCustomCloud_ProvidesCorrectMetricsClientAudience()
        {
            // Arrange
            var expectedMetricsQueryAudience = "https://custom.client.endpoint.com/";
            var azureEndpoints = new AzureEndpoints
            {
                MetricsQueryAudience = expectedMetricsQueryAudience
            };
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Custom, azureEndpoints);

            // Act
            var actualMetricsQueryAudience = cloudEndpoint.DetermineMetricsClientAudience();

            // Assert
            Assert.Equal(expectedMetricsQueryAudience, actualMetricsQueryAudience.ToString());
        }

        [Fact]
        public void DetermineMetricsClientBatchQueryAudience_ForAzureCustomCloud_ProvidesCorrectMetricsClientAudience()
        {
            // Arrange
            var expectedMetricsClientAudience = "https://custom.client.endpoint.com/";
            var azureEndpoints = new AzureEndpoints
            {
                MetricsClientAudience = expectedMetricsClientAudience
            };
            var cloudEndpoint = new TestCloudEndpoints(AzureCloud.Custom, azureEndpoints);

            // Act
            var actualMetricsClientAudience = cloudEndpoint.DetermineMetricsClientBatchQueryAudience();

            // Assert
            Assert.Equal(expectedMetricsClientAudience, actualMetricsClientAudience.ToString());
        }
    }
}