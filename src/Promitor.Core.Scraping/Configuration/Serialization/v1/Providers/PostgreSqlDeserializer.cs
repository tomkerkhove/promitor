using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class PostgreSqlDeserializer : ResourceDeserializer<PostgreSqlResourceV1>
    {
        public PostgreSqlDeserializer(ILogger<PostgreSqlDeserializer> logger) : base(logger)
        {
            MapRequired(resource => resource.ServerName);
        }
    }
}
