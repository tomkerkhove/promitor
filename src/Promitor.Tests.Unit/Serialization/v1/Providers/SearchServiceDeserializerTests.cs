
using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class SearchServiceDeserializerTests : ResourceDeserializerTest<SearchServiceDeserializer>
    {
        private readonly SearchServiceDeserializer _deserializer;

        public SearchServiceDeserializerTests()
        {
            _deserializer = new SearchServiceDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_SearchServiceNameSupplied_SetsSearchServiceName()
        {
            YamlAssert.PropertySet<SearchServiceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "searchServiceName: promitor-search-service",
                "promitor-search-service",
                r => r.SearchServiceName);
        }

        [Fact]
        public void Deserialize_SearchServiceNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SearchServiceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.SearchServiceName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new SearchServiceDeserializer(Logger);
        }
    }
}
