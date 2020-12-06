﻿using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ServiceBusNamespaceDeserializerTests : ResourceDeserializerTest<ServiceBusNamespaceDeserializer>
    {
        private readonly ServiceBusNamespaceDeserializer _deserializer;

        public ServiceBusNamespaceDeserializerTests()
        {
            _deserializer = new ServiceBusNamespaceDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_QueueNameSupplied_SetsQueueName()
        {
            YamlAssert.PropertySet<ServiceBusNamespaceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "queueName: promitor-queue",
                "promitor-queue",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_QueueNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ServiceBusNamespaceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_QueueNameNotSupplied_ReportsNoError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsNoErrorForProperty(
                _deserializer,
                node,
                "queueName");
        }

        [Fact]
        public void Deserialize_NamespaceSupplied_SetsNamespace()
        {
            YamlAssert.PropertySet<ServiceBusNamespaceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "namespace: promitor-sb",
                "promitor-sb",
                r => r.Namespace);
        }

        [Fact]
        public void Deserialize_NamespaceNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ServiceBusNamespaceResourceV1, AzureResourceDefinitionV1>(
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
            return new ServiceBusNamespaceDeserializer(Logger);
        }
    }
}
