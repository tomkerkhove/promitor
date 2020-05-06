using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class IoTHubDeserializer : ResourceDeserializer<IoTHubResourceV1>
    {
        public IoTHubDeserializer(ILogger<IoTHubDeserializer> logger) : base(logger)
        {
            MapRequired(resource => resource.IoTHubName);
        }
    }
}
