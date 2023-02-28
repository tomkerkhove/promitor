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
        [InlineData(null, "default-id")]
        [InlineData(null, " ")]
        [InlineData(null, null)]
        [InlineData("", "default-id")]
        [InlineData("", " ")]
        [InlineData("", null)]
        [InlineData(" ", "default-id")]
        [InlineData(" ", " ")]
        [InlineData(" ", null)]
        public void GetIdentityIdOrDefault_NullOrWhiteSpaceIdentityId_ReturnsDefaultValue(string identityId, string defaultValue)
        {
            // Arrange
            var azureAuthenticationInfo = new AzureAuthenticationInfo
            {
                IdentityId = identityId,
            };

            // Act
            var identityIdOrDefault = azureAuthenticationInfo.GetIdentityIdOrDefault(defaultValue);

            // Assert
            Assert.Equal(identityIdOrDefault, defaultValue);
        }
    }
}
