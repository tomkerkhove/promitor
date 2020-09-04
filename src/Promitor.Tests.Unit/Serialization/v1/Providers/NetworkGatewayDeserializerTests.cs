using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class NetworkGatewayDeserializerTests : ResourceDeserializerTest<NetworkGatewayDeserializer>
    {
        private readonly NetworkGatewayDeserializer _deserializer;

        public NetworkGatewayDeserializerTests()
        {
            _deserializer = new NetworkGatewayDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_NetworkGatewayNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<NetworkGatewayResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "networkGatewayName: promitor-network-gateway",
                "promitor-network-gateway",
                r => r.NetworkGatewayName);
        }

        [Fact]
        public void Deserialize_NetworkGatewayNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<NetworkGatewayResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.NetworkGatewayName);
        }

        [Fact]
        public void Deserialize_NetworkGatewayNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "networkGatewayName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new NetworkGatewayDeserializer(Logger);
        }
    }
}
