using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class StorageQueueDeserializer : ResourceDeserializer
    {
        private const string AccountNameTag = "accountName";
        private const string QueueNameTag = "queueName";
        private const string SasTokenTag = "sasToken";

        private readonly IDeserializer<SecretV2> _secretDeserializer;

        public StorageQueueDeserializer(IDeserializer<SecretV2> secretDeserializer, ILogger logger) : base(logger)
        {
            _secretDeserializer = secretDeserializer;
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new StorageQueueResourceV2
            {
                AccountName = GetString(node, AccountNameTag),
                QueueName = GetString(node, QueueNameTag),
                SasToken = DeserializeSasToken(node)
            };
        }

        private SecretV2 DeserializeSasToken(YamlMappingNode node)
        {
            if (node.Children.TryGetValue(SasTokenTag, out var sasTokenNode))
            {
                return _secretDeserializer.Deserialize((YamlMappingNode) sasTokenNode);
            }

            return null;
        }
    }
}
