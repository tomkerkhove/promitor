using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class BlobStorageDeserializer : StorageAccountDeserializer
    {
        public BlobStorageDeserializer(ILogger<BlobStorageDeserializer> logger) : base(logger)
        {
        }

        protected override StorageAccountResourceV1 DeserializeResource(YamlMappingNode node)
        {
            var storageAccountResource = base.DeserializeResource(node);

            return new BlobStorageResourceV1(storageAccountResource);
        }
    }
}