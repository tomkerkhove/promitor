using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ServiceBusQueueDeserializerTests : ResourceDeserializerTest
    {
        private readonly ServiceBusQueueDeserializer _deserializer;

        public ServiceBusQueueDeserializerTests()
        {
            _deserializer = new ServiceBusQueueDeserializer(NullLogger.Instance);
        }

        [Fact]
        public void Deserialize_QueueNameSupplied_SetsQueueName()
        {
            YamlAssert.PropertySet<ServiceBusQueueResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "queueName: promitor-queue",
                "promitor-queue",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_QueueNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ServiceBusQueueResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_NamespaceSupplied_SetsNamespace()
        {
            YamlAssert.PropertySet<ServiceBusQueueResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "namespace: promitor-sb",
                "promitor-sb",
                r => r.Namespace);
        }

        [Fact]
        public void Deserialize_NamespaceNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ServiceBusQueueResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Namespace);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ServiceBusQueueDeserializer(NullLogger.Instance);
        }
    }
}
