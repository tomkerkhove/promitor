using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Scraper.Tests.Unit.Serialization;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.DeserializerTests
{
    public class ValidationTests
    {
        private readonly Mock<IErrorReporter> errorReporter = new Mock<IErrorReporter>();
        private readonly TestDeserializer deserializer = new TestDeserializer();
        
        [Fact]
        public void Deserialize_RequiredFieldMissing_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("age: 20");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            errorReporter.Verify(r => r.ReportError(node, "'name' is a required field but was not found."));
        }
        
        [Fact]
        public void Deserialize_RequiredFieldProvided_DoesNotReportError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            errorReporter.Verify(
                r => r.ReportError(node, It.Is<string>(message => message.Contains("name"))), Times.Never);
        }
        
        [Fact]
        public void Deserialize_OptionalFieldMissing_DoesNotReportError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: Promitor");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            errorReporter.Verify(
                r => r.ReportError(It.IsAny<YamlNode>(), It.Is<string>(message => message.Contains("age"))), Times.Never);
        }

        [Fact]
        public void Deserialize_UnknownFields_ReportsWarnings()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"city: Glasgow
country: Scotland");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            var cityTagNode = node.Children.Single(c => c.Key.ToString() == "city").Key;
            var countryTagNode = node.Children.Single(c => c.Key.ToString() == "country").Key;
            
            errorReporter.Verify(r => r.ReportWarning(cityTagNode, "Unknown field 'city'."));
            errorReporter.Verify(r => r.ReportWarning(countryTagNode, "Unknown field 'country'."));
        }

        [Fact]
        public void Deserialize_EnumValueInvalid_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("day: Sundag");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            var dayValueNode = node.Children.Single(c => c.Key.ToString() == "day").Value;
            
            errorReporter.Verify(r => r.ReportError(dayValueNode, "'Sundag' is not a valid value for 'day'."));
        }

        [Fact]
        public void Deserialize_IntValueInvalid_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("age: twenty");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            var dayValueNode = node.Children.Single(c => c.Key.ToString() == "age").Value;
            
            errorReporter.Verify(r => r.ReportError(dayValueNode, $"'twenty' is not a valid value for 'age'. The value must be of type {typeof(int)}."));
        }

        [Fact]
        public void Deserialize_TimeSpanValueInvalid_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("interval: twenty");

            // Act
            var result = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            var dayValueNode = node.Children.Single(c => c.Key.ToString() == "interval").Value;
            
            errorReporter.Verify(r => r.ReportError(dayValueNode, $"'twenty' is not a valid value for 'interval'. The value must be in the format 'hh:mm:ss'."));
        }

        [Fact]
        public void Deserialize_IgnoreField_DoesNotReportErrorIfFieldFound()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("customField: 1234");

            // Act
            deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            errorReporter.Verify(
                r => r.ReportWarning(It.IsAny<YamlNode>(), It.Is<string>(s => s.Contains("customField"))), Times.Never);
        }

        // TODO: Check for invalid formats (crontab expression, timespan, etc)
        // TODO: Add a test to make sure that the error is against the mapping node rather than its value for missing required fields

        private class TestConfigObject
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DayOfWeek Day { get; set; }
            public TimeSpan Interval { get; set; }
        }

        private class TestDeserializer: Deserializer<TestConfigObject>
        {
            public TestDeserializer() : base(NullLogger.Instance)
            {
                MapRequired(t => t.Name);
                MapOptional(t => t.Age);
                MapOptional(t => t.Day);
                MapOptional(t => t.Interval);
                IgnoreField("customField");
            }
        }
    }
}