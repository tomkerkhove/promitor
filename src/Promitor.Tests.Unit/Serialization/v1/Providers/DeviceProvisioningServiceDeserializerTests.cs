using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using System.ComponentModel;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class DeviceProvisioningServiceDeserializerTests : ResourceDeserializerTest<DeviceProvisioningServiceDeserializer>
    {
        private readonly DeviceProvisioningServiceDeserializer _deserializer;

        public DeviceProvisioningServiceDeserializerTests()
        {
            _deserializer = new DeviceProvisioningServiceDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_DeviceProvisioningServiceNameSupplied_SetsDeviceProvisioningServiceName()
        {
            const string deviceProvisioningServiceName = "promitor-dps";
            YamlAssert.PropertySet<DeviceProvisioningServiceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"deviceProvisioningServiceName: {deviceProvisioningServiceName}",
                deviceProvisioningServiceName,
                r => r.DeviceProvisioningServiceName);
        }

        [Fact]
        public void Deserialize_DeviceProvisioningServiceNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<DeviceProvisioningServiceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.DeviceProvisioningServiceName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new DeviceProvisioningServiceDeserializer(Logger);
        }
    }
}