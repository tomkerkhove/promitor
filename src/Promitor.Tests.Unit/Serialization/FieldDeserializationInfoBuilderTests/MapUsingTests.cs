using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.FieldDeserializationInfoBuilderTests
{
    public class MapUsingTests : UnitTest
    {
        private readonly FieldDeserializationInfoBuilder<TestConfig, string> _builder =
            new FieldDeserializationInfoBuilder<TestConfig, string>();

        public MapUsingTests()
        {
            _builder.SetProperty(c => c.Name);
        }

        [Fact]
        public void MapUsing_UsesCustomMapper()
        {
            // Act
            _builder.MapUsing((name, _, _) => "Hello " + name);

            // Assert
            var fieldInfo = _builder.Build();
            var result = fieldInfo.CustomMapperFunc("Promitor", new KeyValuePair<YamlNode, YamlNode>(), null);
            Assert.Equal("Hello Promitor", result);
        }

        [Fact]
        public void MapUsing_ReturnsBuilder()
        {
            // Act
            var result = _builder.MapUsing((name, _, _) => "Hello " + name);

            // Assert
            Assert.Same(_builder, result);
        }
    }
}