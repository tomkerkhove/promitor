using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class CosmosDbDeserializer : ResourceDeserializer<CosmosDbResourceV1>
    {
        public CosmosDbDeserializer(ILogger<CosmosDbDeserializer> logger) : base(logger)
        {
            Map(resource => resource.DbName)
                .IsRequired();
        }
    }
}
