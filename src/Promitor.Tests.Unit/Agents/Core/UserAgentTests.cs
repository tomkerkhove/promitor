using System;
using Promitor.Core;
using Xunit;

namespace Promitor.Tests.Unit.Agents.Core
{
    public class UserAgentTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Generate_NoNameProvided_ThrowsException(string agentName)
        {
            // Arrange
            var version = "1.2.3";

            // Act & Assert
            Assert.Throws< ArgumentException>(() => UserAgent.Generate(agentName, version));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Generate_NoVersionProvided_ThrowsException(string version)
        {
            // Arrange
            var agentName = "Scraper";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => UserAgent.Generate(agentName, version));
        }

        [Fact]
        public void Generate_ValidInput_ThrowsException()
        {
            // Arrange
            var agentName = "Scraper";
            var version = "1.2.3";
            var expectedUserAgent = $"App/Promitor Agent/{agentName} Version/{version}";

            // Act
            var generatedUserAgent = UserAgent.Generate(agentName, version);

            // Assert
            Assert.Equal(expectedUserAgent, generatedUserAgent);
        }
    }
}