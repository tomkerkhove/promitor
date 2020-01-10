using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class FunctionAppDeserializerTests : ResourceDeserializerTest<FunctionAppDeserializer>
    {
        private readonly FunctionAppDeserializer _deserializer;

        public FunctionAppDeserializerTests()
        {
            _deserializer = new FunctionAppDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_FunctionAppNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<FunctionAppResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "functionAppName: promitor-function-app",
                "promitor-function-app",
                r => r.FunctionAppName);
        }

        [Fact]
        public void Deserialize_FunctionAppNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<FunctionAppResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.FunctionAppName);
        }

        [Fact]
        public void Deserialize_SlotNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<FunctionAppResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "slotName: staging",
                "staging",
                r => r.SlotName);
        }

        [Fact]
        public void Deserialize_SlotNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<FunctionAppResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.SlotName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new FunctionAppDeserializer(Logger);
        }
    }
}
