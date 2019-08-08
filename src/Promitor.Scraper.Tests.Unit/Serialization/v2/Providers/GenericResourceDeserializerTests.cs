using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Providers
{
    public class GenericResourceDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly GenericResourceDeserializer _deserializer;

        public GenericResourceDeserializerTests()
        {
            _deserializer = new GenericResourceDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_FilterSupplied_SetsFilter()
        {
            DeserializerTestHelpers.AssertPropertySet<GenericResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "filter: EntityName eq 'orders'",
                "EntityName eq 'orders'",
                r => r.Filter);
        }

        [Fact]
        public void Deserialize_FilterNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<GenericResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Filter);
        }

        [Fact]
        public void Deserialize_ResourceUriSupplied_SetsResourceUri()
        {
            DeserializerTestHelpers.AssertPropertySet<GenericResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging",
                "Microsoft.ServiceBus/namespaces/promitor-messaging",
                r => r.ResourceUri);
        }

        [Fact]
        public void Deserialize_ResourceUriNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<GenericResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ResourceUri);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new GenericResourceDeserializer(new Mock<ILogger>().Object);
        }
    }
}
