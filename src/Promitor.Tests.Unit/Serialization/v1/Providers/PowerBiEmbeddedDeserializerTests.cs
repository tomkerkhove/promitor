
using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class PowerBiEmbeddedDeserializerTests : ResourceDeserializerTest<PowerBiEmbeddedDeserializer>
	{
        private readonly PowerBiEmbeddedDeserializer _deserializer;

		public PowerBiEmbeddedDeserializerTests()
		{
            _deserializer = new PowerBiEmbeddedDeserializer(Logger);
		}

        [Fact]
        public void Deserialize_CapacityNameSupplied_SetsCapacityName()
        {
            YamlAssert.PropertySet<PowerBiEmbeddedResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "capacityName: promitor-cap",
                "promitor-cap",
                r => r.CapacityName);
        }

        [Fact]
        public void Deserialize_CapacityNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<PowerBiEmbeddedResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-cap",
                r => r.CapacityName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new PowerBiEmbeddedDeserializer(Logger);
        }
    }
}