using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class WebAppDeserializer : ResourceDeserializer<WebAppResourceV1>
    {
        public WebAppDeserializer(ILogger<WebAppDeserializer> logger) : base(logger)
        {
            Map(resource => resource.WebAppName)
                .IsRequired();
            Map(resource => resource.SlotName);
        }
    }
}
