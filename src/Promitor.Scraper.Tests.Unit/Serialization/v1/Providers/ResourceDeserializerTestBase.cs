using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    public abstract class ResourceDeserializerTestBase
    {
        protected abstract IDeserializer<AzureResourceDefinitionV1> CreateDeserializer();

        [Fact]
        public void Deserialize_ResourceGroupNameSupplied_SetsResourceGroupName()
        {
            var deserializer = CreateDeserializer();

            DeserializerTestHelpers.AssertPropertySet(
                deserializer,
                "resourceGroupName: promitor-resource-group",
                "promitor-resource-group",
                c => c.ResourceGroupName);
        }

        [Fact]
        public void Deserialize_ResourceGroupNameNotSupplied_Null()
        {
            var deserializer = CreateDeserializer();

            DeserializerTestHelpers.AssertPropertyNull(
                deserializer,
                "someProperty: someValue",
                c => c.ResourceGroupName);
        }
    }
}
