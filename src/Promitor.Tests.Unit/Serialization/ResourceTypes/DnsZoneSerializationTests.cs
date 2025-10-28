using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.ResourceTypes
{
    [Category("Unit")]
    public class DnsZoneSerializationTests : UnitTest
    {
        [Fact]
        public void SerializeAndDeserialize_DnsZoneResourceDefinition_RoundTrips()
        {
            // Arrange
            var definition = new DnsZoneResourceDefinition("sub-id","rg-dns","example.com");
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            };
            settings.Converters.Add(new StringEnumConverter());

            // Act
            var json = JsonConvert.SerializeObject(definition, settings);
            var roundTripped = JsonConvert.DeserializeObject<AzureResourceDefinition>(json, settings);

            // Assert
            var dnsDef = Assert.IsType<DnsZoneResourceDefinition>(roundTripped);
            Assert.Equal(definition.ZoneName, dnsDef.ZoneName);
            Assert.Equal(ResourceType.DnsZone, dnsDef.ResourceType);
        }
    }
}
