using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ServiceBusQueueDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly ServiceBusQueueDeserializer _deserializer;

        public ServiceBusQueueDeserializerTests()
        {
            _deserializer = new ServiceBusQueueDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_QueueNameSupplied_SetsQueueName()
        {
            DeserializerTestHelpers.AssertPropertySet<ServiceBusQueueResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "queueName: promitor-queue",
                "promitor-queue",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_QueueNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<ServiceBusQueueResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_NamespaceSupplied_SetsNamespace()
        {
            DeserializerTestHelpers.AssertPropertySet<ServiceBusQueueResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "namespace: promitor-sb",
                "promitor-sb",
                r => r.Namespace);
        }

        [Fact]
        public void Deserialize_NamespaceNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<ServiceBusQueueResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Namespace);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ServiceBusQueueDeserializer(new Mock<ILogger>().Object);
        }
    }
}
