using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class GenericResourceDeserializerTests : ResourceDeserializerTest<GenericResourceDeserializer>
    {
        private readonly GenericResourceDeserializer _deserializer;

        public GenericResourceDeserializerTests()
        {
            _deserializer = new GenericResourceDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_FilterSupplied_SetsFilter()
        {
            YamlAssert.PropertySet<GenericResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "filter: EntityName eq 'orders'",
                "EntityName eq 'orders'",
                r => r.Filter);
        }

        [Fact]
        public void Deserialize_FilterNotSupplied_Null()
        {
            YamlAssert.PropertyNull<GenericResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Filter);
        }

        [Fact]
        public void Deserialize_ResourceUriSupplied_SetsResourceUri()
        {
            YamlAssert.PropertySet<GenericResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging",
                "Microsoft.ServiceBus/namespaces/promitor-messaging",
                r => r.ResourceUri);
        }

        [Fact]
        public void Deserialize_ResourceUriNotSupplied_Null()
        {
            YamlAssert.PropertyNull<GenericResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ResourceUri);
        }

        [Fact]
        public void Deserialize_ResourceUriNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "resourceUri");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new GenericResourceDeserializer(Logger);
        }
    }
}
