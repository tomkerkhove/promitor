using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class NetworkInterfaceDeserializerTests : ResourceDeserializerTest<NetworkInterfaceDeserializer>
    {
        private readonly NetworkInterfaceDeserializer _deserializer;

        public NetworkInterfaceDeserializerTests()
        {
            _deserializer = new NetworkInterfaceDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_NetworkInterfaceNameSupplied_SetsNetworkInterfaceName()
        {
            YamlAssert.PropertySet<NetworkInterfaceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "networkInterfaceName: promitor-nic",
                "promitor-nic",
                r => r.NetworkInterfaceName);
        }

        [Fact]
        public void Deserialize_NetworkInterfaceNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<NetworkInterfaceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.NetworkInterfaceName);
        }

        [Fact]
        public void Deserialize_NetworkInterfaceNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "networkInterfaceName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new NetworkInterfaceDeserializer(Logger);
        }
    }
}
