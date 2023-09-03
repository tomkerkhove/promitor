
using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class PowerBiDedicatedDeserializerTests : ResourceDeserializerTest<PowerBiDedicatedDeserializer>
	{
        private readonly PowerBiDedicatedDeserializer _deserializer;

	public PowerBiDedicatedDeserializerTests()
	{
            _deserializer = new PowerBiDedicatedDeserializer(Logger);
	}

        [Fact]
        public void Deserialize_CapacityNameSupplied_SetsCapacityName()
        {
            YamlAssert.PropertySet<PowerBiDedicatedResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "capacityName: promitor-cap",
                "promitor-cap",
                r => r.CapacityName);
        }

        [Fact]
        public void Deserialize_CapacityNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<PowerBiDedicatedResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-cap",
                r => r.CapacityName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new PowerBiDedicatedDeserializer(Logger);
        }
    }
}
