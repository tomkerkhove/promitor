using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ContainerRegistryDeserializerTests : ResourceDeserializerTest<ContainerRegistryDeserializer>
    {
        private readonly ContainerRegistryDeserializer _deserializer;

        public ContainerRegistryDeserializerTests()
        {
            _deserializer = new ContainerRegistryDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_RegistryNameSupplied_SetsRegistryName()
        {
            YamlAssert.PropertySet<ContainerRegistryResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "registryName: promitor-registry",
                "promitor-registry",
                c => c.RegistryName);
        }

        [Fact]
        public void Deserialize_RegistryNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ContainerRegistryResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.RegistryName);
        }

        [Fact]
        public void Deserialize_RegistryNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "registryName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ContainerRegistryDeserializer(Logger);
        }
    }
}
