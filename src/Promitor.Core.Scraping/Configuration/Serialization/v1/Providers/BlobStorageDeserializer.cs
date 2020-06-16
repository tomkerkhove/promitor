using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class BlobStorageDeserializer : ResourceDeserializer<BlobStorageResourceV1>
    {
        public BlobStorageDeserializer(ILogger<BlobStorageDeserializer> logger) : base(logger)
        {
            Map(resource => resource.AccountName)
                .IsRequired();
        }
    }
}