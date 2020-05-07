using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class DeviceProvisioningServiceDeserializer : ResourceDeserializer<DeviceProvisioningServiceResourceV1>
    {
        public DeviceProvisioningServiceDeserializer(ILogger<DeviceProvisioningServiceDeserializer> logger) : base(logger)
        {
            MapRequired(resource => resource.DeviceProvisioningServiceName);
        }
    }
}
