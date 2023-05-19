using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.FieldDeserializationInfoBuilderTests
{
    public class DefaultTests : UnitTest
    {
        private readonly FieldDeserializationInfoBuilder<TestConfig, string> _builder = new();

        public DefaultTests()
        {
            _builder.SetProperty(c => c.Name);
        }

        [Fact]
        public void WithDefault_SetsDefaultValueForField()
        {
            // Act
            _builder.WithDefault("Promitor");

            // Assert
            var fieldInfo = _builder.Build();
            Assert.Equal("Promitor", fieldInfo.DefaultValue);
        }

        [Fact]
        public void Build_WithDefaultNotCalled_UsesDefaultValueForType()
        {
            // Act
            var fieldInfo = _builder.Build();

            // Assert
            Assert.Equal(default(string), fieldInfo.DefaultValue);
        }

        [Fact]
        public void WithDefault_ReturnsBuilder()
        {
            // Act
            var result = _builder.WithDefault("abc123");

            // Assert
            Assert.Same(_builder, result);
        }
    }
}