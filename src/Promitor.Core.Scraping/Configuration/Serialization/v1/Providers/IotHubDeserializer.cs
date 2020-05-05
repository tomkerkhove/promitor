using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class IotHubDeserializer : ResourceDeserializer<IotHubResourceV1>
    {
        public IotHubDeserializer(ILogger<IotHubDeserializer> logger) : base(logger)
        {
            MapRequired(resource => resource.IotHubName);
        }
    }
}
