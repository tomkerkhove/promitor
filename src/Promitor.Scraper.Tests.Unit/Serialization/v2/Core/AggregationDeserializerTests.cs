using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Core
{
    [Category("Unit")]
    public class AggregationDeserializerTests
    {
        private readonly AggregationDeserializer _deserializer;

        public AggregationDeserializerTests()
        {
            var logger = new Mock<ILogger>();

            _deserializer = new AggregationDeserializer(logger.Object);
        }

        [Fact]
        public void Deserialize_IntervalSupplied_SetsInterval()
        {
            // Arrange
            const string yamlText =
@"aggregation:
    interval: 00:07:00";
            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                yamlText,
                "aggregation",
                TimeSpan.FromMinutes(7),
                a => a.Interval);
        }

        [Fact]
        public void Deserialize_IntervalNotSupplied_UsesDefault()
        {
            const string yamlText =
@"aggregation:
    someProperty: someValue";
            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                yamlText,
                "aggregation",
                TimeSpan.FromMinutes(5),
                a => a.Interval);
        }
    }
}
