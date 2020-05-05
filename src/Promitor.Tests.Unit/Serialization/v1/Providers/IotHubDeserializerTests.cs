using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class IotHubDeserializerTests : ResourceDeserializerTest<IotHubDeserializer>
    {
        private readonly IotHubDeserializer _deserializer;

        public IotHubDeserializerTests()
        {
            _deserializer = new IotHubDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_IotHubNameSupplied_SetsIotHubName()
        {
            const string iotHubName = "promitor-iot-hub";
            YamlAssert.PropertySet<IotHubResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"iotHubName: {iotHubName}",
                iotHubName,
                r => r.IotHubName);
        }

        [Fact]
        public void Deserialize_IotHubNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<IotHubResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.IotHubName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new IotHubDeserializer(Logger);
        }
    }
}