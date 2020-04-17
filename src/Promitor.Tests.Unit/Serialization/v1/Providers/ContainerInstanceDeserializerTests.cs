using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ContainerInstanceDeserializerTests : ResourceDeserializerTest<ContainerInstanceDeserializer>
    {
        private readonly ContainerInstanceDeserializer _deserializer;

        public ContainerInstanceDeserializerTests()
        {
            _deserializer = new ContainerInstanceDeserializer(Logger);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ContainerInstanceDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ContainerGroupSupplied_SetsContainerGroup()
        {
            YamlAssert.PropertySet<ContainerInstanceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "containerGroup: promitor-group",
                "promitor-group",
                c => c.ContainerGroup);
        }

        [Fact]
        public void Deserialize_ContainerGroupNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ContainerInstanceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-resource-group",
                c => c.ContainerGroup);
        }

        [Fact]
        public void Deserialize_ContainerGroupNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "containerGroup");
        }
    }
}
