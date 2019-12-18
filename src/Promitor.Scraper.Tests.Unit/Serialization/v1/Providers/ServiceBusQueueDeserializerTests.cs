using System.ComponentModel;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ServiceBusQueueDeserializerTests : ResourceDeserializerTest<ServiceBusQueueDeserializer>
    {
        private readonly ServiceBusQueueDeserializer _deserializer;
        private readonly Mock<IErrorReporter> _errorReporter = new Mock<IErrorReporter>();

        public ServiceBusQueueDeserializerTests()
        {
            _deserializer = new ServiceBusQueueDeserializer(Logger);
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
        public void Deserialize_QueueNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(r => r.ReportError(node, It.Is<string>(s => s.Contains("queueName"))));
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

        [Fact]
        public void Deserialize_NamespaceNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(r => r.ReportError(node, It.Is<string>(s => s.Contains("namespace"))));
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ServiceBusQueueDeserializer(Logger);
        }
    }
}
