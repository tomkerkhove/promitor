using System;
using System.ComponentModel;
using Promitor.Core.Configuration.FeatureFlags;
using Promitor.Core.Configuration.Model.FeatureFlags;
using Promitor.Scraper.Tests.Unit.Stubs;
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
            var configStub = new OptionsMonitorStub<FeatureFlagsConfiguration>
            {
                CurrentValue = new FeatureFlagsConfiguration
                {
                    DisableMetricTimestamps = flagState
                }
            };
            var featureToggleClient = new FeatureToggleClient(configStub);

            // Act
            var flagStatus = featureToggleClient.IsActive(ToggleNames.DisableMetricTimestamps);

            // Assert
            Assert.True(flagStatus);
        }

        [Fact]
        public void FeatureFlag_FeatureIsConfiguredToBeOff_ReturnsOff()
        {
            // Arrange
            const bool flagState = false;
            var configStub = new OptionsMonitorStub<FeatureFlagsConfiguration>
            {
                CurrentValue = new FeatureFlagsConfiguration
                {
                    DisableMetricTimestamps = flagState
                }
            };
            var featureToggleClient = new FeatureToggleClient(configStub);

            // Act
            var flagStatus = featureToggleClient.IsActive(ToggleNames.DisableMetricTimestamps);

            // Assert
            Assert.False(flagStatus);
        }

        [Fact]
        public void FeatureFlag_FeatureIsNotConfigured_ReturnsOnByDefault()
        {
            //Arrange
            var configStub = new OptionsMonitorStub<FeatureFlagsConfiguration>();
            var featureToggleClient = new FeatureToggleClient(configStub);

            // Act
            var flagStatus = featureToggleClient.IsActive(ToggleNames.DisableMetricTimestamps);

            // Assert
            Assert.True(flagStatus);
        }

        [Fact]
        public void FeatureFlag_FeatureIsNotConfiguredButDefaultStateIsOff_Returns0ff()
        {
            // Arrange
            const bool defaultFlagState = false;
            var configStub = new OptionsMonitorStub<FeatureFlagsConfiguration>();
            var featureToggleClient = new FeatureToggleClient(configStub);

            // Act
            var flagStatus = featureToggleClient.IsActive(ToggleNames.DisableMetricTimestamps, defaultFlagState: defaultFlagState);

            // Assert
            Assert.False(flagStatus);
        }
    }
}