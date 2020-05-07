using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using System.ComponentModel;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class IoTHubDeserializerTests : ResourceDeserializerTest<IoTHubDeserializer>
    {
        private readonly IoTHubDeserializer _deserializer;

        public IoTHubDeserializerTests()
        {
            _deserializer = new IoTHubDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_IoTHubNameSupplied_SetsIoTHubName()
        {
            const string iotHubName = "promitor-iot-hub";
            YamlAssert.PropertySet<IoTHubResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"ioTHubName: {iotHubName}",
                iotHubName,
                r => r.IoTHubName);
        }

        [Fact]
        public void Deserialize_IoTHubNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<IoTHubResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.IoTHubName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new IoTHubDeserializer(Logger);
        }
    }
}