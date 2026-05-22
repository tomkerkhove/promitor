using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class PrivateLinkServiceDeserializerTests : ResourceDeserializerTest<PrivateLinkServiceDeserializer>
    {
        private readonly PrivateLinkServiceDeserializer _deserializer;

        public PrivateLinkServiceDeserializerTests()
        {
            _deserializer = new PrivateLinkServiceDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_PrivateLinkServiceNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<PrivateLinkServiceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "privateLinkServiceName: promitor-private-link-service",
                "promitor-private-link-service",
                r => r.PrivateLinkServiceName);
        }

        [Fact]
        public void Deserialize_PrivateLinkServiceNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<PrivateLinkServiceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.PrivateLinkServiceName);
        }

        [Fact]
        public void Deserialize_PrivateLinkServiceNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "privateLinkServiceName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new PrivateLinkServiceDeserializer(Logger);
        }
    }
}
