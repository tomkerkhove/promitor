using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class StorageQueueDeserializer : ResourceDeserializer
    {
        private const string AccountNameTag = "accountName";
        private const string QueueNameTag = "queueName";
        private const string SasTokenTag = "sasToken";

        private readonly IDeserializer<SecretV1> _secretDeserializer;

        public StorageQueueDeserializer(IDeserializer<SecretV1> secretDeserializer, ILogger logger) : base(logger)
        {
            _secretDeserializer = secretDeserializer;
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var accountName = node.GetString(AccountNameTag);
            var queueName = node.GetString(QueueNameTag);
            var sasToken = node.DeserializeChild(SasTokenTag, _secretDeserializer);

            return new StorageQueueResourceV1
            {
                AccountName = accountName,
                QueueName = queueName,
                SasToken = sasToken
            };
        }
    }
}
