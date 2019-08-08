using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Providers
{
    public abstract class ResourceDeserializerTestBase
    {
        protected abstract IDeserializer<AzureResourceDefinitionV2> CreateDeserializer();

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
