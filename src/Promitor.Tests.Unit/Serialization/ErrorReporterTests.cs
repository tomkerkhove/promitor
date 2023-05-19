using System;
using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;

namespace Promitor.Tests.Unit.Serialization
{
    [Category("Unit")]
    public class ErrorReporterTests : UnitTest
    {
        private readonly ErrorReporter _errorReporter = new();

        [Fact]
        public void ReportError_AfterErrorReported_AddsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: promitor");

            // Act
            _errorReporter.ReportError(node, "Test error message");

            // Assert
            Assert.Collection(_errorReporter.Messages,
                m =>
                {
                    Assert.Equal(node, m.Node);
                    Assert.Equal(MessageType.Error, m.MessageType);
                    Assert.Equal("Test error message", m.Message);
                });
        }

        [Fact]
        public void ReportError_NullNode_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _errorReporter.ReportError(null, "Test error"));
        }

        [Fact]
        public void ReportError_NullMessage_ThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => _errorReporter.ReportError(YamlUtils.CreateYamlNode("name: promitor"), null));
        }

        [Fact]
        public void ReportError_EmptyMessage_ThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => _errorReporter.ReportError(YamlUtils.CreateYamlNode("name: promitor"), string.Empty));
        }

        [Fact]
        public void ReportWarning_NullNode_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _errorReporter.ReportWarning(null, "Test warning"));
        }

        [Fact]
        public void ReportWarning_NullMessage_ThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => _errorReporter.ReportWarning(YamlUtils.CreateYamlNode("name: promitor"), null));
        }

        [Fact]
        public void ReportWarning_EmptyMessage_ThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => _errorReporter.ReportWarning(YamlUtils.CreateYamlNode("name: promitor"), string.Empty));
        }

        [Fact]
        public void ReportWarning_AfterWarningReported_AddsWarning()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: promitor");

            // Act
            _errorReporter.ReportWarning(node, "Test warning message");

            // Assert
            Assert.Collection(_errorReporter.Messages,
                m =>
                {
                    Assert.Equal(node, m.Node);
                    Assert.Equal(MessageType.Warning, m.MessageType);
                    Assert.Equal("Test warning message", m.Message);
                });
        }

        [Fact]
        public void HasErrors_AfterErrorReported_True()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: promitor");
            _errorReporter.ReportError(node, "Test error message");

            // Act
            var hasErrors = _errorReporter.HasErrors;

            // Assert
            Assert.True(hasErrors);
        }

        [Fact]
        public void HasErrors_AfterWarningReported_False()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: promitor");
            _errorReporter.ReportWarning(node, "Test warning");

            // Act
            var hasErrors = _errorReporter.HasErrors;

            // Assert
            Assert.False(hasErrors);
        }

        [Fact]
        public void Messages_MultipleMessagesAdded_ReturnedInOrderOfLineNumber()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"firstNode: one
secondNode: two
thirdNode: three");

            _errorReporter.ReportWarning(node.Children["secondNode"], "second");
            _errorReporter.ReportError(node.Children["firstNode"], "first");
            _errorReporter.ReportWarning(node.Children["thirdNode"], "third");

            // Act
            var messages = _errorReporter.Messages;

            // Assert
            Assert.Collection(messages,
                m => Assert.Equal("first", m.Message),
                m => Assert.Equal("second", m.Message),
                m => Assert.Equal("third", m.Message));
        }
    }
}