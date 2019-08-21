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

        public ServiceBusQueueDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            return new ServiceBusQueueResourceV1
            {
                QueueName = GetString(node, QueueNameTag),
                Namespace = GetString(node, NamespaceTag)
            };
        }
    }
}
