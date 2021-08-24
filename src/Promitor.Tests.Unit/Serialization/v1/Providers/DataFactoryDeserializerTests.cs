using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class DataFactoryDeserializerTests : ResourceDeserializerTest<DataFactoryDeserializer>
    {
        private readonly DataFactoryDeserializer _deserializer;

        public DataFactoryDeserializerTests()
        {
            _deserializer = new DataFactoryDeserializer(Logger);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new DataFactoryDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_FactoryNameSupplied_SetsContainerGroup()
        {
            YamlAssert.PropertySet<DataFactoryResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "factoryName: promitor-group",
                "promitor-group",
                c => c.FactoryName);
        }

        [Fact]
        public void Deserialize_ShareNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<DataFactoryResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-resource-group",
                c => c.FactoryName);
        }

        [Fact]
        public void Deserialize_ShareNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "factoryName");
        }
    }
}
