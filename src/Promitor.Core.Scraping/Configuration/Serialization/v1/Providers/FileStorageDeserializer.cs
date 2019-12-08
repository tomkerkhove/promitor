using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class FileStorageDeserializer : StorageAccountDeserializer
    {
        public FileStorageDeserializer(ILogger<FileStorageDeserializer> logger) : base(logger)
        {
        }

        protected override StorageAccountResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var storageAccountResource = base.DeserializeResource(node, errorReporter);

            return new FileStorageResourceV1(storageAccountResource);
        }
    }
}
