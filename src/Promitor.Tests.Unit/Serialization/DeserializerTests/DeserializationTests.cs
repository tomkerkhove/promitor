using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.DeserializerTests
{
    public class DeserializationTests : UnitTest
    {
        private static readonly TimeSpan defaultInterval = TimeSpan.FromMinutes(5);

        private readonly Mock<IErrorReporter> _errorReporter = new();
        private readonly Mock<IDeserializer<object>> _childDeserializer = new();
        private readonly RegistrationConfigDeserializer _deserializer;

        public DeserializationTests()
        {
            _deserializer = new RegistrationConfigDeserializer(_childDeserializer.Object);
        }

        [Fact]
        public void Deserialize_RequiredFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal("Promitor", result.Name);
        }

        [Fact]
        public void Deserialize_OptionalFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("town: Glasgow");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal("Glasgow", result.Town);
        }

        [Fact]
        public void Deserialize_StringFieldNotSupplied_Null()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("age: 17");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Null(result.Name);
        }

        [Fact]
        public void Deserialize_IntFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("age: 22");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(22, result.Age);
        }

        [Fact]
        public void Deserialize_IntFieldNotSupplied_SetsFieldTo0()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(0, result.Age);
        }

        [Fact]
        public void Deserialize_EnumFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("day: Monday");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(DayOfWeek.Monday, result.Day);
        }

        [Fact]
        public void Deserialize_NullableEnumFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("nullableDay: Monday");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(DayOfWeek.Monday, result.NullableDay);
        }

        [Fact]
        public void Deserialize_EnumFieldNotSupplied_SetsDefault()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(default, result.Day);
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
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

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
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(new TimeSpan(1, 2, 3), result.Interval);
        }

        [Fact]
        public void Deserialize_MapOptional_CanSpecifyDefaultValue()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(defaultInterval, result.DefaultedInterval);
        }

        [Fact]
        public void Deserialize_NullableTimeSpanFieldSupplied_SetsField()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("nullableInterval: 01:02:03");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(new TimeSpan(1, 2, 3), result.NullableInterval);
        }

        [Fact]
        public void Deserialize_CustomMappingFunctionSupplied_UsesCustomMappingFunction()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("invertedProperty: false");

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.True(result.InvertedProperty);
        }

        [Fact]
        public void Deserialize_RequiredChildObject_CanUseChildDeserializer()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"child:
    childProperty: 123");
            var child = new ChildConfig();
            _childDeserializer.Setup(
                d => d.Deserialize((YamlMappingNode)node.Children["child"], _errorReporter.Object)).Returns(child);

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Same(child, result.Child);
        }

        [Fact]
        public void Deserialize_OptionalChildObject_CanUseChildDeserializer()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"optionalChild:
    childProperty: 123");
            var child = new ChildConfig();
            _childDeserializer.Setup(
                d => d.Deserialize((YamlMappingNode)node.Children["optionalChild"], _errorReporter.Object)).Returns(child);

            // Act
            var result = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Same(child, result.OptionalChild);
        }

        public class RegistrationConfig
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
            public ChildConfig Child { get; set; }
            public ChildConfig OptionalChild { get; set; }
        }

        public class ChildConfig
        {
            public string ChildProperty { get; set; }
        }

        private class RegistrationConfigDeserializer: Deserializer<RegistrationConfig>
        {
            public RegistrationConfigDeserializer(IDeserializer<object> childDeserializer) : base(NullLogger.Instance)
            {
                Map(t => t.Name).IsRequired();
                Map(t => t.Age).IsRequired();
                Map(t => t.Day).IsRequired();
                Map(t => t.NullableDay);
                Map(t => t.Classes).IsRequired();
                Map(t => t.Town);
                Map(t => t.Interval);
                Map(t => t.DefaultedInterval).WithDefault(defaultInterval);
                Map(t => t.NullableInterval);
                Map(t => t.InvertedProperty)
                    .MapUsing(InvertBooleanString);
                Map(t => t.Child)
                    .IsRequired()
                    .MapUsingDeserializer(childDeserializer);
                Map(t => t.OptionalChild)
                    .MapUsingDeserializer(childDeserializer);
            }

            private static object InvertBooleanString(string value, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
            {
                var boolValue = bool.Parse(value);

                return !boolValue;
            }
        }
    }
}