using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class NatGatewayDeserializerTests : ResourceDeserializerTest<NatGatewayDeserializer>
    {
        private readonly NatGatewayDeserializer _deserializer;

        public NatGatewayDeserializerTests()
        {
            _deserializer = new NatGatewayDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_NatGatewayNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<NatGatewayResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "natGatewayName: promitor-nat-gateway",
                "promitor-nat-gateway",
                r => r.NatGatewayName);
        }

        [Fact]
        public void Deserialize_NatGatewayNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<NatGatewayResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.NatGatewayName);
        }

        [Fact]
        public void Deserialize_NatGatewayNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "natGatewayName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new NatGatewayDeserializer(Logger);
        }
    }
}
