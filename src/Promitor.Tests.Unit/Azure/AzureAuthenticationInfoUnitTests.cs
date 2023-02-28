using System.ComponentModel;
using Promitor.Integrations.Azure.Authentication;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureAuthenticationInfoUnitTests : UnitTest
    {
        [Theory]
        [InlineData("default-id")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetIdentityIdOrDefault_NonEmptyIdentityId_ReturnsIndentityId(string defaultValue)
        {
            // Arrange
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                IdentityId = "some-identity-id",
            };

            // Act
            var identityIdOrDefault = azureAuthenticationInfo.GetIdentityIdOrDefault(defaultValue);

            // Assert
            Assert.Equal(identityIdOrDefault, azureAuthenticationInfo.IdentityId);
        }

        [Theory]
        [InlineData("default-id")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetIdentityIdOrDefault_NullIdentityId_ReturnsDefaultValue(string defaultValue)
        {
            // Arrange
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                IdentityId = null,
            };

            // Act
            var identityIdOrDefault = azureAuthenticationInfo.GetIdentityIdOrDefault(defaultValue);

            // Assert
            Assert.Equal(identityIdOrDefault, defaultValue);
        }

        [Theory]
        [InlineData("default-id")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetIdentityIdOrDefault_WhiteSpaceIdentityId_ReturnsDefaultValue(string defaultValue)
        {
            // Arrange
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                IdentityId = " ",
            };

            // Act
            var identityIdOrDefault = azureAuthenticationInfo.GetIdentityIdOrDefault(defaultValue);

            // Assert
            Assert.Equal(identityIdOrDefault, defaultValue);
        }
    }
}
