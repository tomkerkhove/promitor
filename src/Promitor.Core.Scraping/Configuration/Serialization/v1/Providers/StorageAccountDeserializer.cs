using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class StorageAccountDeserializer : ResourceDeserializer<StorageAccountResourceV1>
    {
        public StorageAccountDeserializer(ILogger<StorageAccountDeserializer> logger) : base(logger)
        {
            Map(resource => resource.AccountName)
                .IsRequired();
        }
    }
}