using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class WebAppDeserializerTests : ResourceDeserializerTest<WebAppDeserializer>
    {
        private readonly WebAppDeserializer _deserializer;

        public WebAppDeserializerTests()
        {
            _deserializer = new WebAppDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_WebAppNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<WebAppResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "webAppName: promitor-web-app",
                "promitor-web-app",
                r => r.WebAppName);
        }

        [Fact]
        public void Deserialize_WebAppNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<WebAppResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.WebAppName);
        }

        [Fact]
        public void Deserialize_SlotNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<WebAppResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "slotName: staging",
                "staging",
                r => r.SlotName);
        }

        [Fact]
        public void Deserialize_SlotNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<WebAppResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.SlotName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new WebAppDeserializer(Logger);
        }
    }
}
