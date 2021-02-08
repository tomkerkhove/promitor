using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class FrontDoorDeserializerTests : ResourceDeserializerTest<FrontDoorDeserializer>
    {
        private readonly FrontDoorDeserializer _deserializer;

        public FrontDoorDeserializerTests()
        {
            _deserializer = new FrontDoorDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_NameSupplied_SetsName()
        {
            YamlAssert.PropertySet<FrontDoorResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "name: promitor-front-door",
                "promitor-front-door",
                r => r.Name);
        }

        [Fact]
        public void Deserialize_NameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<FrontDoorResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Name);
        }

        [Fact]
        public void Deserialize_NameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "name");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new FrontDoorDeserializer(Logger);
        }
    }
}
