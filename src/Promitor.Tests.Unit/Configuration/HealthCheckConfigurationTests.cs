using System.ComponentModel;
using Promitor.Agents.Scraper.Configuration;
using Xunit;

namespace Promitor.Tests.Unit.Configuration
{
    [Category("Unit")]
    public class HealthCheckConfigurationTests : UnitTest
    {
        [Fact]
        public void Constructor_DefaultConfiguration_ScraperFreshnessHealthCheckIsEnabledByDefault()
        {
            // Arrange & Act
            var configuration = new HealthCheckConfiguration();

            // Assert
            Assert.True(configuration.EnableScraperFreshnessHealthCheck, "EnableScraperFreshnessHealthCheck should be enabled by default");
        }

        [Fact]
        public void EnableScraperFreshnessHealthCheck_SetToFalse_CanBeDisabled()
        {
            // Arrange
            var configuration = new HealthCheckConfiguration();

            // Act
            configuration.EnableScraperFreshnessHealthCheck = false;

            // Assert
            Assert.False(configuration.EnableScraperFreshnessHealthCheck, "EnableScraperFreshnessHealthCheck should be disabled when explicitly set to false");
        }

        [Fact]
        public void EnableScraperFreshnessHealthCheck_SetToTrue_CanBeExplicitlyEnabled()
        {
            // Arrange
            var configuration = new HealthCheckConfiguration
            {
                EnableScraperFreshnessHealthCheck = false
            };

            // Act
            configuration.EnableScraperFreshnessHealthCheck = true;

            // Assert
            Assert.True(configuration.EnableScraperFreshnessHealthCheck, "EnableScraperFreshnessHealthCheck should be enabled when explicitly set to true");
        }

        [Fact]
        public void EnableScraperFreshnessHealthCheck_ObjectInitializer_CanOverrideDefault()
        {
            // Arrange & Act
            var configuration = new HealthCheckConfiguration
            {
                EnableScraperFreshnessHealthCheck = false
            };

            // Assert
            Assert.False(configuration.EnableScraperFreshnessHealthCheck, "EnableScraperFreshnessHealthCheck should respect object initializer override");
        }
    }
}

