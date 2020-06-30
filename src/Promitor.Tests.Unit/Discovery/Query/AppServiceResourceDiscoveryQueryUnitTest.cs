using System;
using System.ComponentModel;
using Bogus;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Discovery.Query
{
    [Category("Unit")]
    public class AppServiceResourceDiscoveryQueryUnitTest
    {
        private readonly Faker _faker = new Faker();

        [Fact]
        public void DetermineAppDetails_FunctionAppWithoutSlot_ProvidesAppNameWithDefaultSlot()
        {
            // Arrange
            var functionAppName = _faker.Name.FirstName();
            var functionAppDiscoveryQuery = new FunctionAppDiscoveryQuery();

            // Act
            var appDetails = functionAppDiscoveryQuery.DetermineAppDetails(functionAppName);

            // Assert
            Assert.Equal(appDetails.AppName, functionAppName);
            Assert.Equal(Promitor.Core.Defaults.AppServices.SlotName, appDetails.SlotName);
        }

        [Fact]
        public void DetermineAppDetails_FunctionAppWithSlot_ProvidesAppNameWithDefaultSlot()
        {
            // Arrange
            var appName = _faker.Name.FirstName();
            var slotName = _faker.Name.FirstName();
            var functionAppName = $"{appName}/{slotName}";
            var functionAppDiscoveryQuery = new FunctionAppDiscoveryQuery();

            // Act
            var appDetails = functionAppDiscoveryQuery.DetermineAppDetails(functionAppName);

            // Assert
            Assert.Equal(appName, appDetails.AppName);
            Assert.Equal(slotName, appDetails.SlotName);
        }

        [Theory]
        [InlineData("funcappname/")]
        [InlineData("func/app/with/multiple/slashes")]
        public void DetermineAppDetails_FunctionAppWithInvalidName_ThrowsNotSupportedException(string functionAppName)
        {
            // Arrange
            var functionAppDiscoveryQuery = new FunctionAppDiscoveryQuery();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => functionAppDiscoveryQuery.DetermineAppDetails(functionAppName));
        }

        [Fact]
        public void DetermineAppDetails_WebAppWithoutSlot_ProvidesAppNameWithDefaultSlot()
        {
            // Arrange
            var webAppName = _faker.Name.FirstName();
            var webAppDiscoveryQuery = new WebAppDiscoveryQuery();

            // Act
            var appDetails = webAppDiscoveryQuery.DetermineAppDetails(webAppName);

            // Assert
            Assert.Equal(appDetails.AppName, webAppName);
            Assert.Equal(Promitor.Core.Defaults.AppServices.SlotName, appDetails.SlotName);
        }

        [Fact]
        public void DetermineAppDetails_WebAppWithSlot_ProvidesAppNameWithDefaultSlot()
        {
            // Arrange
            var appName = _faker.Name.FirstName();
            var slotName = _faker.Name.FirstName();
            var webAppName = $"{appName}/{slotName}";
            var webAppDiscoveryQuery = new WebAppDiscoveryQuery();

            // Act
            var appDetails = webAppDiscoveryQuery.DetermineAppDetails(webAppName);

            // Assert
            Assert.Equal(appName, appDetails.AppName);
            Assert.Equal(slotName, appDetails.SlotName);
        }

        [Theory]
        [InlineData("webappname/")]
        [InlineData("web/app/with/multiple/slashes")]
        public void DetermineAppDetails_WebAppWithInvalidName_ThrowsNotSupportedException(string webAppName)
        {
            // Arrange
            var webAppDiscoveryQuery = new WebAppDiscoveryQuery();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => webAppDiscoveryQuery.DetermineAppDetails(webAppName));
        }
    }
}
