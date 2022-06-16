using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class MySqlDeserializer : ResourceDeserializer<MySqlResourceV1>
    {
        public MySqlDeserializer(ILogger<MySqlDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ServerName)
                .IsRequired();

            Map(resource => resource.Type)
                .IsRequired();
        }
    }
}
