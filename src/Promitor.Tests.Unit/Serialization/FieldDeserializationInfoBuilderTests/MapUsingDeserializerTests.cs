using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.FieldDeserializationInfoBuilderTests
{
    public class MapUsingDeserializerTests
    {
        private readonly FieldDeserializationInfoBuilder<TestConfig, string> _builder =
            new FieldDeserializationInfoBuilder<TestConfig, string>();

        public MapUsingDeserializerTests()
        {
            _builder.SetProperty(c => c.Name);
        }

        [Fact]
        public void MapUsingDeserializer_SetsDeserializer()
        {
            // Arrange
            var deserializer = Mock.Of<IDeserializer>();

            // Act
            _builder.MapUsingDeserializer(deserializer);

            // Assert
            var fieldInfo = _builder.Build();
            Assert.Same(deserializer, fieldInfo.Deserializer);
        }

        [Fact]
        public void MapUsingDeserializer_ReturnsBuilder()
        {
            // Arrange
            var deserializer = Mock.Of<IDeserializer>();

            // Act
            var result = _builder.MapUsingDeserializer(deserializer);

            // Assert
            Assert.Same(_builder, result);
        }
    }
}