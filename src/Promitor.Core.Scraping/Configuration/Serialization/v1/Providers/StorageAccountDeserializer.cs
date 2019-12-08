using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class StorageAccountDeserializer : ResourceDeserializer<StorageAccountResourceV1>
    {
        private const string AccountNameTag = "accountName";

        public StorageAccountDeserializer(ILogger<StorageAccountDeserializer> logger) : base(logger)
        {
        }

        protected override StorageAccountResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var accountName = node.GetString(AccountNameTag);

            return new StorageAccountResourceV1
            {
                AccountName = accountName
            };
        }
    }
}