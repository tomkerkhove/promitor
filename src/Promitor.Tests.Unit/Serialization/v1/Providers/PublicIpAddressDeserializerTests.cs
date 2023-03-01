using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class PublicIpAddressDeserializerTests : ResourceDeserializerTest<PublicIpAddressDeserializer>
    {
        private readonly PublicIpAddressDeserializer _deserializer;

        public PublicIpAddressDeserializerTests()
        {
            _deserializer = new PublicIpAddressDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_PublicIpAddressNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<PublicIpAddressResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "publicIpAddressName: promitor-ip-address",
                "promitor-ip-address",
                r => r.PublicIpAddressName);
        }

        [Fact]
        public void Deserialize_PublicIpAddressNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<PublicIpAddressResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.PublicIpAddressName);
        }

        [Fact]
        public void Deserialize_PublicIpAddressNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "publicIpAddressName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new PublicIpAddressDeserializer(Logger);
        }
    }
}
