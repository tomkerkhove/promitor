using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class StorageQueueDeserializer : StorageAccountDeserializer
    {
        private const string QueueNameTag = "queueName";
        private const string SasTokenTag = "sasToken";

        private readonly IDeserializer<SecretV1> _secretDeserializer;

        public StorageQueueDeserializer(IDeserializer<SecretV1> secretDeserializer, ILogger<StorageQueueDeserializer> logger) : base(logger)
        {
            _secretDeserializer = secretDeserializer;
        }

        protected override StorageAccountResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var storageAccountResource = base.DeserializeResource(node, errorReporter);

            var queueName = node.GetString(QueueNameTag);
            var sasToken = node.DeserializeChild(SasTokenTag, _secretDeserializer, errorReporter);

            var storageQueueResource = new StorageQueueResourceV1(storageAccountResource)
            {
                QueueName = queueName,
                SasToken = sasToken
            };

            return storageQueueResource;
        }
    }
}