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
            var node = (YamlMappingNode)YamlUtils.CreateYamlNode(yamlText).Children["aggregation"];

            // Act
            var aggregation = _deserializer.Deserialize(node);

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(7), aggregation.Interval);
        }

        [Fact]
        public void Deserialize_IntervalNotSupplied_UsesDefault()
        {
            const string yamlText =
@"aggregation:
    someProperty: someValue";
            var node = (YamlMappingNode)YamlUtils.CreateYamlNode(yamlText).Children["aggregation"];

            // Act
            var aggregation = _deserializer.Deserialize(node);

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(5), aggregation.Interval);
        }
    }
}
