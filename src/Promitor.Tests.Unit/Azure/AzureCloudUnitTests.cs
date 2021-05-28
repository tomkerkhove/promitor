using System;
using System.ComponentModel;
using Azure.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
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
            var azureEnvironment = azureCloud.GetAzureEnvironment();

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
            var azureEnvironment = azureCloud.GetAzureEnvironment();

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
            var azureEnvironment = azureCloud.GetAzureEnvironment();

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
            var azureEnvironment = azureCloud.GetAzureEnvironment();

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForUnspecifiedAzureCloud_ThrowsException()
        {
            // Arrange
            var azureCloud = AzureCloud.Unspecified;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(()=> azureCloud.GetAzureEnvironment());
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureGlobalCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Global;
            var expectedAuthorityHost = AzureAuthorityHosts.AzurePublicCloud;

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost();

            // Assert
            PromitorAssert.Equal(expectedAuthorityHost, actualAuthorityHost);
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureChinaCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.China;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureChina;

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost();

            // Assert
            PromitorAssert.Equal(expectedAuthorityHost, actualAuthorityHost);
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureGermanCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Germany;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGermany;

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost();

            // Assert
            PromitorAssert.Equal(expectedAuthorityHost, actualAuthorityHost);
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureUSGovernmentCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.UsGov;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGovernment;

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost();

            // Assert
            PromitorAssert.Equal(expectedAuthorityHost, actualAuthorityHost);
        }

        [Fact]
        public void GetAzureAuthorityHost_ForUnspecifiedAzureCloud_ThrowsException()
        {
            // Arrange
            var azureCloud = AzureCloud.Unspecified;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => azureCloud.GetAzureAuthorityHost());
        }
    }
}
