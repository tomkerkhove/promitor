using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class AggregationDeserializerTests
    {
        private readonly AggregationDeserializer _deserializer;

        public AggregationDeserializerTests()
        {
            _deserializer = new AggregationDeserializer(NullLogger<AggregationDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_IntervalSupplied_SetsInterval()
        {
            // Arrange
            const string yamlText =
@"aggregation:
    interval: 00:07:00";
            YamlAssert.PropertySet(
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
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "aggregation",
                TimeSpan.FromMinutes(5),
                a => a.Interval);
        }
    }
}
