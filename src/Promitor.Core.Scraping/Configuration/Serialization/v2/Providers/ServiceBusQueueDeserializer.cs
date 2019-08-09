using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class ServiceBusQueueDeserializer : ResourceDeserializer
    {
        private const string QueueNameTag = "queueName";
        private const string NamespaceTag = "namespace";

        public ServiceBusQueueDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new ServiceBusQueueResourceV2
            {
                QueueName = GetString(node, QueueNameTag),
                Namespace = GetString(node, NamespaceTag)
            };
        }
    }
}
