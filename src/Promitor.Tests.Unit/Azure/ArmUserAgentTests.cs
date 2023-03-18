using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.AzureMonitor;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class ArmUserAgentTests : UnitTest
    {
        private const string UserNameFormat = "App/Promitor Agent/Scraper Version/{0} OS/{1} Prometheus/{2} OpenTelemetryCollector/{3} StatsD/{4} AtlassianStatuspage/{5}";
        private const string EnabledText = "Enabled";
        private const string DisabledText = "Disabled";

        [Fact]
        public void Generate_AllSinksEnabled_ReturnsCorrectUserAgent()
        {
            // Arrange
            var agentVersion = "1.2.3";
            var enabledMetricSinks = new List<MetricSinkType>
            {
                MetricSinkType.PrometheusScrapingEndpoint,
                MetricSinkType.AtlassianStatuspage,
                MetricSinkType.StatsD,
                MetricSinkType.OpenTelemetryCollector
            };
            var operatingSystem = DetermineOperatingSystem();
            var expectedUserAgent = string.Format(UserNameFormat, agentVersion, operatingSystem, EnabledText, EnabledText, EnabledText, EnabledText);

            // Act
            var userAgent = ArmUserAgent.Generate(agentVersion, enabledMetricSinks);

            // Assert
            Assert.Equal(expectedUserAgent, userAgent);
        }

        [Fact]
        public void Generate_AllSinksExceptPrometheusEnabled_ReturnsCorrectUserAgent()
        {
            // Arrange
            var agentVersion = "1.2.3";
            var enabledMetricSinks = new List<MetricSinkType>
            {
                MetricSinkType.AtlassianStatuspage,
                MetricSinkType.StatsD,
                MetricSinkType.OpenTelemetryCollector
            };
            var operatingSystem = DetermineOperatingSystem();
            var expectedUserAgent = string.Format(UserNameFormat, agentVersion, operatingSystem, DisabledText, EnabledText, EnabledText, EnabledText);

            // Act
            var userAgent = ArmUserAgent.Generate(agentVersion, enabledMetricSinks);

            // Assert
            Assert.Equal(expectedUserAgent, userAgent);
        }

        [Fact]
        public void Generate_NoMetricSinkTypesAreProvided_ThrowsArgumentException()
        {
            // Arrange
            var agentVersion = "1.2.3";
            var enabledMetricSinks = new List<MetricSinkType>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ArmUserAgent.Generate(agentVersion, enabledMetricSinks));
        }

        [Fact]
        public void Generate_MetricSinkTypesIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var agentVersion = "1.2.3";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ArmUserAgent.Generate(agentVersion, metricSinkInfo: null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Generate_AgentVersionIsInvalid_ThrowsArgumentException(string agentVersion)
        {
            // Arrange
            var enabledMetricSinks = new List<MetricSinkType>
            {
                MetricSinkType.AtlassianStatuspage,
                MetricSinkType.StatsD,
                MetricSinkType.OpenTelemetryCollector
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ArmUserAgent.Generate(agentVersion, enabledMetricSinks));
        }

        string DetermineOperatingSystem()
        {
            return RuntimeInformation.OSDescription.Contains("linux", StringComparison.InvariantCultureIgnoreCase) ? "Linux" : "Windows";
        }
    }
}