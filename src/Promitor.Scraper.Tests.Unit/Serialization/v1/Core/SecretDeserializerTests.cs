﻿using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class SecretDeserializerTests
    {
        private readonly SecretDeserializer _deserializer;

        public SecretDeserializerTests()
        {
            _deserializer = new SecretDeserializer(NullLogger<SecretDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_RawValueSupplied_SetsRawValue()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "rawValue: abc123",
                "abc123",
                s => s.RawValue);
        }

        [Fact]
        public void Deserialize_RawValueNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "environmentVariable: MY_VARIABLE",
                s => s.RawValue);
        }

        [Fact]
        public void Deserialize_EnvironmentVariableSupplied_SetsEnvironmentVariable()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "environmentVariable: PROMITOR_SECRET",
                "PROMITOR_SECRET",
                s => s.EnvironmentVariable);
        }

        [Fact]
        public void Deserialize_EnvironmentVariableNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "rawValue: abc123",
                s => s.EnvironmentVariable);
        }

        [Fact]
        public void Deserialize_EnvironmentVariableAndRawValueNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: 123");
            var errorReporter = new Mock<IErrorReporter>();

            // Act
            _deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            errorReporter.Verify(r => r.ReportError(node, "Either 'environmentVariable' or 'rawValue' must be supplied for a secret."));
        }

        [Fact]
        public void Deserialize_EnvironmentVariableAndRawValueBothSupplied_ReportsWarning()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"rawValue: 123
environmentVariable: PROMITOR_SECRET");
            var errorReporter = new Mock<IErrorReporter>();

            // Act
            _deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            errorReporter.Verify(r => r.ReportWarning(node, "Secret with environment variable 'PROMITOR_SECRET' also has a rawValue provided."));
        }
    }
}
