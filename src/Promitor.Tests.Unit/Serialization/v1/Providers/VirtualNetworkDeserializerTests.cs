using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class VirtualNetworkDeserializerTests : ResourceDeserializerTest<VirtualNetworkDeserializer>
    {
        private readonly VirtualNetworkDeserializer _deserializer;

        public VirtualNetworkDeserializerTests()
        {
            _deserializer = new VirtualNetworkDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_VirtualNetworkNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<VirtualNetworkResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "virtualNetworkName: promitor-vnet",
                "promitor-vnet",
                r => r.VirtualNetworkName);
        }

        [Fact]
        public void Deserialize_VirtualNetworkNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<VirtualNetworkResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.VirtualNetworkName);
        }

        [Fact]
        public void Deserialize_VirtualNetworkNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "virtualNetworkName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new VirtualNetworkDeserializer(Logger);
        }
    }
}
