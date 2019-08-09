using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Providers
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
            DeserializerTestHelpers.AssertPropertySet<ServiceBusQueueResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "queueName: promitor-queue",
                "promitor-queue",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_QueueNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<ServiceBusQueueResourceV2, AzureResourceDefinitionV2>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_NamespaceSupplied_SetsNamespace()
        {
            DeserializerTestHelpers.AssertPropertySet<ServiceBusQueueResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "namespace: promitor-sb",
                "promitor-sb",
                r => r.Namespace);
        }

        [Fact]
        public void Deserialize_NamespaceNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<ServiceBusQueueResourceV2, AzureResourceDefinitionV2>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Namespace);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new ServiceBusQueueDeserializer(new Mock<ILogger>().Object);
        }
    }
}
