using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class EventHubsDeserializerTests : ResourceDeserializerTest<EventHubsDeserializer>
    {
        private readonly EventHubsDeserializer _deserializer;

        public EventHubsDeserializerTests()
        {
            _deserializer = new EventHubsDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_TopicNameSupplied_SetsTopicName()
        {
            const string topicName = "promitor-topic";
            YamlAssert.PropertySet<EventHubsResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"topicName: {topicName}",
                topicName,
                r => r.TopicName);
        }

        [Fact]
        public void Deserialize_TopicNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<EventHubsResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.TopicName);
        }

        [Fact]
        public void Deserialize_TopicNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsNoErrorForProperty(
                _deserializer,
                node,
                "topicName");
        }

        [Fact]
        public void Deserialize_NamespaceSupplied_SetsNamespace()
        {
            const string namespaceName = "promitor-event-hubs";
            YamlAssert.PropertySet<EventHubsResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"namespace: {namespaceName}",
                namespaceName,
                r => r.Namespace);
        }

        [Fact]
        public void Deserialize_NamespaceNotSupplied_Null()
        {
            YamlAssert.PropertyNull<EventHubsResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Namespace);
        }

        [Fact]
        public void Deserialize_NamespaceNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "namespace");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new EventHubsDeserializer(Logger);
        }
    }
}
