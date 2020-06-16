using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class FileStorageDeserializer : ResourceDeserializer<FileStorageResourceV1>
    {
        public FileStorageDeserializer(ILogger<FileStorageDeserializer> logger) : base(logger)
        {
            Map(resource => resource.AccountName)
                .IsRequired();
        }
    }
}
