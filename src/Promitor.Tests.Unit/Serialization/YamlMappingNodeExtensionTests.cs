using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;

namespace Promitor.Tests.Unit.Serialization
{
    [Category("Unit")]
    public class YamlMappingNodeExtensionTests
    {
        [Fact]
        public void GetString_PropertySpecified_ReturnsValue()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("property: value");

            // Act
            var value = node.GetString("property");

            // Assert
            Assert.Equal("value", value);
        }

        [Fact]
        public void GetString_PropertyNotSpecified_Null()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("otherProperty: value");

            // Act
            var value = node.GetString("property");

            // Assert
            Assert.Null(value);
        }

        [Fact]
        public void GetEnum_PropertySpecified_ReturnsValue()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("day: Wednesday");

            // Act
            var value = node.GetEnum<DayOfWeek>("day");

            // Assert
            Assert.Equal(DayOfWeek.Wednesday, value);
        }

        [Fact]
        public void GetEnum_PropertyNotSpecified_Null()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("month: January");

            // Act
            var value = node.GetEnum<DayOfWeek>("day");

            // Assert
            Assert.Null(value);
        }

        [Fact]
        public void GetEnum_EnumValueNotValid_ThrowsException()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("day: MonFriday");

            // Act / Assert
            Assert.Throws<ArgumentException>(() => node.GetEnum<DayOfWeek>("day"));
        }

        [Fact]
        public void GetDictionary_PropertySpecified_DeserializesDictionary()
        {
            // Arrange
            const string yamlText =
@"labels:
    env: production
    tier: web";
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var labels = node.GetDictionary("labels");

            // Assert
            var expected = new Dictionary<string, string>
            {
                { "env", "production" },
                { "tier", "web" }
            };

            Assert.Equal(expected, labels);
        }

        [Fact]
        public void GetDictionary_PropertyNotSpecified_Null()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("property: value");

            // Act
            var labels = node.GetDictionary("labels");

            // Assert
            Assert.Null(labels);
        }

        [Fact]
        public void GetTimeSpan_PropertySpecified_ReturnsValue()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("time: 13:08:12");

            // Act
            var value = node.GetTimeSpan("time");

            // Assert
            Assert.Equal(new TimeSpan(0, 13, 8, 12), value);
        }

        [Fact]
        public void GetTimeSpan_PropertyNotSpecified_Null()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("day: Monday");

            // Act
            var value = node.GetTimeSpan("time");

            // Assert
            Assert.Null(value);
        }

        [Fact]
        public void DeserializeChild_PropertySpecified_DeserializesChild()
        {
            // Arrange
            var errorReporter = new Mock<IErrorReporter>();
            const string yamlText =
@"aggregation:
    interval: 00:05:00";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var deserializer = new AggregationDeserializer(NullLogger<AggregationDeserializer>.Instance);

            // Act
            var aggregation = node.DeserializeChild("aggregation", deserializer, errorReporter.Object);

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(5), aggregation.Interval);
        }

        [Fact]
        public void DeserializeChild_PropertyNotSpecified_Null()
        {
            // Arrange
            var errorReporter = new Mock<IErrorReporter>();
            var node = YamlUtils.CreateYamlNode(@"time: 00:05:30");
            var deserializer = new AggregationDeserializer(NullLogger<AggregationDeserializer>.Instance);

            // Act
            var aggregation = node.DeserializeChild("aggregation", deserializer, errorReporter.Object);

            // Assert
            Assert.Null(aggregation);
        }
    }
}
