using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Core
{
    [Category("Unit")]
    public class SecretDeserializerTests
    {
        private readonly SecretDeserializer _deserializer;

        public SecretDeserializerTests()
        {
            _deserializer = new SecretDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_RawValueSupplied_SetsRawValue()
        {
            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                "rawValue: abc123",
                "abc123",
                s => s.RawValue);
        }

        [Fact]
        public void Deserialize_RawValueNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull(
                _deserializer,
                "environmentVariable: MY_VARIABLE",
                s => s.RawValue);
        }

        [Fact]
        public void Deserialize_EnvironmentVariableSupplied_SetsEnvironmentVariable()
        {
            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                "environmentVariable: PROMITOR_SECRET",
                "PROMITOR_SECRET",
                s => s.EnvironmentVariable);
        }

        [Fact]
        public void Deserialize_EnvironmentVariableNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull(
                _deserializer,
                "rawValue: abc123",
                s => s.EnvironmentVariable);
        }
    }
}
