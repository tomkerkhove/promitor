using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ServiceBusQueueDeserializer : ResourceDeserializer
    {
        private const string QueueNameTag = "queueName";
        private const string NamespaceTag = "namespace";

        public ServiceBusQueueDeserializer(ILogger<ServiceBusQueueDeserializer> logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var queueName = node.GetString(QueueNameTag);
            var @namespace = node.GetString(NamespaceTag);

            return new ServiceBusQueueResourceV1
            {
                QueueName = queueName,
                Namespace = @namespace
            };
        }
    }
}
