using System;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.FieldDeserializationInfoBuilderTests
{
    public class SetPropertyTests : UnitTest
    {
        private readonly FieldDeserializationInfoBuilder<TestConfig, string> _builder = new();

        [Fact]
        public void SetProperty_SetsPropertyInfo_WhenBuilding()
        {
            // Arrange
            _builder.SetProperty(c => c.Name);

            // Act
            var fieldInfo = _builder.Build();

            // Assert
            var nameProperty = typeof(TestConfig).GetProperty(nameof(TestConfig.Name));
            Assert.Same(nameProperty, fieldInfo.PropertyInfo);
        }

        [Fact]
        public void SetProperty_ThrowsException_IfExpressionNotForProperty()
        {
            // Act / Assert
            Assert.Throws<ArgumentException>(() => _builder.SetProperty(c => c.GetName()));
        }
    }
}