using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class MariaDbDeserializer : ResourceDeserializer<MariaDbResourceV1>
    {
        public MariaDbDeserializer(ILogger<MariaDbDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ServerName)
                .IsRequired();
        }
    }
}
