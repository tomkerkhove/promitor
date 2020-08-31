using System;
using System.ComponentModel;
using Xunit;

namespace Promitor.Tests.Unit.Prometheus
{
    [Category("Unit")]
    public class StringExtensionTests
    {
        [Fact]
        public void SanitizeForPrometheusLabelKey_ValidKey_IdenticalOutput()
        {
            // Arrange
            var labelKeyInput = "sample_label";

            // Act
            var sanitizedLabelKey = labelKeyInput.SanitizeForPrometheusLabelKey();

            // Assert
            Assert.Equal(labelKeyInput, sanitizedLabelKey);
        }

        [Fact]
        public void SanitizeForPrometheusLabelKey_KeyIsUppercase_ConvertedToLowercase()
        {
            // Arrange
            var labelKeyInput = "SAMPLE_LABEL";
            var expectedLabelKey = "sample_label";

            // Act
            var sanitizedLabelKey = labelKeyInput.SanitizeForPrometheusLabelKey();

            // Assert
            Assert.Equal(expectedLabelKey, sanitizedLabelKey);
        }

        [Fact]
        public void SanitizeForPrometheusLabelKey_KeyWithSlash_SlashIsReplaced()
        {
            // Arrange
            var labelKeyInput = "sample/label";
            var expectedLabelKey = "sample_label";

            // Act
            var sanitizedLabelKey = labelKeyInput.SanitizeForPrometheusLabelKey();

            // Assert
            Assert.Equal(expectedLabelKey, sanitizedLabelKey);
        }
    }
}