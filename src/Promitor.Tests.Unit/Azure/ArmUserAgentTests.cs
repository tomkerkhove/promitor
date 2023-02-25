using System;
using System.Collections.Generic;
using System.ComponentModel;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.AzureMonitor;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class ArmUserAgentTests : UnitTest
    {
        private const string UserNameFormat = "App/Promitor Agent/Scraper Version/{0} Prometheus/{1} OpenTelemetryCollector/{2} StatsD/{3} AtlassianStatuspage/{4}";
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
            var expectedUserAgent = string.Format(UserNameFormat, agentVersion, EnabledText, EnabledText, EnabledText, EnabledText);

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
            var expectedUserAgent = string.Format(UserNameFormat, agentVersion, DisabledText, EnabledText, EnabledText, EnabledText);

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
    }
}