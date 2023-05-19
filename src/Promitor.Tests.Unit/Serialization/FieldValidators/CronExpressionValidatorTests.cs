using System.Linq;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.FieldValidators;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.FieldValidators
{
    public class CronExpressionValidatorTests : UnitTest
    {
        private readonly CronExpressionValidator _validator = new();

        [Fact]
        public void Validate_InvalidExpression_ReportsError()
        {
            AssertExpressionIsNotValid("invalid-expression");
        }

        [Fact]
        public void Validate_ValidExpression_DoesNotReportError()
        {
            AssertExpressionIsValid("*/5 * * * *");
        }

        [Fact]
        public void Validate_ValidExpressionWithSeconds_DoesNotReportError()
        {
            AssertExpressionIsValid("* */5 * * * *");
        }

        [Fact]
        public void Validate_EmptyString_DoesNotReportError()
        {
            AssertExpressionIsValid(string.Empty);
        }

        private void AssertExpressionIsNotValid(string cronExpression)
        {
            // Arrange
            var fieldNodes = YamlUtils
                .CreateYamlNode($"schedule: '{cronExpression}'")
                .Children
                .First();
            var errorReporter = new Mock<IErrorReporter>();

            // Act
            _validator.Validate(fieldNodes.Value.ToString(), fieldNodes, errorReporter.Object);

            // Assert
            errorReporter.Verify(
                e => e.ReportError(fieldNodes.Value, $"'{cronExpression}' is not a valid value for 'schedule'. The value must be a valid Cron expression."));
        }

        private void AssertExpressionIsValid(string cronExpression)
        {
            // Arrange
            var fieldNodes = YamlUtils
                .CreateYamlNode($"schedule: '{cronExpression}'")
                .Children
                .First();
            var errorReporter = new Mock<IErrorReporter>();

            // Act
            _validator.Validate(fieldNodes.Value.ToString(), fieldNodes, errorReporter.Object);

            // Assert
            errorReporter.Verify(
                e => e.ReportError(It.IsAny<YamlNode>(), It.IsAny<string>()), Times.Never);
        }
    }
}