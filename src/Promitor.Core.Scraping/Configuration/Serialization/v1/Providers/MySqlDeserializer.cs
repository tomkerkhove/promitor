using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts.ResourceTypes.Enums;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class MySqlDeserializer : ResourceDeserializer<MySqlResourceV1>
    {
        public MySqlDeserializer(ILogger<MySqlDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ServerName)
                .IsRequired();
            
            // Not marking as optional for backwards compatibility
            // 'Single' should be the default
            Map(resource => resource.Type).WithDefault(MySqlServerType.Single); 
        }
    }
}
