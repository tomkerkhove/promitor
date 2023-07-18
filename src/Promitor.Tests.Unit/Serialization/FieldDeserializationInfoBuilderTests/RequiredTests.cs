using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.FieldDeserializationInfoBuilderTests
{
    public class RequiredTests : UnitTest
    {
        private readonly FieldDeserializationInfoBuilder<TestConfig, string> _builder = new();

        public RequiredTests()
        {
            _builder.SetProperty(c => c.Name);
        }

        [Fact]
        public void IsRequired_SetsFieldAsRequired()
        {
            // Act
            _builder.IsRequired();

            // Assert
            var fieldInfo = _builder.Build();
            Assert.True(fieldInfo.IsRequired);
        }

        [Fact]
        public void Build_DefaultsIsRequiredToFalse()
        {
            // Act
            var fieldInfo = _builder.Build();

            // Assert
            Assert.False(fieldInfo.IsRequired);
        }

        [Fact]
        public void IsRequired_ReturnsBuilder()
        {
            // Act
            var result = _builder.IsRequired();

            // Assert
            Assert.Same(_builder, result);
        }
    }
}