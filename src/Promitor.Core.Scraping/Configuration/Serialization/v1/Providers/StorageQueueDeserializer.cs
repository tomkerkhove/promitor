using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class StorageQueueDeserializer : ResourceDeserializer<StorageQueueResourceV1>
    {
        public StorageQueueDeserializer(IDeserializer<SecretV1> secretDeserializer, ILogger<StorageQueueDeserializer> logger) : base(logger)
        {
            Map(resource => resource.AccountName)
                .IsRequired();
            Map(resource => resource.QueueName)
                .IsRequired();
            Map(resource => resource.SasToken)
                .IsRequired()
                .MapUsingDeserializer(secretDeserializer);
        }
    }
}