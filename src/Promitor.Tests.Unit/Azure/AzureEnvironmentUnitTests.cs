using System.ComponentModel;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Extensions;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureEnvironmentUnitTests : UnitTest
    {
        [Fact]
        public void GetDisplayName_ForAzureGlobalCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureGlobalCloud;
            var expectedDisplayName = "Global";

            // Act
            var displayName = azureEnvironment.GetDisplayName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }

        [Fact]
        public void GetDisplayName_ForAzureChinaCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureChinaCloud;
            var expectedDisplayName = "China";

            // Act
            var displayName = azureEnvironment.GetDisplayName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }

        [Fact]
        public void GetDisplayName_ForAzureGermanCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureGermanCloud;
            var expectedDisplayName = "German";

            // Act
            var displayName = azureEnvironment.GetDisplayName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }

        [Fact]
        public void GetDisplayName_ForUSGovernmentCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureUSGovernment;
            var expectedDisplayName = "US Government";

            // Act
            var displayName = azureEnvironment.GetDisplayName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }
    }
}
