using System;
using System.ComponentModel;
using Promitor.Core.Infrastructure;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Infrastructure
{
    [Category("Unit")]
    public class FeatureFlagTests
    {
        [Fact]
        public void FeatureFlag_FeatureIsConfiguredToBeOn_ReturnsOn()
        {
            // Arrange
            const bool flagState = true;
            Environment.SetEnvironmentVariable("PROMITOR_FEATURE_TestFlag", flagState.ToString());

            // Act
            var flagStatus = FeatureFlag.IsActive("TestFlag");

            // Assert
            Assert.True(flagStatus);
        }

        [Fact]
        public void FeatureFlag_FeatureIsConfiguredToBeOff_ReturnsOff()
        {
            // Arrange
            const bool flagState = false;
            Environment.SetEnvironmentVariable("PROMITOR_FEATURE_TestFlag", flagState.ToString());

            // Act
            var flagStatus = FeatureFlag.IsActive("TestFlag");

            // Assert
            Assert.False(flagStatus);
        }

        [Fact]
        public void FeatureFlag_FeatureIsNotConfigured_ReturnsOnByDefault()
        {
            // Act
            var flagStatus = FeatureFlag.IsActive("TestFlag");

            // Assert
            Assert.True(flagStatus);
        }

        [Fact]
        public void FeatureFlag_FeatureIsNotConfiguredButDefaultStateIsOff_Returns0ff()
        {
            // Arrange
            const bool defaultFlagState = false;

            // Act
            var flagStatus = FeatureFlag.IsActive("TestFlag", defaultFlagState: defaultFlagState);

            // Assert
            Assert.False(flagStatus);
        }
    }
}