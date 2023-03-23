using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class TrafficManagerDeserializerTests : ResourceDeserializerTest<TrafficManagerDeserializer>
    {
        private readonly TrafficManagerDeserializer _deserializer;

        public TrafficManagerDeserializerTests()
        {
            _deserializer = new TrafficManagerDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_NameSupplied_SetsName()
        {
            const string name = "promitor-traffic-manager";
            YamlAssert.PropertySet<TrafficManagerResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"name: {name}",
                name,
                r => r.Name);
        }

        [Fact]
        public void Deserialize_NameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<TrafficManagerResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Name);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new TrafficManagerDeserializer(Logger);
        }
    }
}