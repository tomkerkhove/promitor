using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.DeserializerTests
{
    public class DeserializationTests
    {
        private static readonly TimeSpan DefaultInterval = TimeSpan.FromMinutes(5);

        private readonly Mock<IErrorReporter> errorReporter = new Mock<IErrorReporter>();
        private readonly RegistrationConfigDeserializer deserializer = new RegistrationConfigDeserializer();

        [Fact]
        public void Deserialize_RequiredFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal("Promitor", result.Name);
        }

        [Fact]
        public void Deserialize_OptionalFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("town: Glasgow");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal("Glasgow", result.Town);
        }

        [Fact]
        public void Deserialize_StringFieldNotSupplied_Null()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("age: 17");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Null(result.Name);
        }

        [Fact]
        public void Deserialize_IntFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("age: 22");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(22, result.Age);
        }

        [Fact]
        public void Deserialize_IntFieldNotSupplied_SetsFieldTo0()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(0, result.Age);
        }

        [Fact]
        public void Deserialize_EnumFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("day: Monday");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(DayOfWeek.Monday, result.Day);
        }

        [Fact]
        public void Deserialize_NullableEnumFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("nullableDay: Monday");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(DayOfWeek.Monday, result.NullableDay);
        }

        [Fact]
        public void Deserialize_EnumFieldNotSupplied_SetsDefault()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(default(DayOfWeek), result.Day);
        }

        [Fact]
        public void Deserialize_DictionarySupplied_DeserializesDictionary()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"classes:
    first: maths
    second: chemistry
    third: art");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            var expectedClasses = new Dictionary<string, string>
            {
                { "first", "maths" },
                { "second", "chemistry" },
                { "third", "art" }
            };
            Assert.Equal(expectedClasses, result.Classes);
        }

        [Fact]
        public void Deserialize_TimeSpanFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("interval: 01:02:03");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(new TimeSpan(1, 2, 3), result.Interval);
        }

        [Fact]
        public void Deserialize_MapOptional_CanSpecifyDefaultValue()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(DefaultInterval, result.DefaultedInterval);
        }

        [Fact]
        public void Deserialize_NullableTimeSpanFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("nullableInterval: 01:02:03");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(new TimeSpan(1, 2, 3), result.NullableInterval);
        }

        [Fact]
        public void Deserialize_CustomMappingFunctionSupplied_UsesCustomMappingFunction()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("invertedProperty: false");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.True(result.InvertedProperty);
        }

        // TODO: Test for duplicate field registration

        private class RegistrationConfig
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DayOfWeek Day { get; set; }
            public DayOfWeek? NullableDay { get; set; }
            public Dictionary<string, string> Classes { get; set; }
            public string Town { get; set; }
            public TimeSpan Interval { get; set; }
            public TimeSpan DefaultedInterval { get; set; }
            public TimeSpan? NullableInterval { get; set; }
            public bool InvertedProperty { get; set; }
        }

        private class RegistrationConfigDeserializer: Deserializer<RegistrationConfig>
        {
            public RegistrationConfigDeserializer() : base(NullLogger.Instance)
            {
                MapRequired(t => t.Name);
                MapRequired(t => t.Age);
                MapRequired(t => t.Day);
                MapOptional(t => t.NullableDay);
                MapRequired(t => t.Classes);
                MapOptional(t => t.Town);
                MapOptional(t => t.Interval);
                MapOptional(t => t.DefaultedInterval, DefaultInterval);
                MapOptional(t => t.NullableInterval);
                MapOptional(t => t.InvertedProperty, false, InvertBooleanString);
            }

            private static object InvertBooleanString(string value, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
            {
                var boolValue = bool.Parse(value);

                return !boolValue;
            }
        }
    }
}