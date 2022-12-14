using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class TrafficManagerProfileDeserializerTests : ResourceDeserializerTest<TrafficManagerProfileDeserializer>
    {
        private readonly TrafficManagerProfileDeserializer _deserializer;

        public TrafficManagerProfileDeserializerTests()
        {
            _deserializer = new TrafficManagerProfileDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ProfileNameSupplied_SetsProfileName()
        {
            const string profileName = "promitor-profile";
            YamlAssert.PropertySet<TrafficManagerProfileResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"profileName: {profileName}",
                profileName,
                r => r.ProfileName);
        }

        [Fact]
        public void Deserialize_ProfileNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<TrafficManagerProfileResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ProfileName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new TrafficManagerProfileDeserializer(Logger);
        }
    }
}