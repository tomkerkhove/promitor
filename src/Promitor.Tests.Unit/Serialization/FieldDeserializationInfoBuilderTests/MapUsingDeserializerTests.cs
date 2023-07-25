using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.FieldDeserializationInfoBuilderTests
{
    public class MapUsingDeserializerTests : UnitTest
    {
        private readonly FieldDeserializationInfoBuilder<TestConfig, string> _builder = new();

        public MapUsingDeserializerTests()
        {
            _builder.SetProperty(c => c.Name);
        }

        [Fact]
        public void MapUsingDeserializer_SetsDeserializer()
        {
            // Arrange
            var deserializer = Mock.Of<IDeserializer<object>>();

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
            var deserializer = Mock.Of<IDeserializer<object>>();

            // Act
            var result = _builder.MapUsingDeserializer(deserializer);

            // Assert
            Assert.Same(_builder, result);
        }
    }
}